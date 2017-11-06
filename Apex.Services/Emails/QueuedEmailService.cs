using Apex.Core.Extensions;
using Apex.Data.Entities.Emails;
using Apex.Data.Paginations;
using Apex.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Apex.Services.Emails
{
    public sealed class QueuedEmailService : BaseService<QueuedEmail>, IQueuedEmailService
    {
        public QueuedEmailService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public IPagedList<QueuedEmail> GetListAsync(
            DateTime? createdFrom,
            DateTime? createdTo,
            bool loadNotSentItemsOnly,
            bool loadOnlyItemsToBeSent,
            int maxSendTries,
            bool loadNewest,
            int page,
            int size)
        {
            var query = _dbSet
                .Include(qe => qe.EmailAccount)
                .AsNoTracking();

            if (createdFrom.HasValue && createdTo.HasValue)
            {
                var startDate = createdFrom.Value.StartOfDay();
                var endDate = createdTo.Value.EndOfDay();

                query = query.Where(qe => startDate <= qe.CreatedOn && qe.CreatedOn <= endDate);
            }

            if (loadNotSentItemsOnly)
            {
                query = query.Where(qe => !qe.SentOn.HasValue);
            }

            if (loadOnlyItemsToBeSent)
            {
                DateTime nowUtc = DateTime.UtcNow;

                query = query.Where(qe => !qe.DontSendBeforeDate.HasValue || qe.DontSendBeforeDate.Value <= nowUtc);
            }

            query = query.Where(qe => qe.SentTries < maxSendTries);

            query = loadNewest ?
                query.OrderByDescending(qe => qe.CreatedOn) :
                query.OrderByDescending(qe => qe.Priority).ThenBy(qe => qe.CreatedOn);

            return GetPagedList(query, page, size);
        }

        // public async Task<QueuedEmail> CreateAsync(string email, string subject, string message, EmailAccount emailAccount)
        // {
        //     QueuedEmail queuedEmail = new QueuedEmail
        //     {
        //         From = emailAccount.Email,
        //         FromName = emailAccount.DisplayName,
        //         To = email,
        //         Subject = subject,
        //         Body = message,
        //         CreatedOn = DateTime.UtcNow,
        //         Priority = (int)QueuedEmailPriority.High,
        //         EmailAccountId = emailAccount.Id
        //     };

        //     return await CreateAsync(queuedEmail);
        // }

        // public async Task<int> UpdateAsync(int id, int sentTries, DateTime? sentOn, string failedReason)
        // {
        //     QueuedEmail updatedEntity = await FindAsync(id);

        //     if (updatedEntity == null)
        //     {
        //         throw new ApiException($"{nameof(QueuedEmailService)} » {nameof(UpdateAsync)}] Queued Email not found. Id = {id}");
        //     }

        //     updatedEntity.SentTries = sentTries;
        //     updatedEntity.SentOn = sentOn;
        //     updatedEntity.FailedReason = failedReason;

        //     return await CommitAsync();
        // }
    }
}
