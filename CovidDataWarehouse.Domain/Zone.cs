using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CovidDataWarehouse.Domain
{
    [Table("zone")]
    public class Zone
    {
        [Key]
        [Column("zone_id")]
        public int ZoneId { get; set; }

        [Column("zone_name")]
        public string ZoneName { get; set; }
    }
}
