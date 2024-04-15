namespace WebApplication1.Models
{
    public class ProjectItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ServiceItem> ServiceItems { get; set; }

    }
    public class ServiceItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<HourTypeItem> HourTypeItems { get; set; }
    }
    public class HourTypeItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
