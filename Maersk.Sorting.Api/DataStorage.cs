using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public static class DataStorage
    {
        static List<SortJob> jobQueue = new List<SortJob>();
        public static void SetData(SortJob sortJob)
        {
            jobQueue.Add(sortJob);
        }

        public static List<SortJob> GetDataAll()
        {
            return jobQueue;
        }
    }
}
