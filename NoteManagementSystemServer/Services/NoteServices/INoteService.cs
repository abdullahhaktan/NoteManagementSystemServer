using NoteManagemenSystemServer.Data.DTOs.NoteDtos;
using NoteManagemenSystemServer.Data.Entities;

namespace NoteManagemenSystemServer.Services.NoteServices
{
    public interface INoteService
    {
        Task<List<ResultNoteDto>> GetMyNotesAsync(string userName);
        Task<Note> CreateNoteAsync(CreateNoteDto createNoteDto);
        Task<GetNoteByIdDto> GetNoteByIdAsync(int id);
        Task UpdateNoteAsync(UpdateNoteDto updateNoteDto);
        Task DeleteNoteAsync(int id);
        Task<string> SaveFileAsync(IFormFile file);
    }
}
