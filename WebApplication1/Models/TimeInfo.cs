using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class TimeInfo
    {
        [JsonProperty("startTime")]
        public DateTime startTime { get; set; }
        public string startString { get; set; }
        [JsonProperty("endTime")]
        public DateTime endTime { get; set; }
        public bool running { get; set; }
        public void Start()
        {
            this.startTime = DateTime.Now;
            this.startString = this.startTime.ToString("h:mm tt");
            this.running = true;
        }
        public void Stop()
        {
            this.endTime = DateTime.Now;
            this.running = false;
        }
        public double GetElapsedTime()
        {
            TimeSpan interval;
            if (running)
                interval = DateTime.Now - startTime;
            else if (this.startTime > this.endTime)
                interval = endTime - startTime;
            else
                return 00;
            return interval.TotalSeconds;
        }
    }
}
