﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Events;
using Microsoft.AspNetCore.Mvc;
using Platformus.Barebone;
using Platformus.Forms.Backend.ViewModels.Forms;
using Platformus.Forms.Data.Abstractions;
using Platformus.Forms.Data.Models;

namespace Platformus.Forms.Backend.Controllers
{
  [Area("Backend")]
  public class FormsController : Platformus.Globalization.Backend.Controllers.ControllerBase
  {
    public FormsController(IStorage storage)
      : base(storage)
    {
    }

    public IActionResult Index()
    {
      return this.View(new IndexViewModelFactory(this).Create());
    }

    [HttpGet]
    [ImportModelStateFromTempData]
    public IActionResult CreateOrEdit(int? id)
    {
      return this.View(new CreateOrEditViewModelFactory(this).Create(id));
    }

    [HttpPost]
    [ExportModelStateToTempData]
    public IActionResult CreateOrEdit(CreateOrEditViewModel createOrEdit)
    {
      if (this.ModelState.IsValid)
      {
        Form form = new CreateOrEditViewModelMapper(this).Map(createOrEdit);

        this.CreateOrEditEntityLocalizations(form);

        if (createOrEdit.Id == null)
          this.Storage.GetRepository<IFormRepository>().Create(form);

        else this.Storage.GetRepository<IFormRepository>().Edit(form);

        this.Storage.Save();

        if (createOrEdit.Id == null)
          Event<IFormCreatedEventHandler, IRequestHandler, Form>.Broadcast(this, form);

        else
        {
          Event<IFormEditedEventHandler, IRequestHandler, Form, Form>.Broadcast(
            this, this.Storage.GetRepository<IFormRepository>().WithKey((int)createOrEdit.Id), form
          );
        }

        return this.RedirectToAction("Index");
      }

      return this.CreateRedirectToSelfResult();
    }

    public ActionResult Delete(int id)
    {
      Form form = this.Storage.GetRepository<IFormRepository>().WithKey(id);

      this.Storage.GetRepository<IFormRepository>().Delete(form);
      this.Storage.Save();
      Event<IFormDeletedEventHandler, IRequestHandler, Form>.Broadcast(this, form);
      return this.RedirectToAction("Index");
    }
  }
}