using NoteManagemenSystemServer.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace NoteManagemenSystemServer.Data.DTOs.NoteDtos
{
    public class ResultNoteDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string CourseName { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
