using Microsoft.EntityFrameworkCore;
using CovidDataWarehouse.Domain.DataWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Data
{
    public class CovidDataWarehouseContext:DbContext
    {
        public CovidDataWarehouseContext(DbContextOptions<CovidDataWarehouseContext> options) : base(options)
        { }

        public DbSet<CaseAgeRangeDimension> CaseAgeRangeDimensions { get; set; }
        public DbSet<CaseDateDimension> CaseDateDimensions { get; set; }
        public DbSet<CaseStatusDimension> CaseStatusDimensions { get; set; }
        public DbSet<CaseZoneDimension> CaseZoneDimensions { get; set; }
        public DbSet<CovidCaseFact> CovidCaseFacts { get; set; }

        public DbSet<CovidVaccinationFact> CovidVaccinationFacts { get; set; }
        public DbSet<VaccineAgeRangeDimension> VaccineAgeRangeDimensions { get; set; }
        public DbSet<VaccineDosageDimension> VaccineDosageDimensions { get; set; }
        public DbSet<VaccinePopulationDimension> VaccinePopulationDimensions { get; set; }
        public DbSet<VaccineZoneDimension> VaccineZoneDimensions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("optionsBuilder is not configured.");
            }
        }
    }
}
