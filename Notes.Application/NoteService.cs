using Microsoft.EntityFrameworkCore;
using Notes.Application.DTOs;
using Notes.Domain;
using Notes.Infrastructure;

namespace Notes.Application
{

    public class NoteService : INoteService
    {
        private readonly NotesDbContext _db;
        public NoteService(NotesDbContext db) => _db = db;

        public async Task<IEnumerable<NoteDto>> GetAllAsync()
        {
            return await _db.Notes
                .AsNoTracking()
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<NoteDto> CreateAsync(CreateNoteDto dto)
        {
            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };
            _db.Notes.Add(note);
            await _db.SaveChangesAsync();

            return new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt
            };
        }
    }
}