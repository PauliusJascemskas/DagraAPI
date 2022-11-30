using System.ComponentModel.DataAnnotations;

namespace ASP.NET_WebAPI6.Entities
{
    public partial class AssignmentDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        //public int fk_job { get; set; }
        public string assignee_email { get; set; }
        //public int fk_schedule { get; set; }

    }

    public partial class CreateAssignmentDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        //public int fk_job { get; set; }
        public string assignee_email { get; set; }
        //public int fk_schedule { get; set; }
    }

}