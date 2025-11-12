using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notes.Infrastructure
{
    public class NotesDbContextFactory : IDesignTimeDbContextFactory<NotesDbContext>
    {
        public NotesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NotesDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=NotesDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
            );

            return new NotesDbContext(optionsBuilder.Options);
        }
    }
}
