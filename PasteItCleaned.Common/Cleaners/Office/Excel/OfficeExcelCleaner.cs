﻿using PasteItCleaned.Core.Entities;
using PasteItCleaned.Core.Models;

namespace PasteItCleaned.Plugin.Cleaners.Office.Excel
{
    public class OfficeExcelCleaner : OfficeBaseCleaner
    {
        public override SourceType GetSourceType()
        {
            return SourceType.Excel;
        }

        public override bool CanClean(string html, string rtf)
        {
            return html.ToLower().Contains("<meta name=Generator content=\"Microsoft Excel".ToLower());
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            return base.Clean(html, rtf, config, keepStyles);
        }
    }
}
