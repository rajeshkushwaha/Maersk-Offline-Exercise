using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessor
    {
        Task<SortJob> Process(SortJob job);
        void Process();
        Task<SortJob> EnqueueJob(SortJob job);
        Task<SortJob> GetJob(Guid id);
        Task<SortJob[]> GetJobs();
    }
}