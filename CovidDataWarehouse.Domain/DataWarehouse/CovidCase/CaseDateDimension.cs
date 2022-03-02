using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Case_Date_Dimension")]
    public class CaseDateDimension
    {
        public CaseDateDimension(DateTime date)
        {
            this.Day = date.Day;
            this.Month = date.Month;
            this.Year = date.Year;
            this.DayOfWeek = date.DayOfWeek.ToString();
            this.Date = date;
        }

        [Key]
        [Column("Case_Date_Id")]
        public int CaseDateId { get; set; }

        [Column("Day_Of_Week")]
        public string DayOfWeek { get; set; }

        [Column("Day")]
        public int Day { get; set; }

        [Column("Month")]
        public int Month { get; set; }

        [Column("Year")]
        public int Year { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }
    }
}
