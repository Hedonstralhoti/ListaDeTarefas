using Microsoft.EntityFrameworkCore;

namespace MyTodoApp.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        public DbSet<ToDo> ToDo { get; set; }
    }
}
