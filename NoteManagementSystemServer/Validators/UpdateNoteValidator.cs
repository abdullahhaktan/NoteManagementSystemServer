using FluentValidation;
using NoteManagemenSystemServer.Data.DTOs.NoteDtos;

namespace NoteManagemenSystemServer.Validators
{
    public class UpdateNoteValidator : AbstractValidator<UpdateNoteDto>
    {
        private readonly string[] _allowedTypes = {
            "application/pdf",
            "image/png",
            "image/jpeg",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };
        private const long _maxFileSize = 10 * 1024 * 1024;

        public UpdateNoteValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz not ID.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("Ders adı boş olamaz.")
                .MaximumLength(100).WithMessage("Ders adı en fazla 100 karakter olabilir.");

            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File.Length)
                    .LessThanOrEqualTo(_maxFileSize)
                    .WithMessage("Dosya boyutu en fazla 10 MB olabilir.");

                RuleFor(x => x.File.ContentType)
                    .Must(type => _allowedTypes.Contains(type))
                    .WithMessage("Sadece PDF, PNG, JPG ve DOCX dosyaları yüklenebilir.");
            });
        }
    }
}