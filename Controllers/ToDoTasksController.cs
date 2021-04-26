using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly ToDoAppDbContext _context;

        public ToDoTasksController(ToDoAppDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTasks
        [HttpGet,Authorize]
        public async Task<ActionResult<IEnumerable<ToDoTasks>>> GetToDoTasks()
        {
            return await _context.ToDoTasks.OrderBy(x => x.SLNo).ToListAsync();
        }

        [HttpPost]
        public async Task<bool> PostToDoTasks(List<ToDoTasks> toDoTasks)
        {
            try
            {
                int noOfRowDeleted = _context.Database.ExecuteSqlRaw("Delete from ToDoTasks");
                //_context.ToDoTasks.RemoveRange(_context.ToDoTasks);
                foreach (var item in toDoTasks)
                {
                    item.ID = Guid.NewGuid();
                    _context.ToDoTasks.Add(item);
                }
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ToDoTasksExists(Guid? id)
        {
            return _context.ToDoTasks.Any(e => e.ID == id);
        }
    }
}
