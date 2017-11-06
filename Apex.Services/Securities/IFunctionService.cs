using Apex.Data.Entities.Securities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apex.Services.Securities
{
    public interface IFunctionService : IService<Function>
    {
        Task<bool> HasSubFunctions(int id);

        Task<IEnumerable<Function>> GetHierarchicalListAsync(bool? enabled = null);
    }
}