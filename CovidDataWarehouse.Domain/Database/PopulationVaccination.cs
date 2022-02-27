using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.Database
{
    [Table("population_vaccination")]
    public class PopulationVaccination
    {
        [Key]
        [Column("population_vaccination_id")]
        public int PopulationVaccinationId { get; set; }

        [Column("zone_id")]
        public int ZoneId { get; set; }

        [Column("age_range_id")]
        public int AgeRangeId { get; set; }

        [Column("population")]
        public int Population { get; set; }

        [Column("single_dose")]
        public int SingleDose { get; set; }

        [Column("double_dose")]
        public int DoubleDose { get; set; }

        [Column("triple_dose")]
        public int TripleDose { get; set; }

        public Zone Zone { get; set; }

        public AgeRange AgeRange { get; set; }
    }
}
