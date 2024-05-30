using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HttpFunction
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [BlobInput("images/test-image.jpg")] Stream blobStream)
        {
            return new FileStreamResult(blobStream, "image/jpeg")
            {
                FileDownloadName = "test-image.jpg"
            };
        }
    }
}
