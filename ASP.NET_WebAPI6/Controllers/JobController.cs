using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ASP.NET_WebAPI6.Controllers
{
    [ApiController]
    [Route("api/schedules/{scheduleId}/jobs")]
    public class JobController : ControllerBase
    {
        private readonly DBContext DBContext;

        public JobController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<JobDTO>>> Get(int scheduleId)
        {
            var List = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                s => new JobDTO
                {
                    id = s.id,
                    name = s.name,
                    fk_schedule = s.fk_schedule,
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
        public async Task<ActionResult<JobDTO>> GetJobById(int scheduleId, int id)
        {
            JobDTO Job = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                    s => new JobDTO
                    {
                        id = s.id,
                        name = s.name,
                        fk_schedule = s.fk_schedule,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (Job == null)
            {
                return NotFound();
            }
            else
            {
                return Job;
            }
        }

        [HttpPost]
        public async Task<HttpStatusCode> InsertJob(int scheduleId, JobDTO job)
        {
            var entity = new Job()
            {
                id = job.id,
                name = job.name,
                fk_schedule = job.fk_schedule,
            };

            DBContext.Jobs.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateJob(int scheduleId, JobDTO job)
        {
            var entity = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).FirstOrDefaultAsync(s => s.id == job.id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.id = job.id;
                entity.name = job.name;
                entity.fk_schedule = job.fk_schedule;
                await DBContext.SaveChangesAsync();
                return Ok();
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJob(int scheduleId, int id)
        {
            JobDTO Job = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                    s => new JobDTO
                    {
                        id = s.id,
                        name = s.name,
                        fk_schedule = s.fk_schedule,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (Job == null)
            {
                return NotFound();
            }
            else
            {
                var entity = new Job()
                {
                    id = id
                };
                DBContext.Jobs.Attach(entity);
                DBContext.Jobs.Remove(entity);
                await DBContext.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
