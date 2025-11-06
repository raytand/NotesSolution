using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Infrastructure
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Note> Notes => Set<Note>();
    }

}