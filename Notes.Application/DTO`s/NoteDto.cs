namespace Notes.Application.DTOs
{ 
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
