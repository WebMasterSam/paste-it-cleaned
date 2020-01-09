using System;

namespace PasteItCleaned.Backend.Entities
{
    public class HitDaily
    {
        public Guid HitDailyId { get; set; }
        public DateTime Date { get; set; }
        public int CountExcel { get; set; }
        public int CountWord { get; set; }
        public int CountPowerPoint { get; set; }
        public int CountWeb { get; set; }
        public int CountText { get; set; }
        public int CountImage { get; set; }
        public int CountOther { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
