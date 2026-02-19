using Microsoft.EntityFrameworkCore;
using PillsServer.Common;

namespace PillsServer.Persistancy
{
    public class PillsDbContext : DbContext
    {
        public PillsDbContext(DbContextOptions<PillsDbContext> options) : base(options)
        {
        }
        public DbSet<Pill> Pills { get; set; }
    }
}
