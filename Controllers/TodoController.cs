using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    /*
    Use Following Credentials for the Database:
    Username: todo.db.service
    Password: [DEIN HEIMATORT]90
     */
    
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ToDoContext _context;

        public TodoController(ToDoContext context)
        {
            
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem
                {
                    Name = "Fügen Sie ein neues ToDo hinzu. ",
                    DueDate = DateTime.Today.AddDays(1),
                    Operator = "Sie"
                });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(int id,TodoItem todoItem)
        {
            if (id!=0)
            {
                System.Console.WriteLine("____________________________________________________");
                System.Console.WriteLine(id);
                todoItem.User = _context.Users.First((x)=>x.Id == id);
                System.Console.WriteLine(todoItem.User);
            }
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.TodoItems.ToListAsync();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            Console.WriteLine("delet API");
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return await _context.TodoItems.ToListAsync();
        }

//TODO Repair GetTodoItemOfUser
        [HttpGet("GetTodoItemOfUser/{id}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemOfUser(long id)

        {
            
            List<TodoItem> TodoList = new List<TodoItem>();
             var list = await _context.TodoItems.Where( t=> t.User.Id== id).ToListAsync();
            foreach (var item in list)
            {
                TodoList.Add(item);                
                
            }
            if (TodoList == null)
            {
                return NotFound();
            }

            return TodoList;
        }
    }
}