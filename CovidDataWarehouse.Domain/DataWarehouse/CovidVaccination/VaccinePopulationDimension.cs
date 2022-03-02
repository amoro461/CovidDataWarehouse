using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Vaccine_Population_Dimension")]
    public class VaccinePopulationDimension
    {
        [Key]
        [Column("Vaccine_Population_Id")]
        public int VaccinePopulationId { get; set; }

        [Column("Population")]
        public int Population { get; set; }
    }
}
