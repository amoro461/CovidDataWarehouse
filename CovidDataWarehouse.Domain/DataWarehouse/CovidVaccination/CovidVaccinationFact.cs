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
    public class CovidVaccinationFact
    {
        [Key]
        public int CovidVaccinationId { get; set; }
        public int VaccineZoneId { get; set; }
        public int VaccineAgeRangeId { get; set; }
        public int PopulationId { get; set; }
        public int DosageId { get; set; }
    }
}
