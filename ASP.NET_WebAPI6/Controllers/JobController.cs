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
        public async Task<ActionResult<List<Job>>> Get(int scheduleId)
        {
            var List = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                s => new Job
                {
                    id = s.id,
                    name = s.name,
                    start_date = s.start_date,
                    end_date = s.end_date,
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
        public async Task<ActionResult<Job>> GetJobById(int scheduleId, int id)
        {
            Job Job = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                    s => new Job
                    {
                        id = s.id,
                        name = s.name,
                        start_date = s.start_date,
                        end_date = s.end_date,
                        fk_schedule = s.fk_schedule,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (Job == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Job);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertJob(int scheduleId, CreateJobDTO job)
        {
            var entity = new Job()
            {
                //id = job.id,
                name = job.name,
                start_date = job.start_date,
                end_date = job.end_date,
                fk_schedule = scheduleId,
            };

            DBContext.Jobs.Add(entity);
            await DBContext.SaveChangesAsync();

            return Created($"api/schedules/{scheduleId}/jobs/{entity.id}", entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateJob(int scheduleId, int id, JobDTO job)
        {
            var entity = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).FirstOrDefaultAsync(s => s.id == id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.id = id;
                entity.name = job.name;
                entity.start_date = job.start_date;
                entity.end_date = job.end_date;
                entity.fk_schedule = scheduleId;
                await DBContext.SaveChangesAsync();
                return Ok(entity);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJob(int scheduleId, int id)
        {
            Job Job = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                    s => new Job
                    {
                        id = s.id,
                        name = s.name,
                        start_date = s.start_date,
                        end_date = s.end_date,
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
