﻿// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Platformus.Barebone;
using Platformus.Globalization;
using Platformus.Globalization.Data.Abstractions;

namespace Platformus
{
  public static class IRequestHandlerExtensions
  {
    public static string GetLocalizationValue(this IRequestHandler requestHandler, int dictionaryId)
    {
      return requestHandler.GetLocalizationValue(dictionaryId, CultureManager.GetCurrentCulture(requestHandler.Storage).Id);
    }

    public static string GetLocalizationValue(this IRequestHandler requestHandler, int dictionaryId, int cultureId)
    {
      return requestHandler.Storage.GetRepository<ILocalizationRepository>().WithDictionaryIdAndCultureId(
        dictionaryId, cultureId
      )?.Value;
    }
  }
}