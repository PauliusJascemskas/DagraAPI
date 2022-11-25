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
    [Route("api/companies/{companyId}/schedules")]
    public class ScheduleController : ControllerBase 
    {
        private readonly DBContext DBContext;
        private readonly IMapper _mapper;

        public ScheduleController(DBContext DBContext)
        {
            this.DBContext = DBContext;
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Schedule, ScheduleDTO>()
                ) ;
            _mapper = new Mapper(config);
        }

        [Authorize(Roles ="admin, worker, guest")]
        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> Get()
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
            int company;
            List<Schedule> List;
            company = user.fk_company;
            List = await DBContext.Schedules.Where(s => s.fk_company == company).Select(
            s => new Schedule
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                admin = s.admin,
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


        [Authorize(Roles = "admin, worker")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetScheduleById(int id)
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
            int company;
            Schedule schedule;
            if (user != null)
            {
                company = user.fk_company;
                schedule = await DBContext.Schedules.Select(
                s => new Schedule
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    admin = s.admin,
                }
                ).FirstOrDefaultAsync(s => s.id == id && s.fk_company == company);
                if (schedule == null)
                {
                    return NotFound();
                }
                else
                {
                    return schedule;
                }
            }
            else return NotFound();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> InsertSchedule(int companyId, ScheduleDTO schedule)
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

            int company = user.fk_company;
            

            var entity = new Schedule()
            {
                name = schedule.name,
                fk_company = companyId,
                admin = user.id
            };

            DBContext.Schedules.Add(entity);
            await DBContext.SaveChangesAsync();

            return Created($"api/schedules/{entity.id}", entity);

            //var entity = new Schedule()
            //{
            //    name = schedule.name,
            //    fk_company = schedule.fk_company,
            //    admin = schedule.admin,
            //};

            //DBContext.Schedules.Add(entity);
            //await DBContext.SaveChangesAsync();

            //return Created($"api/schedules/{schedule.id}", entity);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchedule(int companyid, int id, ScheduleDTO schedule)
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
            int company = user.fk_company;

            var entity = await DBContext.Schedules.FirstOrDefaultAsync(s => s.id == id && s.fk_company == company);

            if(entity == null)
            {
                return NotFound();
            }
            entity.name = schedule.name;
            entity.fk_company = user.fk_company;          
            entity.admin = user.id; 

            await DBContext.SaveChangesAsync();
            return Ok(entity);
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchedule(int companyId, int id)
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
            int company = user.fk_company;

            Schedule Schedule = await DBContext.Schedules.Select(
                    s => new Schedule
                    {
                        id = s.id,
                        name = s.name,
                        fk_company = s.fk_company,
                        admin = s.admin,
                    })
                .FirstOrDefaultAsync(s => s.id == id && s.fk_company == company);


            if (Schedule == null)
            {
                return NotFound();
            }
            var entity = new Schedule()
            {
                id = id
            };
            DBContext.Schedules.Attach(entity);
            DBContext.Schedules.Remove(entity);
            await DBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
