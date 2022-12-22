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
using System.ComponentModel.Design;
using System.Net;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<ActionResult<List<OutputScheduleDTO>>> Get(int companyId)
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
            if (user.fk_company != companyId)
                return NotFound();

            List<OutputScheduleDTO> List;
            List = await DBContext.Schedules.Where(s => s.fk_company == user.fk_company).Select(
            s => new OutputScheduleDTO
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                admin = s.admin,
                admin_email = "",
            }
            ).ToListAsync();

            foreach (var schedule in List)
            {
                User temp = await DBContext.Users.Select(
                s => new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }).FirstOrDefaultAsync(s => s.id == schedule.admin);
                schedule.admin_email = temp.email;
            }
            return List;
        }


        [Authorize(Roles = "admin, worker, guest")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OutputScheduleDTO>> GetScheduleById(int companyId, int id)
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
            if (companyId != user.fk_company)
                return NotFound();

            OutputScheduleDTO schedule = await DBContext.Schedules.Select(
            s => new OutputScheduleDTO
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                admin = s.admin,
                admin_email = "",
            }
            ).FirstOrDefaultAsync(s => s.id == id);
            if (schedule == null)
                return NotFound();

            if (companyId != schedule.fk_company)
                return NotFound();

            User temp = await DBContext.Users.Select(
               s => new User
               {
                   id = s.id,
                   name = s.name,
                   fk_company = s.fk_company,
                   email = s.email,
                   role = s.role,
                   password = s.password
               }).FirstOrDefaultAsync(s => s.id == schedule.admin);
            schedule.admin_email = temp.email;

            return schedule;
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> InsertSchedule(int companyId, CreateScheduleDTO newSchedule)
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
            if (companyId != user.fk_company)
                return NotFound();

            User newAdmin = await DBContext.Users.Select(
                s=> new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }).FirstOrDefaultAsync(s => s.email == newSchedule.admin_email);
            if(newAdmin == null || newAdmin.fk_company != user.fk_company)
                return BadRequest("Vartotojas su tokiu elektroniniu paštu neegzistuoja");
            if (newAdmin.role != "admin")
                return BadRequest("Darbuotojas negali būti adminas");

            Schedule schedule = await DBContext.Schedules.Select(
            s => new Schedule
            {
                id = s.id,
                name = s.name,
                fk_company = s.fk_company,
                admin = s.admin,
            }
            ).FirstOrDefaultAsync(s => s.name == newSchedule.name);
            if (schedule != null)
                return BadRequest("Įmonėje jau yra sukurtas tvarkaraštis tokiu pavadinimu.");

            var entity = new Schedule()
            {
                name = newSchedule.name,
                fk_company = companyId,
                admin = newAdmin.id
            };
            DBContext.Schedules.Add(entity);
            await DBContext.SaveChangesAsync();

            return Created($"api/schedules/{entity.id}", entity);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchedule(int companyid, int id, UpdateScheduleDTO schedule)
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
            if (companyid != user.fk_company)
                return NotFound();

            User newAdmin = await DBContext.Users.Select(
                s => new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }).FirstOrDefaultAsync(s => s.email == schedule.admin_email);
            if (newAdmin == null || newAdmin.fk_company != user.fk_company)
                return BadRequest("Vartotojas su tokiu elektroniniu paštu neegzistuoja");
            if (newAdmin.role != "admin")
                return BadRequest("Darbuotojas negali būti adminas");

            var entity = await DBContext.Schedules.FirstOrDefaultAsync(s => s.id == id);
            if (entity == null)
                return NotFound();
            if (companyid != entity.fk_company)
                return NotFound();

            entity.name = schedule.name;
            entity.fk_company = user.fk_company;          
            entity.admin = newAdmin.id; 
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
            if (companyId != user.fk_company)
                return NotFound();

            Schedule Schedule = await DBContext.Schedules.Select(
                    s => new Schedule
                    {
                        id = s.id,
                        name = s.name,
                        fk_company = s.fk_company,
                        admin = s.admin,
                    })
                .FirstOrDefaultAsync(s => s.id == id);
            if (Schedule == null)
                return NotFound();
            if (companyId != Schedule.fk_company)
                return NotFound();

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
