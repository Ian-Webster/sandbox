using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace QueueFunction
{
    public class HttpTriggerFunction
    {

        // output to queue using QueueOutput attribute
        [Function("RunMessages")]
        [QueueOutput("myqueue-items", Connection = "test")]
        public static async Task<List<Message>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("RunMessages");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestData = await req.ReadFromJsonAsync<List<Message>>();

            req.CreateResponse(System.Net.HttpStatusCode.OK);

            return requestData;           
        }

    }
}
