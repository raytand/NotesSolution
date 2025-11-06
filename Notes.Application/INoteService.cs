using Notes.Application.DTOs;

namespace Notes.Application
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetAllAsync();
        Task<NoteDto> CreateAsync(CreateNoteDto dto);
    }
}