using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public class SortJobProcessor : ISortJobProcessor
    {
        private readonly ILogger<SortJobProcessor> _logger;
        
        public SortJobProcessor(ILogger<SortJobProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<SortJob> Process(SortJob job)
        {
            _logger.LogInformation("Processing job with ID '{JobId}'.", job.Id);

            var stopwatch = Stopwatch.StartNew();

            var output = job.Input.OrderBy(n => n).ToArray();
            await Task.Delay(0000); // NOTE: This is just to simulate a more expensive operation

            var duration = stopwatch.Elapsed;

            _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", job.Id, duration);

            return new SortJob(
                id: job.Id,
                status: SortJobStatus.Completed,
                duration: duration,
                input: job.Input,
                output: output);
        }

        public void Process()
        {
            var job = DataStorage.GetDataAll().Where(x => x.Status == SortJobStatus.Pending).FirstOrDefault();
            if (job!=null)
            {
                _logger.LogInformation("Processing job with ID '{JobId}'.", job.Id);

                var stopwatch = Stopwatch.StartNew();

                var output = job.Input.OrderBy(n => n).ToArray();
                //await Task.Delay(5000); // NOTE: This is just to simulate a more expensive operation

                var duration = stopwatch.Elapsed;

                _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", job.Id, duration);

                var updatedJob = new SortJob(
                    id: job.Id,
                    status: SortJobStatus.Completed,
                    duration: duration,
                    input: job.Input,
                    output: output);
                //Remove old item
                DataStorage.GetDataAll().Remove(job);
                //Add updated item
                DataStorage.GetDataAll().Add(updatedJob);
            }
            //return null;
            
        }

        public async Task<SortJob> EnqueueJob(SortJob job)
        {
            await Task.Delay(0);
            DataStorage.SetData(job);
            //jobQueue.Add(job);
            return new SortJob(
                id: job.Id,
                status: SortJobStatus.Pending,
                duration: null,
                input: job.Input,
                output: null) ;
        }

        public async Task<SortJob> GetJob(Guid id)
        {
            await Task.Delay(0);
            var result = DataStorage.GetDataAll().Where(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public async Task<SortJob[]> GetJobs()
        {
            await Task.Delay(0);
            return DataStorage.GetDataAll().ToArray();
        }
    }
}
