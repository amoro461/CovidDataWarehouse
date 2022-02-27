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
    public class VaccinePopulationDimension
    {
        [Key]
        public int PopulationId { get; set; }

        public int Population { get; set; }
    }
}
