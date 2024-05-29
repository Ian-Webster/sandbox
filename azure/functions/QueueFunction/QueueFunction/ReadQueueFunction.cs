using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace QueueFunction
{
    public class ReadQueueFunction
    {
        private readonly ILogger _logger;

        public ReadQueueFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReadQueueFunction>();
        }

        [Function("ReadQueue")]
        public void Run([QueueTrigger("myqueue-items", Connection = "test")] Message myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem.Text}");
        }
    }
}
