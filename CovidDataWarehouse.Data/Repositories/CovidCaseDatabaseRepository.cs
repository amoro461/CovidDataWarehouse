using CovidDataWarehouse.Domain.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDataWarehouse.Data
{
    public class CovidCaseDatabaseRepository
    {
        private CovidDatabaseContext _context = null;

        public CovidCaseDatabaseRepository(CovidDatabaseContext covidDataWarehouseContext)
        {
            _context = covidDataWarehouseContext;
        }

        public List<CovidCase> GetCovidCases()
        {
            return _context.Set<CovidCase>()
                .Include(x => x.AgeRange)
                .Include(x => x.CaseStatus)
                .Include(x => x.Zone)
                .ToList();
        }
    }
}
