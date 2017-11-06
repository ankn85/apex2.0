using Apex.Data;
using Apex.Data.Entities.Blobs;

namespace Apex.Services.Blobs
{
    public sealed class BlobService : BaseService<Blob>, IBlobService
    {
        public BlobService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}