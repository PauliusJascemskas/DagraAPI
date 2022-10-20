
namespace ASP.NET_WebAPI6.Entities
{
    public partial class Job
    {
        public int id { get; set; }
        public string name { get; set; }
        public int fk_schedule { get; set; }
    }
}
