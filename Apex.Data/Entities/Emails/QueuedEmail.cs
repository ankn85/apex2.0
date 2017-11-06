using System;

namespace Apex.Data.Entities.Emails
{
    public class QueuedEmail : BaseEntity
    {
        public int Priority { get; set; }

        public string From { get; set; }

        public string FromName { get; set; }

        public string To { get; set; }

        public string ToName { get; set; }

        public string ReplyTo { get; set; }

        public string ReplyToName { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DontSendBeforeDate { get; set; }

        public int SentTries { get; set; }

        public DateTime? SentOn { get; set; }

        public bool SendManually { get; set; }

        public string FailedReason { get; set; }

        public int EmailAccountId { get; set; }

        public virtual EmailAccount EmailAccount { get; set; }
    }
}
