using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain
{
    [Table("")]
    class CaseAgeRangeDimension
    {
        [Key]
        public int CaseAgeRangeId { get; set; }

        public string AgeRangeName { get; set; }
    }
}
