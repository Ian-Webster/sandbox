using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FileResizer
{
    public class SayHelloInput
    {
        public string Name { get; set; } = string.Empty;
        public int TaskDelay { get; set; } = 0;
    }

    public static class SayHelloOrchestratorFunction
    {
        [Function(nameof(SayHelloOrchestratorFunction))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context, List<SayHelloInput> inputs)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(SayHelloOrchestratorFunction));
            logger.LogInformation("Saying hello.");
            var outputs = new List<string>();

            foreach (var input in inputs)
            {
                outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), input));
            }

            // Replace name and input with values relevant for your Durable Functions Activity
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), new SayHelloInput { Name = "Tokyo", TaskDelay = 10000 }));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), new SayHelloInput { Name = "Seattle", TaskDelay = 5000 }));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), new SayHelloInput { Name = "London", TaskDelay = 0 }));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [Function(nameof(SayHello))]
        public static async Task<string> SayHello([ActivityTrigger] SayHelloInput input, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SayHello");
            await Task.Delay(input.TaskDelay);
            logger.LogInformation("Saying hello to {name} after a delay of {delay}.", input.Name, input.TaskDelay);
            return $"Hello {input.Name}!";
        }

        [Function("Function1_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("Function1_HttpStart");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var helloInputs = JsonConvert.DeserializeObject<List<SayHelloInput>>(requestBody);

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(SayHelloOrchestratorFunction), helloInputs);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return await client.CreateCheckStatusResponseAsync(req, instanceId);            
        }
    }
}
