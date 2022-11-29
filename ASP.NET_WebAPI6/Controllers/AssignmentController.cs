using ASP.NET_WebAPI6.DTO;
using ASP.NET_WebAPI6.Entities;
using DagraAPI;
using DagraAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Data;
using System.Net;

namespace ASP.NET_WebAPI6.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/schedules/{scheduleId}/jobs/{jobId}/assignments")]
    public class AssignmentController : ControllerBase
    {
        private readonly DBContext DBContext;

        public AssignmentController(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [Authorize(Roles = "admin, worker")]
        [HttpGet]
        public async Task<ActionResult<List<Assignment>>> Get(int companyID, int scheduleId, int jobId)
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

            if (companyID != user.fk_company)
            {
                return Forbid();
            }

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId && s.fk_company == companyID);

            if (Schedule.fk_company != user.fk_company)
            {
                return Forbid();
            }


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
                .FirstOrDefaultAsync(s => s.id == jobId);

            if (Job.fk_schedule != scheduleId)
            {
                return Forbid();
            }

            var List = await DBContext.Assignments.Where(s => s.fk_job == jobId).Select(
                s => new Assignment
                {
                    id = s.id,
                    name = s.name,
                    start_time = s.start_time,
                    end_time = s.end_time,
                    fk_job = s.fk_job,
                    fk_assignee = s.fk_assignee,
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
        public async Task<ActionResult<Assignment>> GetAssignmentById(int companyID, int scheduleId, int jobId, int id)
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

            if (companyID != user.fk_company)
            {
                return Forbid();
            }

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId && s.fk_company == companyID);

            if (Schedule.fk_company != user.fk_company)
            {
                return Forbid();
            }


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
                .FirstOrDefaultAsync(s => s.id == jobId);

            if (Job.fk_schedule != scheduleId)
            {
                return Forbid();
            }

            Assignment Assignment = await DBContext.Assignments.Where(s => s.fk_job == jobId).Select(
                    s => new Assignment
                    {
                        id = s.id,
                        name = s.name,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_assignee = s.fk_assignee,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if(Assignment.fk_job != jobId) { return Forbid(); } 
            if (Assignment == null)
            {
                return NotFound();
            }
            else
            {
                return Assignment;
            }
        }

        [Authorize(Roles = "admin, worker")]
        [HttpPost]
        public async Task<ActionResult> InsertAssignment(int companyID, int scheduleId, int jobId, CreateAssignmentDTO assignment)
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

            if (companyID != user.fk_company)
            {
                return Forbid();
            }

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId && s.fk_company == companyID);

            if (Schedule.fk_company != user.fk_company)
            {
                return Forbid();
            }


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
                .FirstOrDefaultAsync(s => s.id == jobId);

            if (Job.fk_schedule != scheduleId)
            {
                return Forbid();
            }

            var entity = new Assignment()
            {
                name = assignment.name,
                creator = user.id,
                start_time = assignment.start_time,
                end_time = assignment.end_time,
                fk_job = jobId,
                fk_assignee = assignment.fk_assignee,
            };

            DBContext.Assignments.Add(entity);
            await DBContext.SaveChangesAsync();

            return Created($"api/companies/{companyID}/schedules/{scheduleId}/jobs/{jobId}/assignments/{entity.id}", entity);
        }

        [Authorize(Roles = "admin, worker")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAssignment(int companyID, int scheduleId, int jobId, int id, AssignmentDTO assignment)
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

            if (companyID != user.fk_company)
            {
                return Forbid();
            }

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId && s.fk_company == companyID);

            if (Schedule.fk_company != user.fk_company)
            {
                return Forbid();
            }


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
                .FirstOrDefaultAsync(s => s.id == jobId);

            if (Job.fk_schedule != scheduleId)
            {
                return Forbid();
            }

            Assignment Assignment = await DBContext.Assignments.Where(s => s.fk_job == jobId).Select(
                    s => new Assignment
                    {
                        id = s.id,
                        name = s.name,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_assignee = s.fk_assignee,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (Assignment.fk_job != jobId) { return Unauthorized(); }
            if(Assignment.creator!=user.id) { return Unauthorized(); }

            var entity = await DBContext.Assignments.Where(s => s.fk_job == jobId).FirstOrDefaultAsync(s => s.id == id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                if(user.role == "Worker" && entity.creator != user.id)
                {
                    return Forbid();
                }
                entity.id = entity.id;
                entity.name = assignment.name;
                entity.creator = entity.creator;
                entity.start_time = assignment.start_time;
                entity.end_time = assignment.end_time;
                entity.fk_job = jobId;
                entity.fk_assignee = assignment.fk_assignee;
                await DBContext.SaveChangesAsync();
                return Ok(entity);
            }
        }

        [Authorize(Roles = "admin, worker")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAssignment(int companyID, int scheduleId, int jobId, int id)
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

            if (companyID != user.fk_company)
            {
                return Forbid();
            }

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId && s.fk_company == companyID);

            if (Schedule.fk_company != user.fk_company)
            {
                return Forbid();
            }


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
                .FirstOrDefaultAsync(s => s.id == jobId);

            if (Job.fk_schedule != scheduleId)
            {
                return Forbid();
            }

            Assignment assignment = await DBContext.Assignments.Where(s => s.fk_job == jobId).Select(
                    s => new Assignment
                    {
                        id = s.id,
                        name = s.name,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_assignee = s.fk_assignee,
                    })
                .FirstOrDefaultAsync(s => s.id == id);

            if (assignment.fk_job != jobId) { return Forbid(); }
            if (assignment.creator != user.id) { return Forbid(); }

            if (assignment == null)
            {
                return NotFound();
            }
            else
            {
                if(user.role == "Worker" && assignment.creator != user.id)
                {
                    return Forbid();
                }
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
