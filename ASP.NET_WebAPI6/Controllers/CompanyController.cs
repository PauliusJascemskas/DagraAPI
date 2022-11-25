using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using AutoMapper;
using DagraAPI;
using DagraAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAuthJwtMySql.Models.Requests;
using System.Net;
using System.Security.Claims;

namespace ASP.NET_WebAPI6.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase  
    {
        private readonly DBContext DBContext;
        private readonly IMapper _mapper;

        public CompanyController(DBContext DBContext)
        {
            this.DBContext = DBContext;
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Company, CompanyDTO>()
                );
            _mapper = new Mapper(config);
        }

        [Authorize(Roles = "admin, worker, guest")]
        [HttpGet]
        public async Task<ActionResult<List<Company>>> Get()
        {
            User user = await DBContext.Users.Select(
                s => new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }).FirstOrDefaultAsync(s => s.email == User.Identity.Name);

            int company_id;
            company_id = user.fk_company;
            List<Company> list = new List<Company>();
            Company company = await DBContext.Companies.Select(
                s => new Company
                {
                    id = s.id,
                    name = s.name,
                    code = s.code,
                    fk_admin = s.fk_admin
                })
            .FirstOrDefaultAsync(s => s.id == company_id);

            if (company == null)
            {
                return NotFound();
            }
            else
            {
                list.Add(company);
                return list;
            }
        }

        [Authorize(Roles = "admin, worker, guest")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Company>>> GetCompanyById()
        {
            User user = await DBContext.Users.Select(
                s => new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }).FirstOrDefaultAsync(s => s.email == User.Identity.Name);

            int company_id;
            company_id = user.fk_company;
            List<Company> list = new List<Company>();
            Company company = await DBContext.Companies.Select(
                s => new Company
                {
                    id = s.id,
                    name = s.name,
                    code = s.code,
                    fk_admin = s.fk_admin
                })
            .FirstOrDefaultAsync(s => s.id == company_id);

            if (company == null)
            {
                return NotFound();
            }
            else
            {
                list.Add(company);
                return list;
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> InsertCompany(CreateCompanyDTO company)
        {
            User user = await DBContext.Users.Select(
            s => new User
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                email = s.email,
                role = s.role,
                password = s.password
            }).FirstOrDefaultAsync(s => s.email == User.Identity.Name);

            int company_id = user.fk_company;
            if(company_id != 0)
            {
                return BadRequest("Naudotojas jau priklausantis įmonei negali kurti naujos įmonės.");
            }

            Company comp = await DBContext.Companies.Select(
                s => new Company
                {
                    id = s.id,
                    name = s.name,
                    code = s.code,
                    fk_admin = s.fk_admin
                })
            .FirstOrDefaultAsync(s => s.code == company.code);

            if (comp != null)
            {
                return BadRequest("Įmonė su tokiu kodu jau egzistuoja sistemoje.");
            }
            var entity = new Company()
            {
                name = company.name,
                code = company.code,
                fk_admin = user.id
            };

            DBContext.Companies.Add(entity);
            await DBContext.SaveChangesAsync();
            
            Company comp2 = await DBContext.Companies.Select(
                s => new Company
                {
                    id = s.id,
                    name = s.name,
                    code = s.code,
                    fk_admin = s.fk_admin
                })
            .FirstOrDefaultAsync(s => s.code == company.code);
            int newCompanyId = comp2.id;

            var entity2 = await DBContext.Users.FirstOrDefaultAsync(s => s.email == user.email);
            entity2.fk_company = newCompanyId;

            await DBContext.SaveChangesAsync();

            return Created($"api/company/{newCompanyId}", comp2);

        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCompany(int id, CompanyDTO company)
        {
            User user = await DBContext.Users.Select(
            s => new User
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                email = s.email,
                role = s.role,
                password = s.password
            }).FirstOrDefaultAsync(s => s.email == User.Identity.Name);
            int company_id = user.fk_company;

            if(company_id == 0)
            {
                return NotFound();
            }
            if(company_id != id)
            {
                return NotFound();
            }
            var entity = await DBContext.Companies.FirstOrDefaultAsync(s => s.id == id);

            if (entity == null)
            {
                return NotFound();
            }
            entity.name = company.name;
            entity.code = company.code;
            entity.fk_admin = company.fk_admin;

            await DBContext.SaveChangesAsync();
            return Ok(entity);
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            User user = await DBContext.Users.Select(
            s => new User
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                email = s.email,
                role = s.role,
                password = s.password
            }).FirstOrDefaultAsync(s => s.email == User.Identity.Name);
            int company_id = user.fk_company;

            if(company_id != id)
            {
                return NotFound();
            }
            Company comp = await DBContext.Companies.Select(
                    s => new Company
                    {
                        id = s.id,
                        name = s.name,
                        code = s.code,
                        fk_admin = s.fk_admin,
                    })
                .FirstOrDefaultAsync(s => s.id == id);


            if (comp == null)
            {
                return NotFound();
            }
            var entity = new Company()
            {
                id = id
            };
            DBContext.Companies.Attach(entity);
            DBContext.Companies.Remove(entity);
            var entity2 = await DBContext.Users.FirstOrDefaultAsync(s => s.email == user.email);
            entity2.fk_company = 0;

            await DBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
