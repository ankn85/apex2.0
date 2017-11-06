using Apex.Data.Entities.Logs;
using System.Collections.Generic;

namespace Apex.Services.Logs
{
    public interface IActivityLogTypeService : IService<ActivityLogType>
    {
       IDictionary<string, int> GetEnabledIdList();
    }
}
