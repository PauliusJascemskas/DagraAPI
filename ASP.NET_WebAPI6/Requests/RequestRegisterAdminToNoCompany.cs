namespace NetCoreAuthJwtMySql.Models.Requests
{
    public class RequestRegisterAdminToNoCompany : BaseRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        //public int fk_company { get; set; }
    }
}
