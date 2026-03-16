using Microsoft.AspNetCore.Identity;

namespace NoteManagemenSystemServer.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IList<Note> Notes { get; set; }
    }
}
