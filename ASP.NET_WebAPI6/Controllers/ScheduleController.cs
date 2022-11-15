using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ASP.NET_WebAPI6.Controllers
{
    [ApiController]
    [Route("api/schedules")]
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

        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> Get()
        {
            var List = await DBContext.Schedules.Select(
                s => new Schedule
                {
                    id = s.id,
                    name = s.name,
                    company = s.company,
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetScheduleById(int id)
        {
            Schedule Schedule = await DBContext.Schedules.Select(
                    s => new Schedule
                    {
                        id = s.id,
                        name = s.name,
                        company = s.company,
                        admin = s.admin,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (Schedule == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Schedule);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertSchedule(Schedule schedule)
        {
            var entity = new Schedule()
            {
                name = schedule.name,
                company = schedule.company,
                admin = schedule.admin,
            };

            DBContext.Schedules.Add(entity);
            await DBContext.SaveChangesAsync();

            return Created($"api/schedules/{schedule.id}", entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchedule(int id, ScheduleDTO schedule)
        {
            var entity = await DBContext.Schedules.FirstOrDefaultAsync(s => s.id == id);

            //entity.id = id;
            entity.name = schedule.name;
            entity.company = schedule.company;
            entity.admin = schedule.admin; 

            await DBContext.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchedule(int id)
        {
            Schedule Schedule = await DBContext.Schedules.Select(
                    s => new Schedule
                    {
                        id = s.id,
                        name = s.name,
                        company = s.company,
                        admin = s.admin,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

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
