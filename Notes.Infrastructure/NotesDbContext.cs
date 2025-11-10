using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Infrastructure
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Note> Notes => Set<Note>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .Property(n => n.Id)
                .ValueGeneratedOnAdd();
        }
    }

}