using System.ComponentModel.DataAnnotations;

namespace NoteManagemenSystemServer.Data.DTOs.NoteDtos
{
    public class UpdateNoteDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string CourseName { get; set; }

        public IFormFile? File { get; set; }

        public int UserId { get; set; }
    }
}
