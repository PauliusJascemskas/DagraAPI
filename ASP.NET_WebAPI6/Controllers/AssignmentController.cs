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

        [Authorize(Roles = "admin, worker, guest")]
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
                return NotFound();

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId);
            if (Schedule == null ||Schedule.fk_company != user.fk_company)
                return NotFound();

            Job Job = await DBContext.Jobs.Select(
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
            if (Job == null || Job.fk_schedule != Schedule.id)
                return NotFound();

            var List = await DBContext.Assignments.Where(s => s.fk_job == Job.id).Select(
                s => new Assignment
                {
                    id = s.id,
                    name = s.name,
                    creator = s.creator,
                    start_time = s.start_time,
                    end_time = s.end_time,
                    fk_job = s.fk_job,
                    fk_assignee = s.fk_assignee,
                }
            ).ToListAsync();

            return List;
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
                return NotFound();

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId);
            if (Schedule == null || Schedule.fk_company != user.fk_company)
                return NotFound();

            Job Job = await DBContext.Jobs.Select(
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
            if (Job == null || Job.fk_schedule != Schedule.id)
                return NotFound();

            Assignment Assignment = await DBContext.Assignments.Select(
                    s => new Assignment
                    {
                        id = s.id,
                        name = s.name,
                        creator = s.creator,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_assignee = s.fk_assignee,
                    })
                .FirstOrDefaultAsync(s => s.id == id);
            if(Assignment == null || Assignment.fk_job != Job.id) 
                return NotFound(); 

            return Assignment;
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
                return NotFound();

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId);
            if (Schedule == null || Schedule.fk_company != user.fk_company)
                return NotFound();

            Job Job = await DBContext.Jobs.Select(
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
            if (Job == null || Job.fk_schedule != Schedule.id)
                return NotFound();

            User assignee = await DBContext.Users.Select(
                s => new User
                {
                    id = s.id,
                    name = s.name,
                    fk_company = s.fk_company,
                    email = s.email,
                    role = s.role,
                    password = s.password
                }).FirstOrDefaultAsync(s => s.email == assignment.assignee_email);
            if (assignee == null || companyID != assignee.fk_company)
                return BadRequest("Nurodytas darbuotojas neegzistuoja.");

            var entity = new Assignment()
            {
                name = assignment.name,
                creator = user.id,
                start_time = assignment.start_time,
                end_time = assignment.end_time,
                fk_job = jobId,
                fk_assignee = assignee.id,
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
                return NotFound();

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId);
            if (Schedule == null || Schedule.fk_company != user.fk_company)
                return NotFound();

            Job Job = await DBContext.Jobs.Select(
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
            if (Job == null || Job.fk_schedule != Schedule.id)
                return NotFound();

            User assignee = await DBContext.Users.Select(
               s => new User
               {
                   id = s.id,
                   name = s.name,
                   fk_company = s.fk_company,
                   email = s.email,
                   role = s.role,
                   password = s.password
               }).FirstOrDefaultAsync(s => s.email == assignment.assignee_email);
            if (assignee == null || companyID != assignee.fk_company)
                return BadRequest("Nurodytas darbuotojas neegzistuoja.");


            var entity = await DBContext.Assignments.FirstOrDefaultAsync(s => s.id == id);
            if (entity == null || entity.fk_job != Job.id)
                return NotFound();

            if(user.role == "worker" && entity.creator != user.id)
                return Forbid();

            entity.id = entity.id;
            entity.name = assignment.name;
            entity.creator = entity.creator;
            entity.start_time = assignment.start_time;
            entity.end_time = assignment.end_time;
            entity.fk_job = jobId;
            entity.fk_assignee = assignee.id;
            await DBContext.SaveChangesAsync();
            return Ok(entity);
            
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
                return NotFound();

            Schedule Schedule = await DBContext.Schedules.Select(
                     s => new Schedule
                     {
                         id = s.id,
                         name = s.name,
                         fk_company = s.fk_company,
                         admin = s.admin,
                     })
                 .FirstOrDefaultAsync(s => s.id == scheduleId);
            if (Schedule == null || Schedule.fk_company != user.fk_company)
                return NotFound();

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
            if (Job == null || Job.fk_schedule != Schedule.id)
                return NotFound();

            Assignment assignment = await DBContext.Assignments.Select(
                    s => new Assignment
                    {
                        id = s.id,
                        name = s.name,
                        creator = s.creator,
                        start_time = s.start_time,
                        end_time = s.end_time,
                        fk_job = s.fk_job,
                        fk_assignee = s.fk_assignee,
                    })
                .FirstOrDefaultAsync(s => s.id == id);
            if (assignment == null || assignment.fk_job != Job.id) 
                return NotFound(); 

            if(user.role == "worker" && assignment.creator != user.id)
                return Forbid();
            
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
