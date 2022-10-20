namespace ASP.NET_WebAPI6.Entities
{
    public partial class Assignment
    {
        public int id { get; set; }
        public int fk_job { get; set; }
        public int fk_worker { get; set; }
        public int fk_schedule { get; set; }
    }
}