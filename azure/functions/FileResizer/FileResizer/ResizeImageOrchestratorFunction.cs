using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FileResizer
{
    public class ResizeImageInput
    {       
        public int Height { get; set; }

        public int Width { get; set; }

        public List<string> Files { get; set; } = new List<string>();
    }

    public class FileForResize
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public string FileContent { get; set; } = string.Empty;
    }

    public class ResizedFile
    {
        public string FileName { get; set; } = string.Empty;

        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }

    public class ResizeResult
    {
        public string FileName { get; set; } = string.Empty;

        public string FileUri { get; set; } = string.Empty;
    }

    public static class ResizeImageOrchestratorFunction
    {
        [Function(nameof(ResizeImageOrchestratorFunction))]
        public static async Task<List<ResizeResult>?> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context, ResizeImageInput resizeData)
        {
            var logger = context.CreateReplaySafeLogger(nameof(ResizeImageOrchestratorFunction));
            logger.LogInformation("File resizer orchestrator started");
            
            var result = new List<ResizeResult>();

            foreach (var file in resizeData.Files)
            {
                // Resize image
                var resizedByteArray = await context.CallActivityAsync<byte[]?>(nameof(ResizeFile), new FileForResize
                {
                    Height = resizeData.Height,
                    Width = resizeData.Width,
                    FileContent = file
                });
                if (resizedByteArray == null || resizedByteArray.Length == 0)
                {
                    logger.LogError("Failed to resize image");
                    continue;
                }
                var resizedFile = new ResizedFile
                {
                    FileName = $"{context.NewGuid()}.jpg",
                    FileContent = resizedByteArray
                };

                // Save resized image
                var resizedFileUri = await context.CallActivityAsync<string>(nameof(SaveFile), resizedFile);

                result.Add(new ResizeResult
                {
                    FileName = resizedFile.FileName,
                    FileUri = resizedFileUri
                });
            }

            return result;

        }

        [Function(nameof(ResizeFile))]
        public static byte[]? ResizeFile([ActivityTrigger] FileForResize resizeData, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(ResizeFile));
            logger.LogInformation("Resizing file");

            // Convert base64 string to byte array
            var imageBytes = Convert.FromBase64String(resizeData.FileContent);

            // Resize image
            using (var image = Image.Load(imageBytes))
            {
                // detect image format
                var format = image.Metadata.DecodedImageFormat;
                if (format == null || !format.Equals(JpegFormat.Instance))
                {
                    logger.LogError("Unsupported image format");
                    return null;
                }

                logger.LogInformation("resizing file to width: {width} and height: {height}", resizeData.Width, resizeData.Height);
                image.Mutate(x => x.Resize(new ResizeOptions 
                    { 
                        Size = new Size(resizeData.Width, resizeData.Height),
                        Mode = ResizeMode.Max
                    })
                );

                using (var output = new MemoryStream())
                {
                    logger.LogInformation("saving image to memory stream");
                    image.SaveAsJpeg(output);
                    return output.ToArray();                        
                }
            }
        }

        [Function(nameof(SaveFile))]
        public static async Task<string> SaveFile([ActivityTrigger] ResizedFile resizedFile, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(SaveFile));
            logger.LogInformation("saving file");

            var blobServiceClient = new BlobServiceClient("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;");
            var containerClient = blobServiceClient.GetBlobContainerClient("resized-images");

            var path = $"./data/{resizedFile.FileName}";

            var blobClient = containerClient.GetBlobClient(resizedFile.FileName);

            using (var stream = new MemoryStream(resizedFile.FileContent, writable: false))
            {
                logger.LogInformation("Uploading file to blob storage");
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.AbsoluteUri;
        }

        [Function("ResizeImageOrchestratorFunction_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ResizeImageOrchestratorFunction_HttpStart");

            var requestData = await req.ReadFromJsonAsync<ResizeImageInput>();

            if (requestData == null)
            {
                var response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await response.WriteStringAsync("Request body is empty");
                return response;
            }

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(ResizeImageOrchestratorFunction), requestData);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
