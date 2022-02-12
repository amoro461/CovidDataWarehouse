using CovidDataWarehouse.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Data
{
    public class CovidDataWarehouseContext:DbContext
    {
        public CovidDataWarehouseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<PopulationVaccination> PopulationVaccinations { get; set; }
        public DbSet<AgeRange> AgeRanges { get; set; }
        public DbSet<CaseStatus> CaseStatuses { get; set; }
        public DbSet<CovidCase> CovidCases { get; set; }
        public DbSet<Zone> Zones { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("optionsBuilder is not configured.");
            }
        }
    }
}
