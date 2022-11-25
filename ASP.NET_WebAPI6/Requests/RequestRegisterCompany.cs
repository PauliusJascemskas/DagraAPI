namespace NetCoreAuthJwtMySql.Models.Requests
{
    public class RequestRegisterCompany : BaseRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int fk_company { get; set; }
        public string company_name { get; set; }
        public int company_code { get; set; }
    }
}
