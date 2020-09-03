using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Core.Models
{
    public class HitDaily
    {
        [Column("hit_daily_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid HitDailyId { get; set; }

        [Column("client_id", TypeName = "char(36)")]
        public Guid ClientId { get; set; }

        [Column("hit_daily_date")]
        public DateTime Date { get; set; }

        [Column("count_excel")]
        public int CountExcel { get; set; }

        [Column("count_word")]
        public int CountWord { get; set; }

        [Column("count_powerpoint")]
        public int CountPowerPoint { get; set; }

        [Column("count_web")]
        public int CountWeb { get; set; }

        [Column("count_text")]
        public int CountText { get; set; }

        [Column("count_image")]
        public int CountImage { get; set; }

        [Column("count_rtf")]
        public int CountRtf { get; set; }

        [Column("count_openoffice")]
        public int CountOpenOffice { get; set; }

        [Column("count_libreoffice")]
        public int CountLibreOffice { get; set; }

        [Column("count_google")]
        public int CountGoogle { get; set; }

        [Column("count_google_sheets")]
        public int CountGoogleSheets { get; set; }

        [Column("count_google_docs")]
        public int CountGoogleDocs { get; set; }

        [Column("count_other")]
        public int CountOther { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }
    }
}
