# 📝 NoteManagementSystemClient

[![Angular](https://img.shields.io/badge/Angular-17-dd0031?logo=angular)](https://angular.io/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178c6?logo=typescript)](https://www.typescriptlang.org/)
[![JWT](https://img.shields.io/badge/Auth-JWT_Bearer-orange)](https://jwt.io/)
[![Standalone](https://img.shields.io/badge/Architecture-Standalone_Components-blueviolet)](https://angular.io/guide/standalone-components)

**NoteManagementSystemClient**, [NoteManagementSystemServer](https://github.com/abdullahhaktan/NoteManagementSystemServer) ile çalışmak üzere geliştirilmiş Angular tabanlı frontend uygulamasıdır. Kullanıcılar ders notlarını yönetebilir, dosya yükleyebilir ve arşivleyebilir.

---

## ✨ Özellikler

- 🔐 **JWT Kimlik Doğrulama** — Token tabanlı güvenli giriş/çıkış
- 📋 **Not Yönetimi** — Not oluşturma, düzenleme, silme ve listeleme
- 📎 **Dosya Yükleme** — PDF, PNG, JPG, DOCX formatlarında dosya ekleme
- 🗃️ **Arşiv** — Silinen notları arşivde görüntüleme ve kalıcı silme
- 🔒 **Route Guard** — Kimlik doğrulanmamış kullanıcıları login'e yönlendirme
- 🌙 **Dark Mode** — Modern koyu tema tasarım

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Açıklama |
| :--- | :--- |
| **Angular 17** | Standalone Component mimarisi |
| **TypeScript** | Tip güvenli geliştirme |
| **Reactive Forms** | Form yönetimi ve validasyon |
| **HTTP Interceptor** | Her isteğe otomatik JWT token ekleme |
| **Route Guards** | Sayfa erişim kontrolü |
| **Lazy Loading** | Performanslı component yükleme |

---

## 📁 Proje Yapısı

```
src/
├── app/
│   ├── core/
│   │   ├── guards/
│   │   │   └── auth.guard.ts          # Route koruma
│   │   ├── interceptors/
│   │   │   └── jwt.interceptor.ts     # Bearer token ekleme
│   │   └── services/
│   │       └── auth.service.ts        # Login/logout/token yönetimi
│   ├── features/
│   │   ├── auth/
│   │   │   └── login/                 # Giriş sayfası
│   │   └── notes/
│   │       ├── note-list/             # Not listesi
│   │       ├── note-form/             # Not oluşturma/düzenleme
│   │       ├── note-detail/           # Not detayı
│   │       └── note-archive/          # Arşiv sayfası
│   ├── shared/
│   │   ├── models/
│   │   │   ├── note.model.ts          # Note DTO modelleri
│   │   │   └── user.model.ts          # User DTO modelleri
│   │   └── services/
│   │       ├── note.service.ts        # Note CRUD işlemleri
│   │       └── archive.service.ts     # Arşiv işlemleri
│   ├── app.component.ts
│   ├── app.config.ts
│   └── app.routes.ts
└── environments/
    ├── environment.ts                 # API URL (development)
    └── environment.prod.ts            # API URL (production)
```

---

## ⚙️ Teknik Mimari

| Özellik | Açıklama |
| :--- | :--- |
| **Mimari** | Standalone Component (NgModule'siz) |
| **HTTP** | `provideHttpClient` + Functional Interceptor |
| **Routing** | Lazy Loading ile `loadComponent` |
| **Auth** | JWT Bearer Token — `localStorage` yönetimi |
| **Form** | Reactive Forms + Validasyon |
| **API İletişim** | `multipart/form-data` (dosya yükleme için) |

---

## 🚀 Kurulum

> ⚠️ Bu projenin çalışması için [NoteManagementSystemServer](https://github.com/abdullahhaktan/NoteManagementSystemServer) API'sinin ayakta olması gerekir.

**1. Repoyu klonlayın:**
```bash
git clone https://github.com/abdullahhaktan/NoteManagementSystemClient.git
cd NoteManagementSystemClient
```

**2. Bağımlılıkları yükleyin:**
```bash
npm install
```

**3. API adresini ayarlayın:**

`src/environments/environment.ts` dosyasını açın:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:XXXX/api'  // .NET API port numaranızı girin
};
```

**4. Uygulamayı başlatın:**
```bash
ng serve
```

Uygulama `http://localhost:4200` adresinde çalışacaktır.

---

## 🔗 İlgili Repo

- **Backend API:** [NoteManagementSystemServer](https://github.com/abdullahhaktan/NoteManagementSystemServer) — ASP.NET Core 8, JWT, Entity Framework Core

---

## 📸 Proje Görselleri

> _(Ekran görüntüleri eklenecek)_

---
