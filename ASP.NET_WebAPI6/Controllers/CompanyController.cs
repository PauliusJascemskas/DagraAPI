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
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.Sig;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public async Task<ActionResult<Company>> Get()
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

            if (user.fk_company != 0)
            {
                int company_id;
                company_id = user.fk_company;
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
                    return company;
                }
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "admin, worker, guest")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
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
            if (id != user.fk_company)
                return NotFound();

            int company_id = user.fk_company;
            Company company = await DBContext.Companies.Select(
                s => new Company
                {
                    id = s.id,
                    name = s.name,
                    code = s.code,
                    fk_admin = s.fk_admin
                }).FirstOrDefaultAsync(s => s.id == id);

            if (company == null)
                return NotFound();
            return company;
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
            if (user.fk_company != 0)
                return BadRequest("Naudotojas, priklausantis įmonei, negali kurti naujos įmonės.");

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
                return BadRequest("Įmonė su tokiu kodu sistemoje jau egzistuoja.");

            var entity = new Company()
            {
                name = company.name,
                code = company.code,
                fk_admin = user.id
            };
            DBContext.Companies.Add(entity);
            await DBContext.SaveChangesAsync();
            
            Company comp_new = await DBContext.Companies.Select(
                s => new Company
                {
                    id = s.id,
                    name = s.name,
                    code = s.code,
                    fk_admin = s.fk_admin
                })
            .FirstOrDefaultAsync(s => s.code == company.code);

            int newCompanyId = comp_new.id;
            var entity2 = await DBContext.Users.FirstOrDefaultAsync(s => s.email == user.email);
            entity2.fk_company = newCompanyId;

            await DBContext.SaveChangesAsync();

            return Created($"api/companies/{newCompanyId}", comp_new);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCompany(int id, UpdateCompanyDTO company)
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
            if (id != user.fk_company)
                return NotFound();

            var entity = await DBContext.Companies.FirstOrDefaultAsync(s => s.id == id);
            if (entity == null)
                return NotFound();

            entity.name = company.name;
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
            if (id != user.fk_company)
                return NoContent();

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
                return NotFound();

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
