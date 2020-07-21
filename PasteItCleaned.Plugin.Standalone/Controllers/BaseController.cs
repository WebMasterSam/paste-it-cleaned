using System;
using System.Collections;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PasteItCleaned.Plugin.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }

        protected void LogError(Exception ex)
        {
            LogError(Guid.Empty, ex);
        }

        protected void LogError(Guid clientId, Exception ex)
        {
            try
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                foreach (DictionaryEntry entry in ex.Data)
                    Console.WriteLine(string.Format("{0} = {1}", entry.Key, entry.Value));
            }
            catch { }

            if (ex.InnerException != null)
                LogError(clientId, ex.InnerException);
        }


        protected ILogger Log { get; }
    }
}
