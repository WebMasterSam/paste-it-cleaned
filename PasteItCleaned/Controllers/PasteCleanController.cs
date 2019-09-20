using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PasteItCleaned.Cleaners;
using PasteItCleaned.Cleaners.Office.Excel;
using PasteItCleaned.Cleaners.Office.PowerPoint;
using PasteItCleaned.Cleaners.Office.Word;
using PasteItCleaned.Cleaners.Web;

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
            // MultiThread where applicable

            var content = "";

            EnsureCleaners();

            try
            {
                content = obj.value;

                if (ApiKeyPresent())
                {
                    if (ApiKeyValid())
                    {
                        if (ApiKeyFitsWithDomain())
                        {
                            return Clean(content);
                        }
                        else
                            return "[PasteItCleaned ERR-012]: The domain where the plugin was used (XXX) is not configure for the account with API key (YYY).";
                    }
                    else
                        return "[PasteItCleaned ERR-011]: Your API key is invalid (YYY).";
                }

                return "[PasteItCleaned ERR-010]: You forgot to add an API key to your script tag.";
            }
            catch (Exception ex)
            {
                // Error handling

                return content;
            }
        }

        private void SaveStat(SourceType type)
        {
            // call DB to increase stat: type, date
        }

        private string Clean(string content)
        {
            foreach (BaseCleaner cleaner in Cleaners)
            {
                if (cleaner.CanClean(content))
                {
                    SaveStat(cleaner.GetSourceType());

                    return cleaner.Clean(content, content);
                }
            }

            return content;
        }

        private bool ApiKeyFitsWithDomain()
        {
            return true;
        }

        private bool ApiKeyPresent()
        {
            // read api key from headers
            return true;
        }

        private bool ApiKeyValid()
        {
            // read api key from headers
            return true;
        }

        private string GetApiKey()
        {
            // read api key from headers
            return "";
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
