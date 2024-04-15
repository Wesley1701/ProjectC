using System;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using RestSharp;

namespace WebApplication1.Models
{
    public class SendHours
    {
        public static void SendingHours(string input)
        {

            var client = new RestClient("https://hr2022.simplicate.nl/api/v2/hours/hours");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authentication-Key", "HInAJkEpNHKXNZfDFkRs96blsgCSYF4g");
            request.AddHeader("Authentication-Secret", "bvyi1UPanMisCNaeM4YtHFOpkk0UVd5C");
            request.AddHeader("Content-Type", "application/json");
            var body = input;
    //        var body = @"{
    //" + "\n" +
    //            @"    ""employee_id"": ""employee:be93045f0f01e63a"",
    //" + "\n" +
    //            @"    ""project_id"": ""project:6cc70702c221b840"",
    //" + "\n" +
    //            @"    ""projectservice_id"": ""service:8c29d20520ed8ffe"",
    //" + "\n" +
    //            @"    ""type_id"": ""hourstype:f902fc5514b044a2"",
    //" + "\n" +
    //            @"    ""hours"": 0.5,
    //" + "\n" +
    //            @"    ""start_date"": ""2022-12-28 10:00:00"",
    //" + "\n" +
    //            @"    ""end_date"": ""2022-12-28 10:30:00"",
    //" + "\n" +
    //            @"    ""is_time_defined"": true
    //" + "\n" +
    //            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public static void SendDataPost() 
        {
            using (var context = new CompanyContext())
            {
                var hours = from Hours in context.Hours
                            select Hours;
                foreach (var hour in hours)
                {
                    if (DateTime.Parse(hour.EndTime) < DateTime.Now)
                    {
                        var hourstype = "";
                        using (var contextt = new CompanyContext())
                        {
                            var ht = contextt.HourType.Where(s => s.ID.Equals(hour.HoursTypeID)).ToArray();
                            hourstype = ht[0].TypeID;
                        }
                        var start = DateTime.Parse(hour.StartTime);
                        var end = DateTime.Parse(hour.EndTime);
                        var difference = (end - start).TotalHours;
                        var body = @"{
                        " + "\n" +
                                    $@"    ""employee_id"": ""{hour.EmployeeID.Trim()}"",
                        " + "\n" +
                                    $@"    ""project_id"": ""{hour.ProjectID.Trim()}"",
                        " + "\n" +
                                    $@"    ""projectservice_id"": ""{hour.ServiceID.Trim()}"",
                        " + "\n" +
                                    $@"    ""type_id"": ""{hourstype.Trim()}"",
                        " + "\n" +
                                    $@"    ""hours"": {difference},
                        " + "\n" +
                                    $@"    ""start_date"": ""{start.ToString("yyyy-MM-dd HH:mm:ss")}"",
                        " + "\n" +
                                    $@"    ""end_date"": ""{end.ToString("yyyy-MM-dd HH:mm:ss")}"",
                        " + "\n" +
                                    @"    ""is_time_defined"": true
                        " + "\n" +
                                    @"}";
                        SendingHours(body);
                        Console.WriteLine(body);
                    }
                }
                context.SaveChanges();
            }
        }
    }
}