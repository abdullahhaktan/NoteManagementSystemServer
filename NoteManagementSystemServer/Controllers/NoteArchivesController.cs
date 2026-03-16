using Microsoft.AspNetCore.Mvc;
using NoteManagemenSystemServer.Services.NoteArhiveServices;

namespace NoteManagemenSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteArchivesController(INoteArchiveService noteArhiveService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMyArchiveNotes()
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                throw new ArgumentNullException("Kullanıcı kaydı bulunamadı");
            }
            var myNotes = await noteArhiveService.GetMyArchiveNotesAsync(userName);
            return Ok(myNotes);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArchiveNote(int id)
        {
            await noteArhiveService.DeleteNoteArchiveAsync(id);
            return NoContent();
        }
    }
}
