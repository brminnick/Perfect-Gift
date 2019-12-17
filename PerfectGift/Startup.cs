using System;
using PerfectGift;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

[assembly: FunctionsStartup(typeof(Startup))]
namespace PerfectGift
{
    public class Startup : FunctionsStartup
    {
        readonly static string _visionApiKey = Environment.GetEnvironmentVariable("VisionApiKey") ?? string.Empty;
        readonly static string _visionApiBaseUrl = Environment.GetEnvironmentVariable("VisionApiBaseUrl") ?? string.Empty;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();

            builder.Services.AddSingleton(new ComputerVisionClient(new ApiKeyServiceClientCredentials(_visionApiKey)) { Endpoint = _visionApiBaseUrl });
            builder.Services.AddSingleton<ComputerVisionService>();
        }
    }
}
