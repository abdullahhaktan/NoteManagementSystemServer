using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteManagemenSystemServer.Data.Entities;

namespace NoteManagemenSystemServer.Context
{
    public class NoteManagementContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public NoteManagementContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity tablolarının isimlerini düzenleme
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");

            // 🔐 Kullanıcı Seed Data
            var hasher = new PasswordHasher<AppUser>();

            var adminUser = new AppUser
            {
                Id = 1,
                UserName = "tetacode",
                NormalizedUserName = "TETACODE",
                Email = "my@tetacode.com",
                NormalizedEmail = "MY@TETACODE.COM",
                FirstName = "Melih",
                LastName = "Yıldırım",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            builder.Entity<AppUser>().HasData(adminUser);

            // 👥 Rol Seed Data
            builder.Entity<AppRole>().HasData(
                new IdentityRole<int> { Id = 1, Name = "User", NormalizedName = "USER" }
            );

            // 📝 NOT SEED DATA - Admin kullanıcısına ait örnek notlar
            builder.Entity<Note>().HasData(
                new Note
                {
                    Id = 1,
                    Title = "ASP.NET Core Giriş",
                    CourseName = "ASP.NET Core",
                    FilePath = "/uploads/aspnet-giris.pdf",
                    FileName = "aspnet-giris.pdf",
                    FileType = "application/pdf",
                    FileSize = "1.2 MB",
                    CreatedDate = DateTime.Now.AddDays(-10),
                    UserId = 1  // adminUser'ın Id'si
                },
                new Note
                {
                    Id = 2,
                    Title = "Entity Framework Core",
                    CourseName = "Veritabanı",
                    FilePath = "/uploads/ef-core.pdf",
                    FileName = "ef-core.pdf",
                    FileType = "application/pdf",
                    FileSize = "2.5 MB",
                    CreatedDate = DateTime.Now.AddDays(-8),
                    UserId = 1
                },
                new Note
                {
                    Id = 3,
                    Title = "React ile Frontend",
                    CourseName = "React",
                    FilePath = "/uploads/react.pdf",
                    FileName = "react.pdf",
                    FileType = "application/pdf",
                    FileSize = "1.8 MB",
                    CreatedDate = DateTime.Now.AddDays(-5),
                    UserId = 1
                },
                new Note
                {
                    Id = 4,
                    Title = "SQL Server Optimizasyon",
                    CourseName = "Veritabanı",
                    FilePath = "/uploads/sql-optimization.pdf",
                    FileName = "sql-optimization.pdf",
                    FileType = "application/pdf",
                    FileSize = "3.1 MB",
                    CreatedDate = DateTime.Now.AddDays(-3),
                    UserId = 1
                }
            );
        }
    }
}
