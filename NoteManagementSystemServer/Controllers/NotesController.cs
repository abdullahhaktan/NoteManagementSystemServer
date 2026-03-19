using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteManagemenSystemServer.Data.DTOs.NoteDtos;
using NoteManagemenSystemServer.Data.Entities;
using NoteManagemenSystemServer.Services.NoteServices;

namespace NoteManagemenSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly INoteService _noteService;
        private readonly IValidator<CreateNoteDto> _createValidator;
        private readonly IValidator<UpdateNoteDto> _updateValidator;

        public NotesController(
            UserManager<AppUser> userManager,
            INoteService noteService,
            IValidator<CreateNoteDto> createValidator,
            IValidator<UpdateNoteDto> updateValidator)
        {
            _userManager = userManager;
            _noteService = noteService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotes()
        {
            // Verify the token contains a valid UserId claim
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var userName = User.Identity.Name;
            if (userName == null)
                throw new ArgumentNullException("Kullanıcı bulunamadı");

            var myNotes = await _noteService.GetMyNotesAsync(userName);
            return Ok(myNotes);
        }

        [HttpGet("GetNoteById/{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var value = await _noteService.GetNoteByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromForm] CreateNoteDto createNoteDto)
        {
            // Validate before processing — catches empty fields, invalid file type/size etc.
            var result = await _createValidator.ValidateAsync(createNoteDto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var value = await _noteService.CreateNoteAsync(createNoteDto);
            return CreatedAtAction(nameof(GetNoteById), new { id = value.Id }, value);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int id)
        {
            // Soft delete — sets DeletedAt timestamp instead of removing from database
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromForm] UpdateNoteDto updateNoteDto)
        {
            // Validate before processing — file validation only runs if a new file is provided
            var result = await _updateValidator.ValidateAsync(updateNoteDto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            await _noteService.UpdateNoteAsync(updateNoteDto);
            return Ok(updateNoteDto);
        }
    }
}