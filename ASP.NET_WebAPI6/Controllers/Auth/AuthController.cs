using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using AutoMapper;
using DagraAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCoreAuthJwtMySql.Models.Requests;
using NetCoreAuthJwtMySql.Models.Responses;
using NetCoreAuthJwtMySql.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DagraAPI.Controllers.Auth
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DBContext DBContext;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration configuration, DBContext dBContext)
        {
            _configuration = configuration;
            this.DBContext = dBContext;
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Schedule, ScheduleDTO>()
                );
            _mapper = new Mapper(config);
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult<ResponseLogin> Login(RequestLogin requestLogin)
        {
            var responseLogin = new ResponseLogin();
            using (var db = new DBContext())
            {
                var existingUser = db.Users.SingleOrDefault(x => x.email == requestLogin.email);
                if (existingUser != null)
                {
                    var isPasswordVerified = CryptoUtil.VerifyPassword(requestLogin.password, existingUser.password);
                    if (isPasswordVerified)
                    {
                        var claimList = new List<Claim>();
                        claimList.Add(new Claim(ClaimTypes.Name, existingUser.email));
                        claimList.Add(new Claim(ClaimTypes.Role, existingUser.role));
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expireDate = DateTime.UtcNow.AddDays(1);
                        var timeStamp = DateUtil.ConvertToTimeStamp(expireDate);
                        var token = new JwtSecurityToken(
                            claims: claimList,
                            notBefore: DateTime.UtcNow,
                            expires: expireDate,
                            signingCredentials: creds);
                        responseLogin.Success = true;
                        responseLogin.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        responseLogin.ExpireDate = timeStamp;
                    }
                    else
                    {
                        responseLogin.MessageList.Add("Neteisingas slaptažodis.");
                    }
                }
                else
                {
                    responseLogin.MessageList.Add("Tokia elektroninio pašto sistemoje nėra.");
                }
            }
            return responseLogin;
        }





        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RequestRegister requestRegister)
        {

            User User = await DBContext.Users.Select(
                    s => new User
                    {
                        id = s.id,
                        name = s.name,
                        fk_company = s.fk_company,
                        email = s.email,
                        role = s.role,
                        password = s.password
                    })
                .FirstOrDefaultAsync(s => s.email == requestRegister.email);

            if (User != null)
            {
                return BadRequest("Toks vartotojas jau yra.");
            }
            var password = requestRegister.password;
            var hashedPassword = CryptoUtil.Hash(password);
            var user = new User()
            {
                name = requestRegister.name,
                email = requestRegister.email,
                password = hashedPassword,
                role = "Worker",
                fk_company = requestRegister.fk_company,
            };

            DBContext.Users.Add(user);
            await DBContext.SaveChangesAsync();

            return Created($"api/register/", user);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdmin(RequestRegister requestRegister)
        {

            User User = await DBContext.Users.Select(
                    s => new User
                    {
                        id = s.id,
                        name = s.name,
                        fk_company = s.fk_company,
                        email = s.email,
                        role = s.role,
                        password = s.password
                    })
                .FirstOrDefaultAsync(s => s.email == requestRegister.email);

            if (User != null)
            {
                return BadRequest("Toks vartotojas jau yra.");
            }
            var password = requestRegister.password;
            var hashedPassword = CryptoUtil.Hash(password);
            var user = new User()
            {
                name = requestRegister.name,
                email = requestRegister.email,
                password = hashedPassword,
                role = "admin",
                fk_company = requestRegister.fk_company,
            };

            DBContext.Users.Add(user);
            await DBContext.SaveChangesAsync();

            return Created($"api/register/", user);

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("gets")]
        public async Task<ActionResult<List<User>>> Get()
        {
            var List = await DBContext.Users.Select(
                s => new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }
    }
}