using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Vaccine_Dosage_Dimension")]
    public class VaccineDosageDimension
    {
        [Key]
        [Column("Vaccine_Dosage_Id")]
        public int VaccineDosageId { get; set; }

        [Column("One_Dose_Vaccinated")]
        public int OneDoseVaccinated { get; set; }

        [Column("Two_Dose_Vaccinated")]
        public int TwoDoseVaccinated { get; set; }

        [Column("Three_Dose_Vaccinated")]
        public int ThreeDoseVaccinated { get; set; }
    }
}
