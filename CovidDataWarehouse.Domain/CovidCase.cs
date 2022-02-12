using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain
{
    [Table("covid_case")]
    public class CovidCase
    {
        [Key]
        [Column("covid_case_id")]
        public int CovidCaseId { get; set; }

        [Column("zone_id")]
        public int ZoneId { get; set; }

        [Column("age_range_id")]
        public int AgeRangeId { get; set; }

        [Column("case_status_id")]
        public int CaseStatusId { get; set; }

        [Column("isFemale")]
        public bool IsFemale { get; set; }

        [Column("isConfirmed")]
        public bool IsConfirmed { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        public Zone Zone { get; set; }

        public CaseStatus CaseStatus { get; set; }

        public AgeRange AgeRange { get; set; }
    }
}
