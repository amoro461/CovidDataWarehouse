using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Case_Age_Range_Dimension")]
    public class CaseAgeRangeDimension
    {
        [Key]
        [Column("Case_Age_Range_Id")]
        public int CaseAgeRangeId { get; set; }

        [Column("Age_Range_Name")]
        public string AgeRangeName { get; set; }
    }
}
