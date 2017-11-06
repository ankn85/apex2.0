using System.Collections.Generic;
using System;

namespace Apex.Data.Entities.Logs
{
    public class ActivityLogType : BaseEntity
    {
        public string SystemKeyword { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
