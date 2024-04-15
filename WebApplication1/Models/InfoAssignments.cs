using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace WebApplication1.Models
{
    public class InfoAssignments
    {
        public static void InformationAssignments()
        {
            using (var context = new CompanyContext())
            {
                Console.WriteLine("Clearing database...");
                context.Service.RemoveRange(context.Service);
                context.HourType.RemoveRange(context.HourType);
                context.SaveChanges();
                Console.WriteLine("Done!");
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://hr2022.simplicate.nl/api/v2/projects/service");
            httpClient.DefaultRequestHeaders.Add("Authentication-Key", "HInAJkEpNHKXNZfDFkRs96blsgCSYF4g");
            httpClient.DefaultRequestHeaders.Add("Authentication-Secret", "bvyi1UPanMisCNaeM4YtHFOpkk0UVd5C");
            var response = httpClient.Send(request);
            var reader = new StreamReader(response.Content.ReadAsStream());
            var responseBody = reader.ReadToEnd();
            File.WriteAllText("Assignments.json", responseBody);
            string json = File.ReadAllText("Assignments.json");
            RootS ob = JsonSerializer.Deserialize<RootS>(json)!;

            using (var context = new CompanyContext())
            {
                int count = 0;
                int counting = 0;
                foreach (var d in ob.data)
                {
                    var n = d.name;
                    count++;
                    foreach (var h in d.hour_types) { counting++; }
                }

                var services = new Service[count];
                var hourTypes = new HourType[counting];
                var counter = 0;
                var counter1 = 0;
                Console.WriteLine("Seeding Services...");
                foreach (var d in ob.data)
                {
                    if (d.name == null) { d.name = ""; }
                    services[counter] = new Service
                    {
                        ID = d.id,
                        Name = d.name,
                        ProjectID = d.project_id
                    };
                    foreach (var h in d.hour_types)
                    {
                        var type = h.hourstype;
                        hourTypes[counter1] = new HourType
                        {
                            ID = h.id,
                            TypeID = type.id,
                            Name = type.label,
                            ServiceID = d.id
                        };
                        counter1++;
                    }
                        counter++;
                }
                context.Service.AddRange(services);
                context.HourType.AddRange(hourTypes);
                Console.WriteLine("Done!");
                context.SaveChanges();
            }
        }
    }
    public class DatumS
    {
        public string name { get; set; }
        public string invoice_method { get; set; }
        public string amount { get; set; }
        public bool  track_hours { get; set; }
        public bool track_cost { get; set; }
        public string id { get; set; }
        public string start_date { get; set; }
        public string project_id { get; set; }
        public string status { get; set; }
        public List<HourTypes> hour_types { get; set; }
        public VatClass vat_class { get; set; }
        public bool invoice_in_installments { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string write_hours_start_date { get; set; }
}
public class HourTypes
    {
        public int budgeted_amount { get; set; }
        public int tariff { get; set; }
        public bool billable { get; set; }
        public string id { get; set; }
        public HoursType hourstype { get; set; }


    }
    public class HoursType
    {
        public string label { get; set; }
        public bool blocked { get; set; }
        public string color { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }
    public class VatClass
    {
        public string id { get; set; }
        public int percentage { get; set; }
        public string name { get; set; }
    }
public class RootS
    {
        public List<DatumS> data { get; set; }
        public object errors { get; set; }
        public object debug { get; set; }
    }
}
