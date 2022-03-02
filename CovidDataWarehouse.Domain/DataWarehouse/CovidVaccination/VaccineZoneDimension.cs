using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Vaccine_Zone_Dimension")]
    public class VaccineZoneDimension
    {
        [Key]
        [Column("Vaccine_Zone_Id")]
        public int VaccineZoneId { get; set; }

        [Column("Zone_Name")]
        public string ZoneName { get; set; }

        [Column("Local_Name")]
        public string LocalName { get; set; }

        [Column("Local_Code")]
        public string LocalCode { get; set; }
    }
}
