using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidDataWarehouse.Domain.Database
{
    [Table("zone")]
    public class Zone
    {
        [Key]
        [Column("zone_id")]
        public int ZoneId { get; set; }

        [Column("zone_name")]
        public string ZoneName { get; set; }

        [Column("local_name")]
        public string LocalName { get; set; }

        [Column("local_code")]
        public string LocalCode { get; set; }
    }
}
