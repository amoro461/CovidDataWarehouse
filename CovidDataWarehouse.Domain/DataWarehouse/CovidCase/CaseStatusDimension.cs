using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Domain.DataWarehouse
{
    [Table("Case_Status_Dimension")]
    public class CaseStatusDimension
    {
        [Key]
        [Column("Case_Status_Id")]
        public int CaseStatusId { get; set; }

        [Column("Status_Name")]
        public string StatusName { get; set; }
    }
}
