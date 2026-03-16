using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteManagemenSystemServer.Context;
using NoteManagemenSystemServer.Data.DTOs.NoteDtos;
using NoteManagemenSystemServer.Data.Entities;
using NoteManagemenSystemServer.Services.NoteArhiveServices;
using NoteManagemenSystemServer.Services.NoteServices;
using NoteManagementSystemServer.Services.TokenServices;
using System.Text;
using System.Text.Json.Serialization;

// create a folder called Uploads in the project root
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadPath))
    Directory.CreateDirectory(uploadPath);

var builder = WebApplication.CreateBuilder(args);


// map Note entity to ResultNoteDto so we don't return the full entity
TypeAdapterConfig<Note, ResultNoteDto>.NewConfig()
    .MaxDepth(2);

// **ÖNEMLÝ: CORS ayarlarýný düzeltelim**
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUi", policy =>
    {
        policy.WithOrigins("https://localhost:7156","http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Cookie gönderimine izin ver
    });
});

// register services to dependency injectioni
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteArchiveService, NoteArchiveService>();

builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<NoteManagementContext>();

// Kimlik doðrulama (Authentication) sistemini JWT þemasýyla çalýþacak þekilde yapýlandýrýyoruz.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        // Gelen Token'ýn geçerli olup olmadýðýný hangi kriterlere göre kontrol edeceðiz?
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // 1. Ýmza Kontrolü: Token bizim gizli anahtarýmýzla (Key) mi imzalanmýþ?
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            // 2. Yayýncý Kontrolü: Bu token'ý bizim API (Issuer) mý üretmiþ?
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // 3. Hedef Kitle Kontrolü: Bu token bizim Uygulamamýz (Audience) için mi üretilmiþ?
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // 4. Zaman Kontrolü: Token'ýn süresi (Expire Date) dolmuþ mu?
            ValidateLifetime = true,

            // 5. Hassasiyet: Sunucu saati ile dünya saati arasýndaki 5 dk'lýk esneklik payýný sýfýrlýyoruz.
            // Süre bittiði saniyede token geçersiz olsun diye.
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddDistributedMemoryCache();

// configure EF Core with SQL Server
builder.Services.AddDbContext<NoteManagementContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString);
});

// Global Authentication
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
})
// Sonsuz döngüyü engellemek için
.AddJsonOptions(config =>
{
    config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MusicApp API", Version = "v1" });

    // 1. Swagger'a "Bearer" þemasýný tanýmlýyoruz
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Lütfen baþýna 'Bearer ' yazarak tokený yapýþtýrýn. Örn: 'Bearer abc123...'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // 2. Tüm API metodlarýna bu güvenlik gereksinimini ekliyoruz
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// **ÖNEMLÝ: Authentication her þeyden önce gelmeli**

app.UseCors("AllowUi");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();