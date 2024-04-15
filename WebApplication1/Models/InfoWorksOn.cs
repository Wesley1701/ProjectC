using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class InfoWorksOn
    {
        public static void InformationWorksOn()
        {
            using (var contextt = new CompanyContext())
            {
                Console.WriteLine("Clearing database...");;
                contextt.WorksOn.RemoveRange(contextt.WorksOn);
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
                int worksonCount = 0;
                foreach(DatumPro datal in ob.data)
                {
                    string fa = datal.name;
                    worksonCount++;
                }
                
                string [] arrayTemp = new string [worksonCount];
                string [] arrayName = new string [worksonCount];
                int counter = 0;
                foreach(DatumPro data in ob.data)
                {
                    string temp = data.id;
                    string name = data.name;
                    
                    arrayTemp[counter] = temp;
                    arrayName[counter] = name;
                    counter++;
                }
                
                Console.WriteLine("Seeding WorksOn...");
                var workson = new WorksOn[worksonCount];
                for(int i = 0; i<worksonCount; i++)
                {
                    workson[i] = new WorksOn
                        {
                            EmployeeSSN = arrayTemp[i],
                            ProjectName = arrayName[i]
                        };
                }
                contextt.WorksOn.AddRange(workson);
                Console.WriteLine("Done!");
                contextt.SaveChanges();
            }
        }
    }
}