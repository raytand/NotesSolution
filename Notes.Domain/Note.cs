namespace Notes.Domain
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

