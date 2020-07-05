using Microsoft.EntityFrameworkCore;
using projetoAlan.Models;

namespace projetoAlan.Context
{
    public class MyContext : DbContext
    {
         public MyContext(DbContextOptions<MyContext> options)
          : base(options)
        {}
        public DbSet<Usuario> Usuarios { get; set; }
        
    }
}