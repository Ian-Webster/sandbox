using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace TableStorage
{
    public class DataToStore
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class HttpTrigger
    {
        private readonly ILogger<HttpTrigger> _logger;

        public HttpTrigger(ILogger<HttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function("StoreData")]
        [TableOutput("TestTable", Connection = "AzureWebJobsStorage")]
        public async Task<TableEntity> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestData = await req.ReadFromJsonAsync<DataToStore>();

            req.CreateResponse(System.Net.HttpStatusCode.OK);

            return new TableEntity(nameof(DataToStore), requestData.Id.ToString())
            {
                { nameof(DataToStore.Name), requestData.Name },
                { nameof(DataToStore.TimeStamp), requestData.TimeStamp },
                { nameof(DataToStore.Id), requestData.Id }
            };            
        }
    }
}
