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
    public class CaseZoneDimension
    {
        [Key]
        public int CaseZoneId { get; set; }

        public string AHSZoneName { get; set; }
    }
}
