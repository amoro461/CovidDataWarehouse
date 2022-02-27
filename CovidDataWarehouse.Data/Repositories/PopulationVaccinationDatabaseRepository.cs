using CovidDataWarehouse.Domain.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Data
{
    public class PopulationVaccinationDatabaseRepository
    {
        private CovidDatabaseContext _context = null;

        public PopulationVaccinationDatabaseRepository(CovidDatabaseContext covidDataWarehouseContext)
        {
            _context = covidDataWarehouseContext;
        }

        public List<PopulationVaccination> GetPopulationVaccinations()
        {
            return _context.Set<PopulationVaccination>()
                .Include(x => x.AgeRange)
                .Include(x => x.Zone)
                .ToList();
        }
    }
}
