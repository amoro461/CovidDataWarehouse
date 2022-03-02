using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Case_Zone_Dimension")]
    public class CaseZoneDimension
    {
        [Key]
        [Column("Case_Zone_Id")]
        public int CaseZoneId { get; set; }

        [Column("AHS_Zone_Name")]
        public string AHSZoneName { get; set; }
    }
}
