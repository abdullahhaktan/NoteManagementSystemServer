using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteManagemenSystemServer.Data.DTOs.NoteDtos;
using NoteManagemenSystemServer.Data.Entities;
using NoteManagemenSystemServer.Services.NoteServices;

namespace NoteManagemenSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController(UserManager<AppUser> userManager, INoteService noteService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMyNotes()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var userName = User.Identity.Name;

            if (userName == null)
            {
                throw new ArgumentNullException("Kullanıcı bulunamadı");
            }

            var myNotes = await noteService.GetMyNotesAsync(userName);
            return Ok(myNotes);
        }

        [HttpGet("GetNoteById/{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var value = await noteService.GetNoteByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromForm] CreateNoteDto createNoteDto)
        {
            var value = await noteService.CreateNoteAsync(createNoteDto);
            return CreatedAtAction(nameof(GetNoteById), new { id = value.Id }, value);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int id)
        {
            await noteService.DeleteNoteAsync(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromForm] UpdateNoteDto updateNoteDto)
        {
            await noteService.UpdateNoteAsync(updateNoteDto);
            return Ok(updateNoteDto);
        }
    }
}