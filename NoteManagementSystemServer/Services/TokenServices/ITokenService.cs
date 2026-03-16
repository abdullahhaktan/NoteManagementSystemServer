using NoteManagemenSystemServer.Data.Entities;

namespace NoteManagementSystemServer.Services.TokenServices
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}
