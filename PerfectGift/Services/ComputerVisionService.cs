using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Linq;
using System.Net.Http;
using System.Net;
using AsyncAwaitBestPractices;
using Microsoft.Extensions.Logging;

namespace PerfectGift
{
    public class ComputerVisionService
    {
        readonly WeakEventManager<InvalidPhotoEventArgs> _invalidPhotoSubmittedEventManager = new WeakEventManager<InvalidPhotoEventArgs>();
        readonly ILogger _logger;
        readonly ComputerVisionClient _computerVisionApiClient;

        public ComputerVisionService(ILoggerFactory loggerFactory, ComputerVisionClient computerVisionClient) =>
            (_logger, _computerVisionApiClient) = (loggerFactory.CreateLogger<ComputerVisionService>(), computerVisionClient);

        public event EventHandler<InvalidPhotoEventArgs> InvalidPhotoSubmitted
        {
            add => _invalidPhotoSubmittedEventManager.AddEventHandler(value);
            remove => _invalidPhotoSubmittedEventManager.RemoveEventHandler(value);
        }

        public async Task<bool> IsPhotoValid(Stream photo, IReadOnlyCollection<string> requiredPhotoTags, bool shouldAllowAdultContent = false, bool shouldAllowRacyContent = false)
        {
            bool isInvalidAPIKey = false;
            bool hasInternetConnectionFailed = false;

            ImageAnalysis? imageAnalysisResult;

            _logger.LogInformation("Generating Image Analysis");

            try
            {
                imageAnalysisResult = await _computerVisionApiClient.AnalyzeImageInStreamAsync(photo, new List<VisualFeatureTypes> { VisualFeatureTypes.Adult, VisualFeatureTypes.Tags }).ConfigureAwait(false);
            }
            catch (HttpRequestException e) when (e.InnerException is WebException webException
                                                    && (webException.Status is WebExceptionStatus.NameResolutionFailure || webException.Status is WebExceptionStatus.ConnectFailure))
            {
                _logger.LogInformation(e.ToString());

                imageAnalysisResult = null;
                hasInternetConnectionFailed = true;
            }
            catch (ComputerVisionErrorException e) when (e.Response.StatusCode is HttpStatusCode.Unauthorized)
            {
                _logger.LogInformation(e.ToString());

                imageAnalysisResult = null;
                isInvalidAPIKey = true;
            }

            var doesContainAdultContent = imageAnalysisResult?.Adult?.IsAdultContent ?? false;
            var doesContainRacyContent = imageAnalysisResult?.Adult?.IsRacyContent ?? false;

            var imageTags = imageAnalysisResult?.Tags?.Select(x => x.Name) ?? Enumerable.Empty<string>();
            var matchingTagsCount = imageTags.Intersect(requiredPhotoTags.Distinct()).Count();

            bool doesImageContainAllPhotoTags = matchingTagsCount == requiredPhotoTags.Count;

            if (isInvalidAPIKey
                || (doesContainRacyContent && !shouldAllowRacyContent)
                || (doesContainAdultContent && !shouldAllowAdultContent)
                || !doesImageContainAllPhotoTags
                || hasInternetConnectionFailed)
            {
                OnDisplayInvalidPhotoAlert(doesContainAdultContent, doesContainRacyContent, doesImageContainAllPhotoTags, isInvalidAPIKey, hasInternetConnectionFailed);
                return false;
            }

            return true;
        }

        void OnDisplayInvalidPhotoAlert(bool doesContainAdultContent, bool doesContainRacyContent, bool doesImageContainAcceptablePhotoTags, bool invalidAPIKey, bool internetConnectionFailed) =>
            _invalidPhotoSubmittedEventManager.RaiseEvent(this, new InvalidPhotoEventArgs(doesContainAdultContent, doesContainRacyContent, doesImageContainAcceptablePhotoTags, invalidAPIKey, internetConnectionFailed), nameof(InvalidPhotoSubmitted));
    }
}
