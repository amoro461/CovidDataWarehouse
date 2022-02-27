using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.Database
{
    [Table("age_range")]
    public class AgeRange
    {
        [Key]
        [Column("age_range_id")]
        public int AgeRangeId { get; set; }

        [Column("age_range_name")]
        public string AgeRangeName { get; set; }
    }
}
