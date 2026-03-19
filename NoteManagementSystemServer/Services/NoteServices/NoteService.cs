using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteManagemenSystemServer.Context;
using NoteManagemenSystemServer.Data.DTOs.NoteDtos;
using NoteManagemenSystemServer.Data.Entities;

namespace NoteManagemenSystemServer.Services.NoteServices
{
    public class NoteService : INoteService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly NoteManagementContext _noteManagementContext;
        private readonly string _uploadPath;

        public NoteService(UserManager<AppUser> userManager, NoteManagementContext noteManagementContext)
        {
            _userManager = userManager;
            _noteManagementContext = noteManagementContext;

            // Ensure the Uploads folder exists on startup
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public async Task<List<ResultNoteDto>> GetMyNotesAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            // Only return notes that haven't been soft-deleted (DeletedAt == null)
            var myNotes = await _noteManagementContext.Notes.Where(n => n.UserId == user.Id && n.DeletedAt == null).ToListAsync();
            var values = myNotes.Adapt<List<ResultNoteDto>>();
            return values;
        }

        public async Task<Note> CreateNoteAsync(CreateNoteDto createNoteDto)
        {
            var note = createNoteDto.Adapt<Note>();

            // If a file was uploaded, save it and populate file-related fields
            if (createNoteDto.File != null)
            {
                var savedFileName = await SaveFileAsync(createNoteDto.File);
                note.FileName = createNoteDto.File.FileName;
                note.FilePath = $"/uploads/{savedFileName}";
                note.FileType = createNoteDto.File.ContentType;
                note.FileSize = FormatFileSize(createNoteDto.File.Length);
            }

            await _noteManagementContext.AddAsync(note);
            await _noteManagementContext.SaveChangesAsync();

            return note;
        }

        public async Task<GetNoteByIdDto> GetNoteByIdAsync(int id)
        {
            var note = await _noteManagementContext.Notes.FindAsync(id);

            if (note == null)
            {
                throw new ArgumentNullException("Silinecek note bulunamadı");
            }

            var value = note.Adapt<GetNoteByIdDto>();
            return value;
        }

        public async Task UpdateNoteAsync(UpdateNoteDto updateNoteDto)
        {
            var existingNote = await _noteManagementContext.Notes.FindAsync(updateNoteDto.Id);

            if (existingNote == null)
            {
                throw new ArgumentNullException("Güncellenecek note bulunamadı");
            }

            // Mevcut notu güncelle
            existingNote.Title = updateNoteDto.Title;
            existingNote.CourseName = updateNoteDto.CourseName;
            existingNote.UpdatedDate = DateTime.Now;

            // YENİ DOSYA GELDİYSE
            if (updateNoteDto.File != null)
            {
                // Delete the old file from disk before saving the new one
                if (!string.IsNullOrEmpty(existingNote.FilePath))
                {
                    var oldFileName = Path.GetFileName(existingNote.FilePath);
                    var oldFilePath = Path.Combine(_uploadPath, oldFileName);
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                // Yeni dosyayı kaydet
                var savedFileName = await SaveFileAsync(updateNoteDto.File);
                existingNote.FileName = updateNoteDto.File.FileName;
                existingNote.FilePath = $"/uploads/{savedFileName}";
                existingNote.FileType = updateNoteDto.File.ContentType;
                existingNote.FileSize = FormatFileSize(updateNoteDto.File.Length);
            }

            _noteManagementContext.Notes.Update(existingNote);
            await _noteManagementContext.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = await _noteManagementContext.Notes.FindAsync(id);

            if (note == null)
            {
                throw new ArgumentNullException("Silinecek note bulunamadı");
            }

            // Soft delete — we don't remove the record, just set DeletedAt timestamp
            note.DeletedAt = DateTime.Now;
            _noteManagementContext.Notes.Update(note);
            await _noteManagementContext.SaveChangesAsync();
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Use a GUID as filename to prevent collisions and avoid exposing original names
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private string FormatFileSize(long bytes)
        {
            // Converts raw byte count to human-readable format (KB, MB, GB)
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}