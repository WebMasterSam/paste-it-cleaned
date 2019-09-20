using PasteItCleaned.Cleaners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteItCleaned.Helpers
{
    public static class DbHelper
    {
        public static void SaveStat(SourceType type)
        {
            // call DB to increase stat: type, date
            // async; should never block process just to get stat
        }
    }
}
