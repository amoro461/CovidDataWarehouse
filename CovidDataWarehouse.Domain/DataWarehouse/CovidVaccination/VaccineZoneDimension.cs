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
    public class VaccineZoneDimension
    {
        [Key]
        public int ZoneId { get; set; }

        public string ZoneName { get; set; }

        public string LocalName { get; set; }

        public string LocalCode { get; set; }
    }
}
