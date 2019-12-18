# Perfect Gift

As part of the [Day 18 Challenge](https://25daysofserverless.com/calendar/18) of [#25DaysOfServerless](https://25daysofserverless.com), we are using [Azure Blob Storage](https://azure.microsoft.com/services/storage/blobs?WT.mc_id=25daysofserverless-github-bramin), [Azure Cognitive Services](https://azure.microsoft.com/services/cognitive-services?WT.mc_id=25daysofserverless-github-bramin), and [Azure Functions](https://docs.microsoft.com/azure/azure-functions/?WT.mc_id=25daysofserverless-github-bramin) to ensure that each gift is perfectly wrapped.

Each present must be wrapped according to the following rules

1. Placed in a box
2. Box is wrapped
3. A bow / ribbon placed on top

Using this [example of a perfectly wrapped gift](https://user-images.githubusercontent.com/13558917/70572373-88876980-1b54-11ea-8cd5-af07306b6d19.jpg), the Computer Vision API confirms the following **Tags**:

-   [x] Box
-   [x] Gift Wrapping
-   [x] Ribbon
-   [x] Present

[![Computer Vision Results Example](https://user-images.githubusercontent.com/13558917/70573740-71964680-1b57-11ea-9126-e71f2de14a45.png)](https://azure.microsoft.com/services/cognitive-services/computer-vision?WT.mc_id=25daysofserverless-github-cxa)
