using System.Text.Json;

namespace WebApplication1.Models
{
    public class Login
    {
        public string LoginFuncc(string Email, string psw)
        {
            try
            {
                string endPoint = "https://hr2022.simplicate.nl/site/login";
                var client = new HttpClient();
                var data = new[]
                {
                new KeyValuePair<string, string>("LoginForm[username]", Email),
                new KeyValuePair<string, string>("LoginForm[password]", psw),
            };
                client.PostAsync(endPoint, new FormUrlEncodedContent(data)).GetAwaiter().GetResult();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://hr2022.simplicate.nl/api/v2/users/user");
                var response = client.Send(request);
                var reader = new StreamReader(response.Content.ReadAsStream());
                var responseBody = reader.ReadToEnd();
                File.WriteAllText("User.json", responseBody);
                string json = File.ReadAllText("User.json");
                Roott ob = JsonSerializer.Deserialize<Roott>(json)!;
                var id = ob.data.employee_id;
                return id;
            }
            catch
            {
                return "0";
            }
        }
    }
    public class Data
    {
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string family_name { get; set; }
        public bool is_authy_enabled { get; set; }
        public bool is_employee { get; set; }
        public string employee_id { get; set; }
        public string person_id { get; set; }
    }

    public class Roott
    {
        public Data data { get; set; }
        public object errors { get; set; }
        public object debug { get; set; }
    }
}