using Microsoft.EntityFrameworkCore;


namespace GreenLoop.DAL.Data
{
    public class GreenLoopDbContext: DbContext
    {
        public GreenLoopDbContext(DbContextOptions<GreenLoopDbContext> options)
            : base(options)
        {
        }


        // DbSets 
        // public DbSet<User> Users { get; set; }
        // public DbSet<Product> Products { get; set; }
    }
}
