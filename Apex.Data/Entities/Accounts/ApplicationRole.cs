using Microsoft.AspNetCore.Identity;

namespace Apex.Data.Entities.Accounts
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }
    }
}
