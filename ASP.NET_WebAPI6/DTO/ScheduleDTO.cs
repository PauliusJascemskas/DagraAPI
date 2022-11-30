namespace ASP.NET_WebAPI6.DTO
{
    public class ScheduleDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        //public int fk_company { get; set; }
        public int admin { get; set; }
    }
    public class UpdateScheduleDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        //public int fk_company { get; set; }
        public string admin_email { get; set; }
    }

    public class CreateScheduleDTO
    {
        public string name { get; set; }
        public string admin_email { get; set; }
    }
}
