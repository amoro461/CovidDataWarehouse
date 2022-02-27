using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("")]
    public class CovidCaseFact
    {
        [Key]
        public int CovidCaseId { get; set; }

        public int CaseZoneId { get; set; }

        public int CaseAgeRangeId { get; set; }

        public int CaseStatusId { get; set; }

        public int DateId { get; set; }

        public bool IsFemale { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
