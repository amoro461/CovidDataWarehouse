﻿using CovidDataWarehouse.Data;
using CovidDataWarehouse.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidDataWarehouse.App.Controllers
{
    public class DataScraper : Controller
    {
        private static CovidDataWarehouseContext _context;

        public DataScraper(CovidDataWarehouseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

                    List<CaseStatus> caseStatuses = _context.CaseStatuses.ToList();
                    List<Zone> zones = _context.Zones.ToList();
                    List<AgeRange> ageRanges = _context.AgeRanges.ToList();
                    CaseStatus caseStatus;
                    Zone zone;
                    AgeRange ageRange;

                    foreach (AlbertaCaseData caseData in albertaCasesData)
                    {
                        if (zones.FirstOrDefault(i => i.ZoneName.Equals(caseData.AlbertaHealthServicesZone)) == null)
                        {
                            zone = new Zone { ZoneName = caseData.AlbertaHealthServicesZone };
                            _context.Zones.Add(zone);
                            zones.Add(zone);
                        }

                        if (caseStatuses.FirstOrDefault(i => i.CaseStatusName.Equals(caseData.CaseStatus)) == null)
                        {
                            caseStatus = new CaseStatus { CaseStatusName = caseData.CaseStatus };
                            _context.CaseStatuses.Add(caseStatus);
                            caseStatuses.Add(caseStatus);
                        }

                        if (ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(caseData.AgeGroup)) == null)
                        {
                            ageRange = new AgeRange { AgeRangeName = caseData.AgeGroup };
                            _context.AgeRanges.Add(ageRange);
                            ageRanges.Add(ageRange);
                        }
                    }

                    await _context.SaveChangesAsync();

                    caseStatuses = _context.CaseStatuses.ToList();
                    zones = _context.Zones.ToList();
                    ageRanges = _context.AgeRanges.ToList();

                    foreach (AlbertaCaseData caseData in albertaCasesData.Take(1000))
                    {
                        _context.CovidCases.Add(new CovidCase()
                        {
                            ZoneId = zones.FirstOrDefault(i => i.ZoneName.Equals(caseData.AlbertaHealthServicesZone)).ZoneId,
                            AgeRangeId = ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(caseData.AgeGroup)).AgeRangeId,
                            CaseStatusId = caseStatuses.FirstOrDefault(i => i.CaseStatusName.Equals(caseData.CaseStatus)).CaseStatusId,
                            IsFemale = caseData.IsFemale,
                            IsConfirmed = caseData.CaseConfirmed,
                            Date = caseData.DateReported
                        });
                    }

                    await _context.SaveChangesAsync();

                    return document;
                }
            }
            catch (Exception e)
            {  }

            return "Failed";
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

                    List<Zone> zones = _context.Zones.ToList();
                    List<AgeRange> ageRanges = _context.AgeRanges.ToList();
                    Zone zone;
                    AgeRange ageRange;

                    foreach (AlbertaVaccineData vaccineData in albertaVaccinesData)
                    {
                        if (zones.FirstOrDefault(i => i.ZoneName.Equals(vaccineData.ZoneName)) == null)
                        {
                            zone = new Zone { ZoneName = vaccineData.ZoneName };
                            _context.Zones.Add(zone);
                            zones.Add(zone);
                        }

                        if (ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(vaccineData.AgeGroup)) == null)
                        {
                            ageRange = new AgeRange { AgeRangeName = vaccineData.AgeGroup };
                            _context.AgeRanges.Add(ageRange);
                            ageRanges.Add(ageRange);
                        }
                    }

                    await _context.SaveChangesAsync();

                    zones = _context.Zones.ToList();
                    ageRanges = _context.AgeRanges.ToList();

                    foreach (AlbertaVaccineData vaccineData in albertaVaccinesData)
                    {
                        _context.PopulationVaccinations.Add(new PopulationVaccination()
                        {
                            ZoneId = zones.FirstOrDefault(i => i.ZoneName.Equals(vaccineData.ZoneName)).ZoneId,
                            AgeRangeId = ageRanges.FirstOrDefault(i => i.AgeRangeName.Equals(vaccineData.AgeGroup)).AgeRangeId,
                            Population = vaccineData.Population,
                            SingleDose = vaccineData.OneDose,
                            DoubleDose = vaccineData.TwoDose,
                            TripleDose = vaccineData.ThreeDose
                        });
                    }

                    await _context.SaveChangesAsync();

                    return document;
                }
            }
            catch (Exception e)
            { }

            return "Failed";
        }

        public async Task<IActionResult> DeleteDatabaseData()
        {
            _context.AgeRanges.RemoveRange(_context.AgeRanges.ToList());
            _context.CaseStatuses.RemoveRange(_context.CaseStatuses.ToList());
            _context.Zones.RemoveRange(_context.Zones.ToList());
            _context.CovidCases.RemoveRange(_context.CovidCases.ToList());
            _context.PopulationVaccinations.RemoveRange(_context.PopulationVaccinations.ToList());

            await _context.SaveChangesAsync();

            return Ok();
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
        public string AgeGroup { get; set; }
        public int Population { get; set; }
        public int OneDose { get; set; }
        public int TwoDose { get; set; }
        public int ThreeDose { get; set; }

        public AlbertaVaccineData(string csvLine)
        {
            string[] values = csvLine.Split(',');

            this.ZoneName = values[2];
            this.AgeGroup = values[3];
            this.Population = Int32.Parse(values[4]);
            this.OneDose = Int32.Parse(values[5]);
            this.TwoDose = Int32.Parse(values[7]);
            this.ThreeDose = Int32.Parse(values[9]);
        }
    }
}
