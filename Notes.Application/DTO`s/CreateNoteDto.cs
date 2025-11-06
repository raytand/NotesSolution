namespace Notes.Application.DTOs
{
    public class CreateNoteDto
    {
        public string Title { get; set; } = default!;
        public string? Content { get; set; }
    }
}
