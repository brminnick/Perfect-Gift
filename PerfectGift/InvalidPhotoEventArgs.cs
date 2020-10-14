using System;

namespace PerfectGift
{
    public class InvalidPhotoEventArgs : EventArgs
    {
        public InvalidPhotoEventArgs(bool doesContainAdultContent, bool doesContainRacyContent, bool doesImageContainRequiredPhotoTags, bool invalidAPIKey, bool internetConnectionFailed) =>
            (DoesContainAdultContent, DoesContainRacyContent, DoesImageContainAcceptablePhotoTags, InternetConnectionFailed, InvalidAPIKey) = (doesContainAdultContent, doesContainRacyContent, doesImageContainRequiredPhotoTags, invalidAPIKey, internetConnectionFailed);

        public bool DoesContainRacyContent { get; }
        public bool DoesContainAdultContent { get; }
        public bool DoesImageContainAcceptablePhotoTags { get; }
        public bool InternetConnectionFailed { get; }
        public bool InvalidAPIKey { get; }

        public override string ToString()
        {
            return @$"{nameof(DoesContainRacyContent)} {DoesContainRacyContent}
                        {nameof(DoesContainAdultContent)} {DoesContainAdultContent}
                        {nameof(DoesImageContainAcceptablePhotoTags)} {DoesImageContainAcceptablePhotoTags}
                        {nameof(InternetConnectionFailed)} {InternetConnectionFailed}
                        {nameof(InvalidAPIKey)} {InvalidAPIKey}";
        }
    }
}
