
namespace ASP.NET_WebAPI6.Entities
{
    public partial class Job
    {
        public int id { get; set; }
        public string name { get; set; }
        public int creator { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int fk_schedule { get; set; }
    }
}
