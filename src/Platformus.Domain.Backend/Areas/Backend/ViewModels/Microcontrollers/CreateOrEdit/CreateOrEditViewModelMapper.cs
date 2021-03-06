﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Platformus.Barebone;
using Platformus.Domain.Data.Abstractions;
using Platformus.Domain.Data.Models;
using Platformus.Globalization.Backend.ViewModels;

namespace Platformus.Domain.Backend.ViewModels.Microcontrollers
{
  public class CreateOrEditViewModelMapper : ViewModelFactoryBase
  {
    public CreateOrEditViewModelMapper(IRequestHandler requestHandler)
      : base(requestHandler)
    {
    }

    public Microcontroller Map(CreateOrEditViewModel createOrEdit)
    {
      Microcontroller microcontroller = new Microcontroller();

      if (createOrEdit.Id != null)
        microcontroller = this.RequestHandler.Storage.GetRepository<IMicrocontrollerRepository>().WithKey((int)createOrEdit.Id);

      microcontroller.Name = createOrEdit.Name;
      microcontroller.UrlTemplate = createOrEdit.UrlTemplate;
      microcontroller.ViewName = createOrEdit.ViewName;
      microcontroller.CSharpClassName = createOrEdit.CSharpClassName;
      microcontroller.Position = createOrEdit.Position;
      return microcontroller;
    }
  }
}