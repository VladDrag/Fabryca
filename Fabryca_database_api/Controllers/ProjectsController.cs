using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fabryca_database_api.Models;

namespace Fabryca_database_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly FabrycaContext _context;

        public ProjectsController(FabrycaContext context)
        {
            _context = context;
        }

        private List<ProjectToApi> DbToDTO(List<Project> projects)
        {
          var result = new List<ProjectToApi>();

          foreach (var project in projects)
          {
            var temp = new ProjectToApi(project);
            result.Add(temp);
          }
          return result;
        }
        // private bool TicketExists(int id) => (_context.Ticket?.Any(e => e.Id == id)).GetValueOrDefault();

        // GET: api/FabrukaDb
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectToApi>>> GetProjects()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
          var res =  DbToDTO(await _context.Projects.ToListAsync());
          return res;
        }
    
    //     // GET: api/FabrukaDb/5
    //     [HttpGet("{title}")]
    //     public async Task<ActionResult<TicketToApi>> GetTicket(string title)
    //     {
    //       if (_context.Ticket == null) return NotFound();

    //       var ticket = await _context.Ticket.Include(x => x.Category).FirstOrDefaultAsync(x => x.Title == title);

    //       if (ticket == null) return NotFound();

    //       return new TicketToApi(ticket);
    //     }

    //     // PUT: api/FabrukaDb/5
    //     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //     [HttpPut("{oldTitle}/title")]
    //     public async Task<IActionResult> PutTicketTitle(string oldTitle, string newTitle)
    //     {
    //       var ticketToChange = await _context.Ticket.FirstOrDefaultAsync(x => x.Title == oldTitle);
    //       if (ticketToChange == null) return BadRequest();

    //       ticketToChange.Title = newTitle;
    //       _context.Entry(ticketToChange).State = EntityState.Modified;

    //       await _context.SaveChangesAsync();

    //       return NoContent();
    //     }

    //     [HttpPut("{title}/description")]
    //     public async Task<IActionResult> PutTicketDescription(string title, string description)
    //     {
    //       var ticketToChange = await _context.Ticket.FirstOrDefaultAsync(x => x.Title == title);
    //       if (ticketToChange == null) return BadRequest();

    //       ticketToChange.Description = description;
    //       _context.Entry(ticketToChange).State = EntityState.Modified;

    //       await _context.SaveChangesAsync();

    //       return NoContent();
    //     }

    //     [HttpPut("{title}/status")]
    //     public async Task<IActionResult> PutTicketStatus(string title, string status)
    //     {
    //       var ticketToChange = await _context.Ticket.FirstOrDefaultAsync(x => x.Title == title);
    //       if (ticketToChange == null) return BadRequest();

    //       ticketToChange.Status = status;
    //       _context.Entry(ticketToChange).State = EntityState.Modified;

    //       await _context.SaveChangesAsync();

    //       return NoContent();
    //     }

    //     [HttpPut("{title}/category")]
    //     public async Task<IActionResult> PutTicketCategory(string title, string categoryName)
    //     {
    //       var ticketToChange = await _context.Ticket.FirstOrDefaultAsync(x => x.Title == title);
    //       if (ticketToChange == null) return BadRequest();

    //       var category = await _context.Category.FirstOrDefaultAsync(x => x.Name == categoryName);
    //       if ( category != null)
    //       {
    //         ticketToChange.Category = category;
    //         _context.Entry(ticketToChange).State = EntityState.Modified;
    //         await _context.SaveChangesAsync();
    //       }

    //       return NoContent();
    //     }

    //     [HttpPut("{title}/ticket")]
    //     public async Task<IActionResult> PutTicket(string title, string? newTitle, string? newStatus, string? newCategoryName, string? newDescription)
    //     {
    //       var ticketToChange = await _context.Ticket.FirstOrDefaultAsync(x => x.Title == title);
    //       if (ticketToChange == null) return BadRequest();

    //       var category = await _context.Category.FirstOrDefaultAsync(x => x.Name == newCategoryName);


    //       if (category != null) ticketToChange.Category = category;
    //       if (!string.IsNullOrEmpty(newTitle))
    //       {
    //         var test = await _context.Ticket.Where(x => x.Title != title).FirstOrDefaultAsync(x => x.Title == newTitle);
    //         if (test != null) return BadRequest();
    //         ticketToChange.Title = newTitle;
    //       }
    //       if (!string.IsNullOrEmpty(newStatus)) ticketToChange.Status = newStatus;
    //       if (!string.IsNullOrEmpty(newDescription)) ticketToChange.Description = newDescription;

    //       _context.Entry(ticketToChange).State = EntityState.Modified;
    //       await _context.SaveChangesAsync();

    //       return NoContent();
    //     }



    //     // POST: api/FabrykaDb
    //     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectCreation project)
        {
        //   Category category = await _context.Category.FirstOrDefaultAsync(x => x.Name == "Planned");

          var tick = await _context.Projects.FirstOrDefaultAsync(x => x.Name == project.Name);

          if (tick != null)
          {
            return BadRequest("Please choose a unique title for the ticket");
          }

          var DbProject = new Project();

          if (!string.IsNullOrEmpty(project.Name))
          {
            DbProject = new Project { 
                                        Name = project.Name
                                      };
            if (_context.Projects == null)
            {
                return Problem("Entity set 'FabrycaContext.Projects'  is null.");
            }
              _context.Projects.Add(DbProject);
              await _context.SaveChangesAsync();
          }
            // return CreatedAtAction("GetProject", new { name = project.Name }, project); ADD THIS WHEN THE OTHER ROUTES ARE MADE
            return Ok(project);
        }

    //     }

    //     // DELETE: api/FabrukaDb/5
    //     [HttpDelete("{title}")]
    //     public async Task<IActionResult> DeleteTicket(string title)
    //     {
    //         if (_context.Ticket == null)
    //         {
    //             return NotFound();
    //         }
    //         var ticket = await _context.Ticket.FirstOrDefaultAsync(x => x.Title == title);
    //         if (ticket == null)
    //         {
    //             return NotFound();
    //         }

    //         _context.Ticket.Remove(ticket);
    //         await _context.SaveChangesAsync();

    //         return NoContent();
    //     }
    }
}