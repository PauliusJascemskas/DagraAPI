namespace ASP.NET_WebAPI6.DTO
{
    public class CompanyDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public int fk_admin { get; set; }
    }
    public class CreateCompanyDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        public int code { get; set; }
       public int fk_admin { get; set; }
    }

    public class UpdateCompanyDTO
    {
        //public int id { get; set; }
        public string name { get; set; }
        //public int code { get; set; }
        //public int fk_admin { get; set; }
    }
}
