using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteManagemenSystemServer.Context;
using NoteManagemenSystemServer.Data.DTOs.NoteDtos;
using NoteManagemenSystemServer.Data.Entities;

namespace NoteManagemenSystemServer.Services.NoteArhiveServices
{
    public class NoteArchiveService : INoteArchiveService
    {
        private readonly UserManager<AppUser> _userManager;
        private NoteManagementContext _noteManagementContext;
        private readonly string _uploadPath;

        public NoteArchiveService(UserManager<AppUser> userManager, NoteManagementContext noteManagementContext)
        {
            _userManager = userManager;
            _noteManagementContext = noteManagementContext;

            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }


        public async Task<List<ResultNoteDto>> GetMyArchiveNotesAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var myArchiveNotes = await _noteManagementContext.Notes.Where(n => n.UserId == user.Id && n.DeletedAt != null).ToListAsync();
            var values = myArchiveNotes.Adapt<List<ResultNoteDto>>();
            return values;
        }

        public async Task DeleteNoteArchiveAsync(int id)
        {
            var note = await _noteManagementContext.Notes.FindAsync(id);
            if (note == null)
            {
                throw new ArgumentNullException("Silinecek Not Arşivi bulunamadı");
            }

            var filePath = Path.GetFileName(note.FilePath);
            var fullFilePath = Path.Combine(_uploadPath, filePath);

            File.Delete(fullFilePath);
            _noteManagementContext.Notes.Remove(note);
            await _noteManagementContext.SaveChangesAsync();
        }
    }
}
