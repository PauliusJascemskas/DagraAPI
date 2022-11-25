using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using DagraAPI;
using DagraAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace ASP.NET_WebAPI6.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/schedules/{scheduleId}/jobs")]
    public class JobController : ControllerBase
    {
        private readonly DBContext DBContext;

        public JobController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [Authorize(Roles = "admin, worker, guest")]
        [HttpGet]
        public async Task<ActionResult<List<Job>>> Get(int scheduleId)
        {
            var List = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                s => new Job
                {
                    id = s.id,
                    name = s.name,
                    creator = s.creator,
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

        [Authorize(Roles = "admin, worker, guest")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJobById(int scheduleId, int id)
        {
            Job Job = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                    s => new Job
                    {
                        id = s.id,
                        name = s.name,
                        creator = s.creator,
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

        [Authorize(Roles = "admin, worker")]
        [HttpPost]
        public async Task<ActionResult> InsertJob(int scheduleId, CreateJobDTO job)
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

            var entity = new Job()
            {
                name = job.name,
                creator = user.id,
                start_date = job.start_date,
                end_date = job.end_date,
                fk_schedule = scheduleId,
            };

            DBContext.Jobs.Add(entity);
            await DBContext.SaveChangesAsync();

            return Created($"api/schedules/{scheduleId}/jobs/{entity.id}", entity);
        }

        [Authorize(Roles = "admin, worker")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateJob(int scheduleId, int id, JobDTO job)
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
            var entity = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).FirstOrDefaultAsync(s => s.id == id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                if (user.role == "Worker" && entity.creator != user.id)
                {
                    return Unauthorized();
                }
                entity.id = id;
                entity.name = job.name;
                entity.start_date = job.start_date;
                entity.end_date = job.end_date;
                entity.fk_schedule = scheduleId;
                await DBContext.SaveChangesAsync();
                return Ok(entity);
            }
            
        }

        [Authorize(Roles = "admin, worker")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJob(int scheduleId, int id)
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

            Job Job = await DBContext.Jobs.Where(s => s.fk_schedule == scheduleId).Select(
                    s => new Job
                    {
                        id = s.id,
                        name = s.name,
                        creator = s.creator,
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
                if (user.role == "Worker" && Job.creator != user.id)
                {
                    return Unauthorized();
                }
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
