# 📝 NoteManagementSystemServer

[![.NET](https://img.shields.io/badge/.NET-8.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF_Core-Code_First-blue)](https://learn.microsoft.com/en-us/ef/core/)
[![JWT](https://img.shields.io/badge/Auth-JWT_Bearer-orange)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Docs-Swagger_UI-85EA2D?logo=swagger)](https://swagger.io/)
[![Identity](https://img.shields.io/badge/Identity-ASP.NET_Core-blueviolet)](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)

**NoteManagementSystemServer**, kullanıcıların ders notlarını yönetebileceği, dosya yükleyebileceği ve arşivleyebileceği bir RESTful API'dir. ASP.NET Core 8, Entity Framework Core ve JWT kimlik doğrulama kullanılarak geliştirilmiştir.

> 🖥️ **Frontend:** [NoteManagementSystemClient](https://github.com/abdullahhaktan/NoteManagementSystemClient) — Angular 17

---

## ✨ Özellikler

- 🔐 **JWT Kimlik Doğrulama** — Token tabanlı güvenli giriş/çıkış, HmacSha512 imzalama
- 📋 **Not Yönetimi** — Not oluşturma, düzenleme, listeleme ve soft-delete
- 📎 **Dosya Yükleme** — Sunucuya dosya yükleme, GUID tabanlı benzersiz isimlendirme
- 🗃️ **Soft Delete & Arşiv** — Silinen notlar `DeletedAt` alanıyla arşivlenir, kalıcı silme ayrıca yapılır
- 🛡️ **Global Authentication** — Tüm endpoint'ler varsayılan olarak korumalı, sadece `[AllowAnonymous]` olanlar açık
- 🔄 **CORS** — Angular frontend ile güvenli iletişim
- 📄 **Swagger UI** — Bearer token desteğiyle API dokümantasyonu

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Açıklama |
| :--- | :--- |
| **ASP.NET Core 8** | Web API framework |
| **Entity Framework Core** | Code First ORM |
| **ASP.NET Core Identity** | Kullanıcı ve rol yönetimi |
| **JWT Bearer** | Token tabanlı kimlik doğrulama |
| **Mapster** | DTO mapping (MaxDepth=2 ile döngüsel referans koruması) |
| **MS SQL Server** | Veritabanı |
| **Swagger / Swashbuckle** | API dokümantasyonu |

---

## 📁 Proje Yapısı

```
NoteManagementSystemServer/
├── Context/
│   └── NoteManagementContext.cs       # DbContext, Identity, Seed Data
├── Controllers/
│   ├── LoginController.cs             # POST /api/login — [AllowAnonymous]
│   ├── NotesController.cs             # CRUD /api/notes
│   └── NoteArchivesController.cs      # GET, DELETE /api/notearchives
├── Data/
│   ├── DTOs/
│   │   ├── NoteDtos/                  # CreateNoteDto, UpdateNoteDto, ResultNoteDto, GetNoteByIdDto
│   │   └── UserDtos/                  # LoginDto, UserDto
│   └── Entities/
│       ├── AppUser.cs                 # Identity kullanıcısı
│       ├── AppRole.cs                 # Identity rolü
│       └── Note.cs                    # Not entity
├── Services/
│   ├── NoteServices/
│   │   ├── INoteService.cs
│   │   └── NoteService.cs             # CRUD + dosya yönetimi
│   ├── NoteArchiveServices/
│   │   ├── INoteArchiveService.cs
│   │   └── NoteArchiveService.cs      # Arşiv listeleme + kalıcı silme
│   └── TokenServices/
│       ├── ITokenService.cs
│       └── TokenService.cs            # JWT token üretimi
├── Uploads/                           # Yüklenen dosyalar (git'e eklenmez)
└── Program.cs                         # Uygulama yapılandırması
```

---

## ⚙️ Teknik Mimari

| Özellik | Açıklama |
| :--- | :--- |
| **Mimari** | Service Layer + Repository Pattern (EF Core) |
| **Veri Yönetimi** | Entity Framework Core — Code First |
| **Kimlik Doğrulama** | JWT Bearer — HmacSha512, `ClockSkew = Zero` |
| **Yetkilendirme** | Global `AuthorizeFilter` — tüm controller'lar korumalı |
| **DTO Mapping** | Mapster — `MaxDepth(2)` ile sonsuz döngü koruması |
| **Dosya Yükleme** | `[FromForm]` + `IFormFile` — GUID isimli fiziksel depolama |
| **Soft Delete** | `DeletedAt` alanı — arşiv ve aktif notlar bu alanla ayrılır |
| **JSON** | `ReferenceHandler.IgnoreCycles` — döngüsel referans engeli |

---

## 🔌 API Endpoints

### Auth
| Method | Endpoint | Açıklama | Auth |
| :--- | :--- | :--- | :--- |
| POST | `/api/login` | Kullanıcı girişi, JWT token döner | ❌ |

### Notes
| Method | Endpoint | Açıklama | Auth |
| :--- | :--- | :--- | :--- |
| GET | `/api/notes` | Kullanıcının aktif notları | ✅ |
| GET | `/api/notes/GetNoteById/{id}` | Nota göre detay | ✅ |
| POST | `/api/notes` | Yeni not oluştur `[FromForm]` | ✅ |
| PUT | `/api/notes` | Notu güncelle `[FromForm]` | ✅ |
| DELETE | `/api/notes?id={id}` | Notu arşive taşı (soft delete) | ✅ |

### Archive
| Method | Endpoint | Açıklama | Auth |
| :--- | :--- | :--- | :--- |
| GET | `/api/notearchives` | Arşivlenen notları listele | ✅ |
| DELETE | `/api/notearchives?id={id}` | Notu kalıcı sil (dosyayla birlikte) | ✅ |

---

## 🚀 Kurulum

**1. Repoyu klonlayın:**
```bash
git clone https://github.com/abdullahhaktan/NoteManagementSystemServer.git
cd NoteManagementSystemServer
```

**2. `appsettings.json` dosyasını düzenleyin:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=NoteManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "buraya-en-az-32-karakterlik-gizli-anahtar",
    "Issuer": "NoteManagementSystemServer",
    "Audience": "NoteManagementSystemClient",
    "ExpireInMinutes": "60"
  }
}
```

**3. Veritabanını oluşturun:**
```bash
dotnet ef database update
```

**4. Projeyi çalıştırın:**
```bash
dotnet run
```

Swagger UI: `https://localhost:{PORT}/swagger`

---

## 🔐 Seed Data

Proje ilk çalıştırıldığında aşağıdaki veriler otomatik oluşturulur:

| Alan | Değer |
| :--- | :--- |
| **Kullanıcı Adı** | `tetacode` |
| **Şifre** | `Admin123!` |
| **E-posta** | `my@tetacode.com` |
| **Rol** | `User` |

> ⚠️ Üretim ortamında seed data şifresini mutlaka değiştirin.

---

## 📸 Proje Görselleri

> _(Ekran görüntüleri eklenecek)_

---
