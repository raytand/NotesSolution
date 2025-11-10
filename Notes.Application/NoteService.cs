using Microsoft.EntityFrameworkCore;
using Notes.Application.DTOs;
using Notes.Domain;
using Notes.Infrastructure;
using Confluent.Kafka;

namespace Notes.Application
{

    public class NoteService : INoteService
    {
        private readonly NotesDbContext _db;
        private readonly IProducer<Null, string> _producer;

        public NoteService(NotesDbContext db, IProducer<Null, string> producer)
        {
            _db = db;
            _producer = producer;
        }


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
            await _producer.ProduceAsync("notes-topic", new Message<Null, string> { Value = $"Note created: {note.Id}" });
            _producer.Flush(TimeSpan.FromSeconds(5));
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