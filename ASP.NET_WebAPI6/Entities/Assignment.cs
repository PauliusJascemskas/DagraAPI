namespace ASP.NET_WebAPI6.Entities
{
    public partial class Assignment
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int creator { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public int fk_job { get; set; }
        public int fk_assignee { get; set; }
    }
}