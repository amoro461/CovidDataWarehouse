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
    public class VaccineDosageDimension
    {
        [Key]
        public int DosageId { get; set; }
        public int OneDoseVaccinated { get; set; }
        public int TwoDoseVaccinated { get; set; }
        public int ThreeDoseVaccinated { get; set; }
    }
}
