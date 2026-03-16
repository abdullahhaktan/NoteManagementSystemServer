using NoteManagemenSystemServer.Data.DTOs.NoteDtos;

namespace NoteManagemenSystemServer.Services.NoteArhiveServices
{
    public interface INoteArchiveService
    {
        Task<List<ResultNoteDto>> GetMyArchiveNotesAsync(string userName);
        Task DeleteNoteArchiveAsync(int id);
    }
}
