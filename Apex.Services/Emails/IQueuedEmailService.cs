using Apex.Data.Entities.Emails;
using Apex.Data.Paginations;
using System;

namespace Apex.Services.Emails
{
    public interface IQueuedEmailService : IService<QueuedEmail>
    {
        IPagedList<QueuedEmail> GetListAsync(
            DateTime? createdFrom,
            DateTime? createdTo,
            bool loadNotSentItemsOnly,
            bool loadOnlyItemsToBeSent,
            int maxSendTries,
            bool loadNewest,
            int page,
            int size);

        //Task<QueuedEmail> CreateAsync(string email, string subject, string message, EmailAccount emailAccount);

        //Task<int> UpdateAsync(int id, int sentTries, DateTime? sentOnUtc, string failedReason);
    }
}
