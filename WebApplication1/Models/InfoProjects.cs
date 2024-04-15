using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class InfoProjects
    {
        public static void InformationProjects()
        {
            using (var contextt = new CompanyContext())
            {
                Console.WriteLine("Clearing database...");;
                contextt.Project.RemoveRange(contextt.Project);
                contextt.SaveChanges();
                Console.WriteLine("Done!");
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://hr2022.simplicate.nl/api/v2/projects/project");
            httpClient.DefaultRequestHeaders.Add("Authentication-Key", "HInAJkEpNHKXNZfDFkRs96blsgCSYF4g");
            httpClient.DefaultRequestHeaders.Add("Authentication-Secret", "bvyi1UPanMisCNaeM4YtHFOpkk0UVd5C");
            var response = httpClient.Send(request);
            var reader = new StreamReader(response.Content.ReadAsStream());
            var responseBody = reader.ReadToEnd();
            File.WriteAllText("Projects.json", responseBody);
            string json = File.ReadAllText("Projects.json");
            RootPro ob = JsonSerializer.Deserialize<RootPro>(json)!;
            using (var contextt = new CompanyContext())
            {   
                int projectsCount = 0;
                foreach(DatumPro datal in ob.data)
                {
                    string fa = datal.name;
                    projectsCount++;
                }

                string [] arrayID = new string [projectsCount];
                string [] arrayName = new string [projectsCount];
                int [] arraymany = new int [projectsCount];
                int counter = 0;
                int count = 0;
                int counterr = 0;
                try{
                foreach (DatumPro data in ob.data)
                {
                    foreach(EmployeePro emp in data.employees){
                        count++;
                    }
                    arraymany[counterr] = count;
                    count = 0;
                    counterr++;
                }
                }
                catch{
                    arraymany[counterr] = 0;
                }

                foreach(DatumPro data in ob.data)
                {
                    string ID = data.id;
                    string name = data.name;
                    
                    arrayID[counter] = ID;
                    arrayName[counter] = name;
                    counter++;
                }
                
                Console.WriteLine("Seeding Projects...");
                var projects = new Project[projectsCount];
                for(int i = 0; i<projectsCount; i++)
                {
                    projects[i] = new Project
                        {
                            NumberID = arrayID[i],
                            Name = arrayName[i],
                            WorkingOn = arraymany[i]
                        };
                }
                contextt.Project.AddRange(projects);
                Console.WriteLine("Done!");
                contextt.SaveChanges();
            }
        }
    }
       public class Budget
    {
        public Hours hours { get; set; }
        public Costs costs { get; set; }
        public Total total { get; set; }
    }

    public class Costs
    {
        public int value_budget { get; set; }
        public int value_spent { get; set; }
    }

    public class DatumPro
    {
        public string id { get; set; }
        public ProjectStatus project_status { get; set; }
        public string hours_rate_type { get; set; }
        public Organization organization { get; set; }
        public SeparateInvoiceRecipient separate_invoice_recipient { get; set; }
        public MyOrganizationProfile my_organization_profile { get; set; }
        public List<EmployeePro> employees { get; set; }
        public Budget budget { get; set; }
        public string timeline_email_address { get; set; }
        public string created { get; set; }
        public string modified { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string simplicate_url { get; set; }
        public bool is_reverse_billing { get; set; }
        public bool is_invoice_approval { get; set; }
        public string my_organization_profile_id { get; set; }
        public string organization_id { get; set; }
        public string name { get; set; }
        public bool billable { get; set; }
        public bool can_register_mileage { get; set; }
        public string project_number { get; set; }
        public string start_date { get; set; }
    }

    public class EmployeePro
    {
        public string id { get; set; }
        public string employee_id { get; set; }
        public string name { get; set; }
        public int tariff { get; set; }
        public bool declarable { get; set; }
        public int amount { get; set; }
    }

    public class HoursPro
    {
        public int amount_budget { get; set; }
        public double amount_spent { get; set; }
        public int value_budget { get; set; }
        public double value_spent { get; set; }
    }

    public class MyOrganizationProfile
    {
        public string id { get; set; }
        public Organization organization { get; set; }
    }

    public class Organization
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ProjectStatus
    {
        public string id { get; set; }
        public string label { get; set; }
    }

    public class RootPro
    {
        public List<DatumPro> data { get; set; }
        public object errors { get; set; }
        public object debug { get; set; }
    }

    public class SeparateInvoiceRecipient
    {
        public bool is_separate_invoice_recipient { get; set; }
    }

    public class Total
    {
        public int value_budget { get; set; }
        public double value_spent { get; set; }
        public int value_invoiced { get; set; }
    }
}