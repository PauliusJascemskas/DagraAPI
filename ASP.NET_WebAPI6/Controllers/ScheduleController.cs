using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
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

        public ScheduleController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<ScheduleDTO>>> Get()
        {
            var List = await DBContext.Schedules.Select(
                s => new ScheduleDTO
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
        public async Task<ActionResult<ScheduleDTO>> GetScheduleById(int id)
        {
            ScheduleDTO Schedule = await DBContext.Schedules.Select(
                    s => new ScheduleDTO
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
                return Schedule;
            }
        }

        [HttpPost]
        public async Task<HttpStatusCode> InsertSchedule(ScheduleDTO schedule)
        {
            var entity = new Schedule()
            {
                id = schedule.id,
                name = schedule.name,
                company = schedule.company,
                admin = schedule.admin,
            };

            DBContext.Schedules.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("{id}")]
        public async Task<HttpStatusCode> UpdateSchedule(ScheduleDTO schedule)
        {
            var entity = await DBContext.Schedules.FirstOrDefaultAsync(s => s.id == schedule.id);

            entity.id = schedule.id;
            entity.name = schedule.name;
            entity.company = schedule.company;
            entity.admin = schedule.admin;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("{id}")]
        public async Task<HttpStatusCode> DeleteSchedule(int id)
        {
            var entity = new Schedule()
            {
                id = id
            };
            DBContext.Schedules.Attach(entity);
            DBContext.Schedules.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
