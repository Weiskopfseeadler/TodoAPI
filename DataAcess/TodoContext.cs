using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users {get;set;}
        
     
    }
}
