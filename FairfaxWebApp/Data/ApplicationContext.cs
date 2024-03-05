using Microsoft.EntityFrameworkCore;

namespace FairfaxWebApp.Data
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
        public virtual DbSet<Bulletin> Bulletins { get; set; }

    }
}
