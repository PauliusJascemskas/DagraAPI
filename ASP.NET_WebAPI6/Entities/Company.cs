
namespace ASP.NET_WebAPI6.Entities
{
    public partial class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public int fk_admin { get; set; }
    }
}
