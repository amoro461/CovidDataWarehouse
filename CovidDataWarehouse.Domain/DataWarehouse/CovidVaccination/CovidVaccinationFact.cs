using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Covid_Vaccine_Fact")]
    public class CovidVaccinationFact
    {
        [Key]
        [Column("Covid_Vaccine_Id")]
        public int CovidVaccinationId { get; set; }

        [Column("Vaccine_Zone_Id")]
        public int VaccineZoneId { get; set; }

        [Column("Vaccine_Age_Range_Id")]
        public int VaccineAgeRangeId { get; set; }

        [Column("Vaccine_Population_Id")]
        public int VaccinePopulationId { get; set; }

        [Column("Vaccine_Dosage_Id")]
        public int VaccineDosageId { get; set; }
    }
}
