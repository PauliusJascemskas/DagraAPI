namespace NetCoreAuthJwtMySql.Models.Requests
{
    public class RequestLogin : BaseRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
