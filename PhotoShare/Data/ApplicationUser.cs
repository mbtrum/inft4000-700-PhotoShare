
using Microsoft.AspNetCore.Identity;

namespace PhotoShare.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData] // property is included in download of personal data.
        public string Name { get; set; } = string.Empty;

        [PersonalData]
        public string Bio { get; set; } = string.Empty;

        [PersonalData]
        public string Location { get; set; } = string.Empty;

        [PersonalData]
        public bool IsForHire { get; set; } = false;
    }
}
