using CovidDataWarehouse.Data;
using CovidDataWarehouse.Domain.Database;
using CovidDataWarehouse.Domain.DataWarehouse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidDataWarehouse.App.Services
{
    public class DataScraperServices : Controller
    {
        private static CovidDatabaseContext _databaseContext;
        private static CovidDataWarehouseContext _dataWarehouseContext;
        private readonly CovidCaseDatabaseRepository _covidCaseDatabaseRepository;
        private readonly PopulationVaccinationDatabaseRepository _populationVaccinationDatabaseRepository;

        public DataScraperServices(CovidDatabaseContext databaseContext, CovidDataWarehouseContext dataWarehouseContext)
        {
            _databaseContext = databaseContext;
            _dataWarehouseContext = dataWarehouseContext;
            _covidCaseDatabaseRepository = new CovidCaseDatabaseRepository(databaseContext);
            _populationVaccinationDatabaseRepository = new PopulationVaccinationDatabaseRepository(databaseContext);
        }

        public async Task<string> GetAlbertaCovidData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54251/");

            string baseAddress = "https://www.alberta.ca/data/stats/covid-19-alberta-statistics-data.csv";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseAddress);
            request.Method = "GET";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<AlbertaCaseData> albertaCasesData = new List<AlbertaCaseData>();

                    string document = new StreamReader(response.GetResponseStream()).ReadToEnd().Replace("\"", "");

                    foreach (string csvLine in document.Split(new string[] { Environment.NewLine },
                        StringSplitOptions.None).Skip(1))
                    {
                        try
                        {
                            albertaCasesData.Add(new AlbertaCaseData(csvLine));
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                    }

                    List<CaseStatus> caseStatuses = _databaseContext.CaseStatuses.ToList();
                    List<Zone> zones = _databaseContext.Zones.ToList();
                    List<AgeRange> ageRanges = _databaseContext.AgeRanges.ToList();
                    CaseStatus caseStatus;
                    Zone zone;
                    AgeRange ageRange;

                    foreach (AlbertaCaseData caseData in albertaCasesData)
                    {
                        if (zones.FirstOrDefault(i => i.ZoneName.Equals(caseData.AlbertaHealthServicesZone)) == null)
                        {
                            zone = new Zone { ZoneName = caseData.AlbertaHealthServicesZone };
                            _databaseContext.Zones.Add(zone);
                            zones.Add(zone);
                        }

                        if (caseStatuses.FirstOrDefault(i => i.CaseStatusName.Equals(caseData.CaseStatus)) == null)
                        {
                            caseStatus = new CaseStatus { CaseStatusName = caseData.CaseStatus };
                            _databaseContext.CaseStatuses.Add(caseStatus);
                            caseStatuses.Add(caseStatus);
                        }

                        if (ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(caseData.AgeGroup)) == null)
                        {
                            ageRange = new AgeRange { AgeRangeName = caseData.AgeGroup };
                            _databaseContext.AgeRanges.Add(ageRange);
                            ageRanges.Add(ageRange);
                        }
                    }

                    await _databaseContext.SaveChangesAsync();

                    caseStatuses = _databaseContext.CaseStatuses.ToList();
                    zones = _databaseContext.Zones.ToList();
                    ageRanges = _databaseContext.AgeRanges.ToList();
                    int count = 0;

                    foreach (AlbertaCaseData caseData in albertaCasesData)
                    {
                        _databaseContext.CovidCases.Add(new CovidCase()
                        {
                            ZoneId = zones.FirstOrDefault(i => i.ZoneName.Equals(caseData.AlbertaHealthServicesZone)).ZoneId,
                            AgeRangeId = ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(caseData.AgeGroup)).AgeRangeId,
                            CaseStatusId = caseStatuses.FirstOrDefault(i => i.CaseStatusName.Equals(caseData.CaseStatus)).CaseStatusId,
                            IsFemale = caseData.IsFemale,
                            IsConfirmed = caseData.CaseConfirmed,
                            Date = caseData.DateReported
                        });

                        if (count == 1000)
                        {
                            await _databaseContext.SaveChangesAsync();
                            count = 0;
                        }
                        else
                        {
                            count += 1;
                        }
                    }

                    await _databaseContext.SaveChangesAsync();

                    return JsonConvert.SerializeObject(
                        new JsonResult
                        {
                            IsSucceed = true
                        });
                }
                else
                {
                    return JsonConvert.SerializeObject(
                        new JsonResult
                        {
                            IsSucceed = false
                        });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }
        }

        public async Task<string> GetAlbertaVaccineData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54251/");

            string baseAddress = "https://www.alberta.ca/data/stats/lga-coverage.csv";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseAddress);
            request.Method = "GET";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<AlbertaVaccineData> albertaVaccinesData = new List<AlbertaVaccineData>();

                    string document = new StreamReader(response.GetResponseStream()).ReadToEnd().Replace("\"", "");

                    foreach (string csvLine in document.Split(new string[] { Environment.NewLine },
                        StringSplitOptions.None).Skip(1))
                    {
                        try
                        {
                            albertaVaccinesData.Add(new AlbertaVaccineData(csvLine)); ;
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                    }

                    List<Zone> zones = _databaseContext.Zones.ToList();
                    List<AgeRange> ageRanges = _databaseContext.AgeRanges.ToList();
                    Zone zone;
                    AgeRange ageRange;

                    foreach (AlbertaVaccineData vaccineData in albertaVaccinesData)
                    {
                        if (zones.FirstOrDefault(i => i.ZoneName.Equals(vaccineData.ZoneName)) == null)
                        {
                            zone = new Zone
                            {
                                ZoneName = vaccineData.ZoneName,
                                LocalCode = vaccineData.LocalCode,
                                LocalName = vaccineData.LocalName
                            };
                            _databaseContext.Zones.Add(zone);
                            zones.Add(zone);
                        }

                        if (ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(vaccineData.AgeGroup)) == null)
                        {
                            ageRange = new AgeRange { AgeRangeName = vaccineData.AgeGroup };
                            _databaseContext.AgeRanges.Add(ageRange);
                            ageRanges.Add(ageRange);
                        }
                    }

                    await _databaseContext.SaveChangesAsync();

                    zones = _databaseContext.Zones.ToList();
                    ageRanges = _databaseContext.AgeRanges.ToList();

                    foreach (AlbertaVaccineData vaccineData in albertaVaccinesData)
                    {
                        _databaseContext.PopulationVaccinations.Add(new PopulationVaccination()
                        {
                            ZoneId = zones.FirstOrDefault(i => i.ZoneName.Equals(vaccineData.ZoneName)).ZoneId,
                            AgeRangeId = ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(vaccineData.AgeGroup)).AgeRangeId,
                            Population = vaccineData.Population,
                            SingleDose = vaccineData.OneDose,
                            DoubleDose = vaccineData.TwoDose,
                            TripleDose = vaccineData.ThreeDose
                        });
                    }

                    await _databaseContext.SaveChangesAsync();

                    return JsonConvert.SerializeObject(
                        new JsonResult
                        {
                            IsSucceed = true
                        });
                }
                else
                {
                    return JsonConvert.SerializeObject(
                        new JsonResult
                        {
                            IsSucceed = false
                        });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }
        }

        public async Task<string> ConvertDataVaccines()
        {
            List<VaccineAgeRangeDimension> vaccineAgeRangeDimensions = _dataWarehouseContext.VaccineAgeRangeDimensions.ToList();
            List<VaccineDosageDimension> vaccineDosageDimensions = _dataWarehouseContext.VaccineDosageDimensions.ToList();
            List<VaccinePopulationDimension> vaccinePopulationDimensions = _dataWarehouseContext.VaccinePopulationDimensions.ToList();
            List<VaccineZoneDimension> vaccineZoneDimensions = _dataWarehouseContext.VaccineZoneDimensions.ToList();
            VaccineAgeRangeDimension vaccineAgeRangeDimension;
            VaccineDosageDimension vaccineDosageDimension;
            VaccinePopulationDimension vaccinePopulationDimension;
            VaccineZoneDimension vaccineZoneDimension;

            foreach (PopulationVaccination populationVaccination in _populationVaccinationDatabaseRepository.GetPopulationVaccinations())
            {
                vaccineAgeRangeDimension = vaccineAgeRangeDimensions.FirstOrDefault(i => i.AgeRangeName.Equals(populationVaccination.AgeRange.AgeRangeName));

                if (vaccineAgeRangeDimension == null)
                {
                    vaccineAgeRangeDimension = new VaccineAgeRangeDimension { AgeRangeName = populationVaccination.AgeRange.AgeRangeName };
                    vaccineAgeRangeDimensions.Add(vaccineAgeRangeDimension);
                    _dataWarehouseContext.Add(vaccineAgeRangeDimension);
                }

                vaccineDosageDimension = vaccineDosageDimensions.FirstOrDefault(i => i.OneDoseVaccinated == populationVaccination.SingleDose
                    && i.TwoDoseVaccinated == populationVaccination.DoubleDose && i.ThreeDoseVaccinated == populationVaccination.TripleDose);

                if (vaccineDosageDimension == null)
                {
                    vaccineDosageDimension = new VaccineDosageDimension
                    {
                        OneDoseVaccinated = populationVaccination.SingleDose,
                        TwoDoseVaccinated = populationVaccination.DoubleDose,
                        ThreeDoseVaccinated = populationVaccination.TripleDose
                    };
                    vaccineDosageDimensions.Add(vaccineDosageDimension);
                    _dataWarehouseContext.VaccineDosageDimensions.Add(vaccineDosageDimension);
                }

                vaccinePopulationDimension = vaccinePopulationDimensions.FirstOrDefault(i => i.Population == populationVaccination.Population);

                if (vaccinePopulationDimension == null)
                {
                    vaccinePopulationDimension = new VaccinePopulationDimension { Population = populationVaccination.Population };
                    vaccinePopulationDimensions.Add(vaccinePopulationDimension);
                    _dataWarehouseContext.VaccinePopulationDimensions.Add(vaccinePopulationDimension);
                }

                vaccineZoneDimension = vaccineZoneDimensions.FirstOrDefault(i => i.LocalCode.Equals(populationVaccination.Zone.LocalCode)
                    && i.LocalName.Equals(populationVaccination.Zone.LocalName) && i.ZoneName.Equals(populationVaccination.Zone.ZoneName));

                if (vaccineZoneDimension == null)
                {
                    vaccineZoneDimension = new VaccineZoneDimension
                    {
                        LocalCode = populationVaccination.Zone.LocalCode,
                        LocalName = populationVaccination.Zone.LocalName,
                        ZoneName = populationVaccination.Zone.ZoneName
                    };
                    vaccineZoneDimensions.Add(vaccineZoneDimension);
                    _dataWarehouseContext.Add(vaccineZoneDimension);
                }

                await _dataWarehouseContext.SaveChangesAsync();

                CovidVaccinationFact covidVaccinationFact = new CovidVaccinationFact
                {
                    VaccineZoneId = vaccineZoneDimension.VaccineZoneId,
                    VaccineAgeRangeId = vaccineAgeRangeDimension.VaccineAgeRangeId,
                    VaccineDosageId = vaccineDosageDimension.VaccineDosageId,
                    VaccinePopulationId = vaccinePopulationDimension.VaccinePopulationId
                };

                _dataWarehouseContext.CovidVaccinationFacts.Add(covidVaccinationFact);
            }

            try
            {
                await _dataWarehouseContext.SaveChangesAsync();

                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = true
                    });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }
        }

        public async Task<string> ConvertDataCases()
        {
            List<CaseAgeRangeDimension> caseAgeRangeDimensions = _dataWarehouseContext.CaseAgeRangeDimensions.ToList();
            List<CaseDateDimension> caseDateDimensions = _dataWarehouseContext.CaseDateDimensions.ToList();
            List<CaseStatusDimension> caseStatusDimensions = _dataWarehouseContext.CaseStatusDimensions.ToList();
            List<CaseZoneDimension> caseZoneDimensions = _dataWarehouseContext.CaseZoneDimensions.ToList();

            CaseAgeRangeDimension caseAgeRangeDimension;
            CaseDateDimension caseDateDimension;
            CaseStatusDimension caseStatusDimension;
            CaseZoneDimension caseZoneDimension;

            foreach (CovidCase covidCase in _covidCaseDatabaseRepository.GetCovidCases())
            {
                caseAgeRangeDimension = caseAgeRangeDimensions.FirstOrDefault(i => i.AgeRangeName.Equals(covidCase.AgeRange.AgeRangeName));

                if (caseAgeRangeDimension == null)
                {
                    caseAgeRangeDimension = new CaseAgeRangeDimension { AgeRangeName = covidCase.AgeRange.AgeRangeName };
                    caseAgeRangeDimensions.Add(caseAgeRangeDimension);
                    _dataWarehouseContext.CaseAgeRangeDimensions.Add(caseAgeRangeDimension);
                }

                caseDateDimension = caseDateDimensions.FirstOrDefault(i => i.Date.Equals(covidCase.Date));

                if (caseDateDimension == null)
                {
                    caseDateDimension = new CaseDateDimension(covidCase.Date);
                    caseDateDimensions.Add(caseDateDimension);
                    _dataWarehouseContext.CaseDateDimensions.Add(caseDateDimension);
                }

                caseStatusDimension = caseStatusDimensions.FirstOrDefault(i => i.StatusName.Equals(covidCase.CaseStatus.CaseStatusName));

                if (caseStatusDimension == null)
                {
                    caseStatusDimension = new CaseStatusDimension { StatusName = covidCase.CaseStatus.CaseStatusName };
                    caseStatusDimensions.Add(caseStatusDimension);
                    _dataWarehouseContext.CaseStatusDimensions.Add(caseStatusDimension);
                }

                caseZoneDimension = caseZoneDimensions.FirstOrDefault(i => i.AHSZoneName.Equals(covidCase.Zone.ZoneName));

                if (caseZoneDimension == null)
                {
                    caseZoneDimension = new CaseZoneDimension { AHSZoneName = covidCase.Zone.ZoneName };
                    caseZoneDimensions.Add(caseZoneDimension);
                    _dataWarehouseContext.CaseZoneDimensions.Add(caseZoneDimension);
                }

                await _dataWarehouseContext.SaveChangesAsync();

                CovidCaseFact covidCaseFact = new CovidCaseFact
                {
                    CaseZoneId = caseZoneDimension.CaseZoneId,
                    CaseAgeRangeId = caseAgeRangeDimension.CaseAgeRangeId,
                    CaseStatusId = caseStatusDimension.CaseStatusId,
                    CaseDateId = caseDateDimension.CaseDateId,
                    IsFemale = covidCase.IsFemale,
                    IsConfirmed = covidCase.IsConfirmed
                };

                _dataWarehouseContext.CovidCaseFacts.Add(covidCaseFact);
            }

            try
            {
                await _dataWarehouseContext.SaveChangesAsync();

                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = true
                    });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }
        }

        public async Task<string> DeleteDatabaseData()
        {
            try
            {
                _databaseContext.AgeRanges.RemoveRange(_databaseContext.AgeRanges.ToList());
                _databaseContext.CaseStatuses.RemoveRange(_databaseContext.CaseStatuses.ToList());
                _databaseContext.Zones.RemoveRange(_databaseContext.Zones.ToList());
                _databaseContext.CovidCases.RemoveRange(_databaseContext.CovidCases.ToList());
                _databaseContext.PopulationVaccinations.RemoveRange(_databaseContext.PopulationVaccinations.ToList());

                await _databaseContext.SaveChangesAsync();

                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = true
                    });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }
        }

        public async Task<string> DeleteDataWarehouseData()
        {
            _dataWarehouseContext.CovidCaseFacts.RemoveRange(_dataWarehouseContext.CovidCaseFacts.ToList());
            _dataWarehouseContext.CovidVaccinationFacts.RemoveRange(_dataWarehouseContext.CovidVaccinationFacts.ToList());

            try
            {
                await _dataWarehouseContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }

            _dataWarehouseContext.CaseAgeRangeDimensions.RemoveRange(_dataWarehouseContext.CaseAgeRangeDimensions.ToList());
            _dataWarehouseContext.CaseDateDimensions.RemoveRange(_dataWarehouseContext.CaseDateDimensions.ToList());
            _dataWarehouseContext.CaseStatusDimensions.RemoveRange(_dataWarehouseContext.CaseStatusDimensions.ToList());
            _dataWarehouseContext.CaseZoneDimensions.RemoveRange(_dataWarehouseContext.CaseZoneDimensions.ToList());

            _dataWarehouseContext.VaccineAgeRangeDimensions.RemoveRange(_dataWarehouseContext.VaccineAgeRangeDimensions.ToList());
            _dataWarehouseContext.VaccineDosageDimensions.RemoveRange(_dataWarehouseContext.VaccineDosageDimensions.ToList());
            _dataWarehouseContext.VaccinePopulationDimensions.RemoveRange(_dataWarehouseContext.VaccinePopulationDimensions.ToList());
            _dataWarehouseContext.VaccineZoneDimensions.RemoveRange(_dataWarehouseContext.VaccineZoneDimensions.ToList());

            try
            {
                await _dataWarehouseContext.SaveChangesAsync();

                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = true
                    });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(
                    new JsonResult
                    {
                        IsSucceed = false
                    });
            }
        }
    }

    public class AlbertaCaseData
    {
        public DateTime DateReported { get; set; }
        public string AlbertaHealthServicesZone { get; set; }
        public bool IsFemale { get; set; }
        public string AgeGroup { get; set; }
        public string CaseStatus { get; set; }
        public bool CaseConfirmed { get; set; }

        public AlbertaCaseData(string csvLine)
        {
            string[] values = csvLine.Split(',');

            this.DateReported = DateTime.Parse(values[1]);
            this.AlbertaHealthServicesZone = values[2];
            this.IsFemale = values[3].Equals("Female") ? true : false;
            this.AgeGroup = values[4];
            this.CaseStatus = values[5];
            this.CaseConfirmed = values[6].Equals("Confirmed") ? true : false;
        }
    }

    public class AlbertaVaccineData
    {
        public string ZoneName { get; set; }
        public string LocalName { get; set; }
        public string LocalCode { get; set; }
        public string AgeGroup { get; set; }
        public int Population { get; set; }
        public int OneDose { get; set; }
        public int TwoDose { get; set; }
        public int ThreeDose { get; set; }

        public AlbertaVaccineData(string csvLine)
        {
            string[] values = csvLine.Split(',');

            this.LocalCode = values[0];
            this.LocalName = values[1];
            this.ZoneName = values[2];
            this.AgeGroup = values[3];
            this.Population = Int32.Parse(values[4]);
            this.OneDose = Int32.Parse(values[5]);
            this.TwoDose = Int32.Parse(values[7]);
            this.ThreeDose = Int32.Parse(values[9]);
        }
    }

    public class JsonResult
    {
        public bool IsSucceed { get; set; }
    }
}
