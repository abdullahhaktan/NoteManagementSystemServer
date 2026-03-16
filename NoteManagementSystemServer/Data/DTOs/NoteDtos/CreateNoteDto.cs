using System.ComponentModel.DataAnnotations;

namespace NoteManagemenSystemServer.Data.DTOs.NoteDtos
{
    public class CreateNoteDto
    {

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string CourseName { get; set; }

        public IFormFile? File { get; set; }

        public int UserId { get; set; }
    }
}
