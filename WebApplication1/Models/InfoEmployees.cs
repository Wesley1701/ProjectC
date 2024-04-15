using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class InfoEmployees
    {
        public static void InformationEmployees()
        {
            using (var context = new CompanyContext())
            {
                Console.WriteLine("Clearing database...");
                context.Employee.RemoveRange(context.Employee);
                context.SaveChanges();
                Console.WriteLine("Done!");
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://hr2022.simplicate.nl/api/v2/hrm/employee");
            httpClient.DefaultRequestHeaders.Add("Authentication-Key", "HInAJkEpNHKXNZfDFkRs96blsgCSYF4g");
            httpClient.DefaultRequestHeaders.Add("Authentication-Secret", "bvyi1UPanMisCNaeM4YtHFOpkk0UVd5C");
            var response = httpClient.Send(request);
            var reader = new StreamReader(response.Content.ReadAsStream());
            var responseBody = reader.ReadToEnd();
            File.WriteAllText("Employee.json", responseBody);
            string json = File.ReadAllText("Employee.json");
            Root ob = JsonSerializer.Deserialize<Root>(json)!;
            using (var context = new CompanyContext())
            {   
                int employeeCount = 0;
                foreach(Datum data in ob.data)
                {
                    string fa = data.name;
                    employeeCount++;
                }

                string[] arrayMI = new string[employeeCount];
                string [] arraySSN = new string [employeeCount];
                string [] arrayName = new string [employeeCount];
                Gender [] arrayGender = new Gender [employeeCount];
                int counter = 0;
                foreach(Datum data in ob.data)
                {
                    string initials = data.avatar.initials;
                    string SSN = data.id;
                    string name = data.name;
                    try{
                        string gender = data.person.gender;
                        if(gender == "Gender_Male"){
                            arrayGender[counter] = Gender.Male;
                        }
                        else if(gender == "Gender_Female"){
                            arrayGender[counter] = Gender.Female;
                        }
                        else{
                            arrayGender[counter] = Gender.Other;
                        }
                    }
                    catch{
                        arrayGender[counter] = Gender.Unknown;
                    }
                    arrayMI[counter] = initials;
                    arraySSN[counter] = SSN;
                    arrayName[counter] = name;
                    counter++;
                }
                
                Console.WriteLine("Seeding Employees...");
                var employees = new Employee[employeeCount];
                for(int i = 0; i<employeeCount; i++)
                {
                    employees[i] = new Employee
                        {
                            SSN = arraySSN[i],
                            Initials = arrayMI[i],
                            Name = arrayName[i],
                            Gender = arrayGender[i]
                        };
                }
                context.Employee.AddRange(employees);
                Console.WriteLine("Done!");
                context.SaveChanges();
            }
        }
    }

    public class Address
    {
        public string id { get; set; }
        public string country { get; set; }
        public string type { get; set; }
        public string country_code { get; set; }
        public string country_id { get; set; }
    }

    public class Avatar
    {
        public string initials { get; set; }
        public string color { get; set; }
    }
    
     public class Person
    {
        public string date_of_birth { get; set; }
        public string gender_id { get; set; }
        public string gender { get; set; }
        public Address address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string twitter_url { get; set; }
        public string linkedin_url { get; set; }
        public string facebook_url { get; set; }
        public string id { get; set; }
        public string full_name { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public bool is_user { get; set; }
        public Supervisor supervisor { get; set; }
        public Person person { get; set; }
        public Status status { get; set; }
        public string person_id { get; set; }
        public string name { get; set; }
        public string bank_account { get; set; }
        public string function { get; set; }
        public Type type { get; set; }
        public string employment_status { get; set; }
        public string work_email { get; set; }
        public string hourly_sales_tariff { get; set; }
        public string hourly_cost_tariff { get; set; }
        public Avatar avatar { get; set; }
        public string created { get; set; }
        public string modified { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string simplicate_url { get; set; }
        public string timeline_email_address { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
        public object errors { get; set; }
        public object debug { get; set; }
    }

    public class Status
    {
        public string id { get; set; }
        public string label { get; set; }
    }

    public class Supervisor
    {
        public string id { get; set; }
        public string person_id { get; set; }
        public string name { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string label { get; set; }
    }
}