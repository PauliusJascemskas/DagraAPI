
namespace ASP.NET_WebAPI6.Entities
{
    public partial class JobDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        //public int fk_schedule { get; set; }
    }
    public partial class CreateJobDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        //public int fk_schedule { get; set; }
    }
}
