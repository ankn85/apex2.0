using Apex.Core.Caching;
using Apex.Data.Entities.Emails;
using Apex.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Apex.Services.Emails
{
    public sealed class EmailAccountService : BaseService<EmailAccount>, IEmailAccountService
    {
        private const string DefaultEmailAccountKey = "cache.defaultemailaccount";

        private readonly IMemoryCacheService _memoryCacheService;
        
        public EmailAccountService(
            ObjectDbContext dbContext,
            IMemoryCacheService memoryCacheService) : base(dbContext)
        {
            _memoryCacheService = memoryCacheService;
        }

        public async Task<EmailAccount> GetDefaultAsync()
        {
            return await _memoryCacheService.GetSlidingExpiration(
                DefaultEmailAccountKey,
                () =>
                {
                    return QueryNoTracking().FirstOrDefaultAsync(ea => ea.IsDefaultEmailAccount);
                });
        }
    }
}