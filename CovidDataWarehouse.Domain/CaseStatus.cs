using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain
{
    [Table("case_status")]
    public class CaseStatus
    {
        [Key]
        [Column("case_status_id")]
        public int CaseStatusId { get; set; }

        [Column("case_status_name")]
        public string CaseStatusName { get; set; }
    }
}
