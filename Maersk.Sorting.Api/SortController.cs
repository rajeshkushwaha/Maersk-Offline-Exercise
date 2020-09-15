using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    public class SortController : ControllerBase
    {
        private readonly ISortJobProcessor _sortJobProcessor;
        private readonly ILogger<SortController> _logger;

        public SortController(ISortJobProcessor sortJobProcessor,
            ILogger<SortController> logger)
        {
            _sortJobProcessor = sortJobProcessor;
            _logger = logger;
        }

        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }

        [HttpPost]
        public async Task<ActionResult<SortJob>> EnqueueJob(int[] values)
        {
            // TODO: Should enqueue a job to be processed in the background.
            var pendingJob =  new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var result =  await _sortJobProcessor.EnqueueJob(pendingJob);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<SortJob[]>> GetJobs()
        {
            // TODO: Should return all jobs that have been enqueued (both pending and completed).
            var result = await _sortJobProcessor.GetJobs();
            return Ok(result);
        }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<SortJob>> GetJob(Guid jobId)
        {
            // TODO: Should return a specific job by ID.
            var result =await _sortJobProcessor.GetJob(jobId);
            _logger.LogInformation($"data id:{result.Id}");
            return Ok(result);
        }
    }
}
