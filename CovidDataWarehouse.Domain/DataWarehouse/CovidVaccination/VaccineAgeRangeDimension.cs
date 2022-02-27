﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain
{
    [Table("")]
    public class VaccineAgeRangeDimension
    {
        [Key]
        public int AgeRangeId { get; set; }

        public string AgeRangeName { get; set; }
    }
}
