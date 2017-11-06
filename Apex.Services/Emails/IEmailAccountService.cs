using Apex.Data.Entities.Emails;
using System.Threading.Tasks;

namespace Apex.Services.Emails
{
    public interface IEmailAccountService : IService<EmailAccount>
    {
        Task<EmailAccount> GetDefaultAsync();
    }
}