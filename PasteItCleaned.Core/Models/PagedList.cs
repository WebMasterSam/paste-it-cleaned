using System;
using System.Collections.Generic;

namespace PasteItCleaned.Core.Models
{
    public abstract class PagedListBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    public class PagedList<T> : PagedListBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedList()
        {
            Results = new List<T>();
        }
    }
}
