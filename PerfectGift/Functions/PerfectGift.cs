using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PerfectGift
{
    public class PerfectGiftFunction
    {
        static readonly IReadOnlyList<string> _requiredPhotoTags = new[] { "box", "gift wrapping", "ribbon", "present" };

        readonly ILogger _logger;
        readonly ComputerVisionService _computerVisionService;

        public PerfectGiftFunction(ComputerVisionService computerVisionService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PerfectGiftFunction>();

            _computerVisionService = computerVisionService;
            computerVisionService.InvalidPhotoSubmitted += HandleInvalidPhotoSubmitted;
        }

        [FunctionName(nameof(PerfectGiftFunction))]
        public async Task Run([BlobTrigger("gifts")] CloudBlockBlob giftPhotoBlob, ILogger logger)
        {
            logger.LogInformation("Blob Storage Function Tiggered");

            logger.LogInformation("Opening Image Stream");

            try
            {
                using var giftPhotoStream = await giftPhotoBlob.OpenReadAsync().ConfigureAwait(false);

                var isPhotoValid = await _computerVisionService.IsPhotoValid(giftPhotoStream, _requiredPhotoTags).ConfigureAwait(false);

                if (isPhotoValid)
                {
                    logger.LogInformation("Perfect Gift Confirmed");
                }
                else
                {
                    logger.LogInformation($"Photo {giftPhotoBlob.Name} is not a perfect gift");

                    logger.LogInformation($"Deleting {giftPhotoBlob.Name} from container");

                    await giftPhotoBlob.DeleteAsync().ConfigureAwait(false);

                    logger.LogInformation($"Deleted {giftPhotoBlob.Name}");
                }
            }
            catch (Exception e)
            {
                logger.LogInformation(e.ToString());

                logger.LogInformation($"Deleting {giftPhotoBlob.Name} from container");

                await giftPhotoBlob.DeleteAsync().ConfigureAwait(false);

                logger.LogInformation($"Deleted {giftPhotoBlob.Name}");
            }
        }

        void HandleInvalidPhotoSubmitted(object? sender, InvalidPhotoEventArgs e) => _logger.LogInformation(e.ToString());
    }
}
