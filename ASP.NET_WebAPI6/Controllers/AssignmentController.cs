using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ASP.NET_WebAPI6.Controllers
{
    [ApiController]
    [Route("api/schedules/{scheduleId}/jobs/{jobId}/assignments")]
    public class AssignmentController : ControllerBase
    {
        private readonly DBContext DBContext;

        public AssignmentController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssignmentDTO>>> Get(int scheduleId, int jobId)
        {
            var List = await DBContext.Assignments.Where(s => s.fk_schedule == scheduleId && s.fk_job == jobId).Select(
                s => new AssignmentDTO
                {
                    id = s.id,
                    name = s.name,
                    start_time = s.start_time,
                    end_time = s.end_time,
                    fk_job = s.fk_job,
                    fk_worker = s.fk_worker,
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
        public async Task<ActionResult<AssignmentDTO>> GetAssignmentById(int scheduleId, int jobId, int id)
        {
            AssignmentDTO Assignment = await DBContext.Assignments.Where(s => s.fk_schedule == scheduleId && s.fk_job == jobId).Select(
                    s => new AssignmentDTO
                    {
                        id = s.id,
                        name = s.name,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_worker = s.fk_worker,
                        fk_schedule = s.fk_schedule,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (Assignment == null)
            {
                return NotFound();
            }
            else
            {
                return Assignment;
            }
        }

        [HttpPost]
        public async Task<HttpStatusCode> InsertAssignment(int scheduleId, int jobId, AssignmentDTO assignment)
        {
            var entity = new Assignment()
            {
                id = assignment.id,
                name = assignment.name,
                start_time = assignment.start_time,
                end_time = assignment.end_time,
                fk_job = assignment.fk_job,
                fk_worker = assignment.fk_worker,
                fk_schedule = assignment.fk_schedule,
            };

            DBContext.Assignments.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAssignment(int scheduleId, int jobId, AssignmentDTO assignment)
        {
            var entity = await DBContext.Assignments.Where(s => s.fk_schedule == scheduleId && s.fk_job == jobId).FirstOrDefaultAsync(s => s.id == assignment.id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.id = assignment.id;
                entity.name = assignment.name;
                entity.start_time = assignment.start_time;
                entity.end_time = assignment.end_time;
                entity.fk_job = assignment.fk_job;
                entity.fk_schedule = assignment.fk_schedule;
                entity.fk_worker = assignment.fk_worker;
                await DBContext.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAssignment(int scheduleId, int jobId, int id)
        {
            AssignmentDTO assignment = await DBContext.Assignments.Where(s => s.fk_schedule == scheduleId && s.fk_job == jobId).Select(
                    s => new AssignmentDTO
                    {
                        id = s.id,
                        name = s.name,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_schedule = s.fk_schedule,
                        fk_worker = s.fk_worker,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (assignment == null)
            {
                return NotFound();
            }
            else
            {
                var entity = new Assignment()
                {
                    id = id
                };
                DBContext.Assignments.Attach(entity);
                DBContext.Assignments.Remove(entity);
                await DBContext.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
