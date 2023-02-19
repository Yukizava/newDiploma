using Microsoft.AspNetCore.Identity;

namespace NewDiploma.Data.Identity
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
