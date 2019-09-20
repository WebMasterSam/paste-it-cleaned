using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Cleaners;
using PasteItCleaned.Cleaners.Office.Excel;
using PasteItCleaned.Cleaners.Office.PowerPoint;
using PasteItCleaned.Cleaners.Office.Word;
using PasteItCleaned.Cleaners.Web;
using PasteItCleaned.Helpers;

namespace PasteItCleaned.Controllers
{
    [Route("api/v1/clean")]
    [ApiController]
    public class PasteCleanController : ControllerBase
    {
        private static List<BaseCleaner> Cleaners = new List<BaseCleaner>();

        // POST api/v1/clean
        [HttpPost()]
        public ActionResult<string> Post([FromBody] CleanObject obj)
        {
            var content = "";

            try
            {
                EnsureCleaners();

                content = obj.value;

                if (ApiKeyHelper.ApiKeyPresent())
                {
                    if (ApiKeyHelper.ApiKeyValid())
                    {
                        if (ApiKeyHelper.ApiKeyFitsWithDomain())
                        {
                            if (AccountHelper.AccountIsPaid())
                            {
                                return Clean(content);
                            }
                            else
                                return ErrorHelper.GetAccountIsUnpaid();
                        }
                        else
                            return ErrorHelper.GetApiKeyDomainNotConfigured();
                    }
                    else
                        return ErrorHelper.GetApiKeyInvalid();
                }

                return ErrorHelper.GetApiKeyAbsent();
            }
            catch (Exception ex)
            {
                // Error handling

                return content;
            }
        }

        private string Clean(string content)
        {
            foreach (BaseCleaner cleaner in Cleaners)
            {
                if (cleaner.CanClean(content))
                {
                    DbHelper.SaveStat(cleaner.GetSourceType());

                    return cleaner.Clean(content);
                }
            }

            return content;
        }

        private void EnsureCleaners()
        {
            if (Cleaners.Count == 0)
            {
                Cleaners.Add(new OfficeExcelCleaner());
                Cleaners.Add(new OfficeWordCleaner());
                Cleaners.Add(new OfficePowerPointCleaner());
                Cleaners.Add(new WebCleaner());
            }
        }
    }

    public class CleanObject
    {
        public string value { get; set; }
    }
}
