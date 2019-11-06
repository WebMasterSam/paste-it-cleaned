﻿namespace PasteItCleaned.Common.Controllers.Entities
{
    public class Error
    {
        public Error(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
        public string Exception { get { return "editor.alert.failure"; } }
    }
}