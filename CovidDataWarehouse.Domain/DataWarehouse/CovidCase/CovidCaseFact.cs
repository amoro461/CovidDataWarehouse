using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Covid_Cases_Fact")]
    public class CovidCaseFact
    {
        [Key]
        [Column("Case_Covid_Id")]
        public int CovidCaseId { get; set; }

        [Column("Case_Zone_Id")]
        public int CaseZoneId { get; set; }

        [Column("Case_Age_Range_Id")]
        public int CaseAgeRangeId { get; set; }

        [Column("Case_Status_Id")]
        public int CaseStatusId { get; set; }

        [Column("Case_Date_Id")]
        public int CaseDateId { get; set; }

        [Column("IsFemale")]
        public bool IsFemale { get; set; }

        [Column("IsConfirmed")]
        public bool IsConfirmed { get; set; }
    }
}
