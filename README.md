[![](https://res.cloudinary.com/jen-looper/image/upload/v1576271295/images/challenge-18_hraoqx.jpg)](https://25daysofserverless.com/calendar/18)


The challenge on [Day 18](https://25daysofserverless.com/calendar/18) of [#25DaysOfServerless](https://25daysofserverless.com) is to ensure that each gift is perfectly wrapped according to the following rules

1. Placed in a box
2. Box is wrapped
3. A bow / ribbon placed on top

Let's accomplish this using [Azure Blob Storage](https://azure.microsoft.com/services/storage/blobs?WT.mc_id=25daysofserverless-github-bramin), [Azure Computer Vision API](https://azure.microsoft.com/services/cognitive-services/computer-vision?WT.mc_id=25daysofserverless-github-bramin), and [Azure Functions](https://docs.microsoft.com/azure/azure-functions/?WT.mc_id=25daysofserverless-github-bramin).

## Example

Using this [example of a perfectly wrapped gift](https://user-images.githubusercontent.com/13558917/70572373-88876980-1b54-11ea-8cd5-af07306b6d19.jpg), the Computer Vision API confirms the following **Tags**:

- Box
- Gift Wrapping
- Ribbon
- Present

[![Computer Vision Results Example](https://user-images.githubusercontent.com/13558917/70573740-71964680-1b57-11ea-9126-e71f2de14a45.png)](https://azure.microsoft.com/services/cognitive-services/computer-vision?WT.mc_id=25daysofserverless-github-bramin)

## Creating the Solution

### Step 0: Install Tools

In this step, we will install the necessary commandline tools in order to complete the solution.

1 - Install .NET Core v3.1

- In a browser, navigate to the [Download .NET Core Website](https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=25daysofserverless-github-bramin)
- On the **Download .NET Core Website**, install .NET Core 3.1

2 - Install the [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest&WT.mc_id=25daysofserverless-github-bramin)

  - (Windows) [Download the MSI Installer](https://aka.ms/installazurecliwindows)

  - (macOS) In the terminal, enter the following command:

```bash
brew update && brew install azure-cli
```

 3 - Install [Azure Functions Core Tools v3.x](https://docs.microsoft.com/azure/azure-functions/functions-run-local?WT.mc_id=25daysofserverless-github-bramin#v3)

  - (Windows) In the terminal, enter the following command:

```bash
npm install -g azure-functions-core-tools@3
```

  - (macOS) In the terminal, enter the following command:

```bash
brew tap azure/functions
brew install azure-functions-core-tools@3
# if upgrading on a machine that has 2.x installed
brew link --overwrite azure-functions-core-tools@3
```

4 - Install Git Command Line Tools

  - In a browser, navigate to the [Git Downloads Page](https://git-scm.com/downloads)
  - On the **Git Downloads Page**, install git for your specific operating system

### Step 1: Downloading the Solution Repo

In this step, we will fork and clone the solution repo to our local machine.

0 - Star the [Solution Repo](https://github.com/brminnick/Perfect-Gift)
> **Note** Starring the repo will help it become more discoverable, helping more devs find the solution

  - In a browser, navigate to the [Perfect-Gift repo](https://github.com/brminnick/Perfect-Gift)
  - In the browser, tt the top of the page, click **Star**
![GitHub Star](https://user-images.githubusercontent.com/13558917/71127002-02e06b00-219f-11ea-9c10-347049d4fcf7.png)

1 - Fork the [Solution Repo](https://github.com/brminnick/Perfect-Gift)
  
  - In a browser, navigate to the [Perfect-Gift repo](https://github.com/brminnick/Perfect-Gift)
  - In the browser, tt the top of the page, click **Star**
![GitHub Fork](https://user-images.githubusercontent.com/13558917/71126991-fa883000-219e-11ea-9da5-26a6f893b439.png)

2 - Clone the newly Forked Repo

  - In the terminal, enter the following command:

```bash
git clone https://github.com/[your github user name]/Perfect-Gift
```
> **Note** Replace `[your github user name]` with your [GitHub User name](https://stackoverflow.com/a/19077217/5953643)

### Step 2: Log into Azure CLI

1 - In the terminal, enter the following command to login into Azure CLI:

```bash
az login
```

> **Note:** Stand by until the Azure CLI opens your browser to the Azure Login page

### Step 3: Create Azure Resources

In this step, we'll generate the following Azure Resources:

- Azure Resource Group
  - This is a folder in azure that will hold our resources
- [Azure Blob Storage](https://azure.microsoft.com/services/storage/blobs?WT.mc_id=25daysofserverless-github-bramin)
  - This is where we'll upload and store the images of our gifts
- [Computer Vision API](https://azure.microsoft.com/services/cognitive-services/computer-vision?WT.mc_id=25daysofserverless-github-bramin) Key
  - This API will use machine learning to confirm that our gift has been perfectly wrapped
- [Azure Functions](https://docs.microsoft.com/azure/azure-functions/?WT.mc_id=25daysofserverless-github-bramin)
  - This server less function will run each time a new photo is uploaded to Azure Blob Storage to confirm that the image contains a perfectly wrapped gift

1 - In the terminal, enter the following command to create an Azure Resource Group

```bash
az group create --name PerfectGift --location EastUS
```

2 - In the terminal, enter the following command to create a free Computer Vision resource

```bash
az cognitiveservices account create --resource-group PerfectGift --name PerfectGiftComputerVision --sku F0 --kind ComputerVision --location EastUS
```

3 - In the terminal, in the JSON response, note the value of **endpoint**

> **Note:** For the EastUS, the endpoint should be `https://eastus.api.cognitive.microsoft.com/?WT.mc_id=perfectgift-github-bramin`. We will use this value later in our serverless function.

4 - In the terminal, enter the following command to retrieve the newly generated Computer Vision API Key

```bash
az cognitiveservices account keys list --resource-group PerfectGift --name PerfectGiftComputerVision
```

5 - In the terminal, in the JSON response, note the value of **key1**

> **Note:** The JSON response will provide two keys. Both keys are valid, and we'll be using **key1** for our serverless function.
> ```json
> {
>   "key1": "[YOUR API KEY]",
>   "key2": "[YOUR API KEY]"
> }
> ```

6 - In the terminal, enter the following command to create an Azure Storage account:

```bash
az storage account create --name giftstorage[YOUR NAME] --location EastUS --resource-group PerfectGift --sku Standard_LRS
```

> **Note:** Replace `[Your Name]` with your name to ensure the storage account name is unique, e.g. `giftstoragebrandon`

7 - In the terminal, enter the following command retrive the Azure Storage Connection String

```bash
az storage account show-connection-string --name giftstorage[YOUR NAME]
```

> **Note:** Replace `[Your Name]` with your name

8 - In the terminal, in the JSON response, copy the value of **connectionString**
> **Note:** We will use **connectionString** in the next step to create a new storage container

9 - In the terminal, enter the following command to create container called `gifts` in our Azure Storage account:

```bash
az storage container create --name gifts --connection-string "[YOUR CONNECTION STRING]"
```

> **Note:** Replace `[YOUR CONNECTION STRING]` with the vaulue of **connectionString** retreived in the previous step, e.g. az `storage container create --name gifts --connection-string "abc123def456ghi789=="`

10 - In the terminal, enter the following command to create an Azure Function App:

```bash
az functionapp create --resource-group PerfectGift --consumption-plan-location EastUS --name PerfectGift-[Your Name] --storage-account  giftstorage[YOUR NAME] --runtime dotnet
```

> **Note:** Replace `[Your Name]` with your name to ensure the function app name is unique, e.g. `PerfectGift-Brandon`

11 - In the terminal, enter the following to set the Azure Functions Runtime to v3:

```bash
az functionapp config appsettings set --resource-group PerfectGift --name PerfectGift-Brandon --settings "FUNCTIONS_EXTENSION_VERSION=~3"
```

12 - In the terminal, enter the following to add the Computer Vision API **key** and **endpoint** to the newly created Azure Function App:

```bash
az functionapp config appsettings set --resource-group PerfectGift --name PerfectGift-[YOUR NAME] --settings "VisionApiKey=[YOUR API KEY]" "VisionApiBaseUrl=[YOUR COMPUTER VISION ENDPOINT]"
```

> **Note:** Replace `[YOUR NAME]` with your name, replace `[YOUR API KEY]` with the value of **key1** and replace `[YOUR COMPUTER VISION ENDPOINT]` with the value of **endpoint**
> e.g. `az functionapp config appsettings set --resource-group PerfectGift --name PerfectGift-Brandon --settings "VisionApiKey=abc123" "VisionApiBaseUrl=https://eastus.api.cognitive.microsoft.com/?WT.mc_id=perfectgift-github-bramin"`

### Step 4: Publish Azure Function

In this step, we will publish the solution found in `PerfectGift.csproj` to Azure.

1 - In the terminal, enter the following command to navigate to the folder containing `PerfectGift.csproj` in the cloned solution repo

  - (Windows)

```bash
cd [Path to cloned solution repo]\Perfect-Gift\PerfectGift\
```

  - (macOS)

```bash
cd [Path to cloned solution repo]/Perfect-Gift/PerfectGift
```

2 - In the terminal, enter the following command to publish `PerfectGift.csproj` to our Azure Function App:

```bash
func azure functionapp publish PerfectGift-[YOUR NAME]
```

> **Note:** Replace `[YOUR NAME]` with your name

### Step 5: Upload Perfectly Wrapped Gift Images

Our serverless functon is now ready to verify our gift images!

In this step, we will upload images to Azure Blob Storage and confirm that our serverless function automatically verifies the image is a perfectly wrapped gift. If the image is not of a perfectly wrapped gift, it will automatically be removed from Azure Blob Storage.

1 - Download this sample image of a [perfectly wrapped gift](https://user-images.githubusercontent.com/13558917/70572373-88876980-1b54-11ea-8cd5-af07306b6d19.jpg)

2 - Move & rename the downloaded image of a perfectly wrapped gift:

  - (Windows) Save the file as C:\Downloads\gift.jpg
  - (macOS) Save the file as ~/Download/gift.jpg

3 - In the terminal, enter the following command to upload our image of a perfectly wrapped gift

```bash
az storage blob upload --container-name gifts --connection-string "[YOUR CONNECTION STRING]" --file [FILE PATH TO GIFT IMAGE] --name Gift1
```

> **Note:** Replace `[YOUR CONNECTION STRING]` with the value of **connectionString** and replace `[FILE PATH TO GIFT IMAGE]` with the file path to your wrapped gift image
> e.g. `az storage blob upload --container-name gifts --connection-string "abc123def456ghi789==" --file cd:\Downloads\gift.jpg --name Gift1`

4 - In the terminal, enter the following command to confirm the image has been **not** been deleted from storage:

```bash
az storage blob list --container-name gifts --connection-string "[YOUR CONNECTION STRING]"
```

> **Note:** Replace `[YOUR CONNECTION STRING]` with the value of **connectionString** and replace `[FILE PATH TO GIFT IMAGE]` with the file path to your wrapped gift image
> e.g. `az storage blob list --container-name gifts --connection-string "abc123def456ghi789=="`

5 - In the JSON response, confirm **"name": "Gift1"**

6 - Download this sample image of a [wrapped gift missing a bow](https://user-images.githubusercontent.com/13558917/71133980-696e8480-21b1-11ea-942e-93508643f1e7.jpg)

7 - Move & rename the downloaded image of a wrapped gift missing a bow:

  - (Windows) Save the file as C:\Downloads\nobow.jpg
  - (macOS) Save the file as ~/Download/nobow.jpg

8. In the terminal, enter the following command to upload our image of a perfectly wrapped gift

```bash
az storage blob list --container-name gifts --connection-string "[YOUR CONNECTION STRING]" --file [FILE PATH TO NO BOW GIFT IMAGE] --name Gift2
```

> **Note:** Replace `[YOUR CONNECTION STRING]` with the value of **connectionString** and replace `[FILE PATH TO GIFT IMAGE]` with the file path to your wrapped gift image
>
> e.g. `az storage blob upload --container-name gifts --connection-string "abc123def456ghi789==" --file cd:\Downloads\nobow.jpg --name Gift2`

9 - In the terminal, enter the following command to confirm the image has been **not** been deleted from storage:

```bash
az storage blob list --container-name gifts --connection-string "[YOUR CONNECTION STRING]"
```

> **Note:** Replace `[YOUR CONNECTION STRING]` with the value of **connectionString** and replace `[FILE PATH TO GIFT IMAGE]` with the file path to your wrapped gift image
>
> e.g. `az storage blob list --container-name gifts --connection-string "abc123def456ghi789=="`

10 - In the JSON response, confirm that **"name": "Gift2"** does **not** exist

> **Note:** It may take a minute for the Blob Trigger Function to analyze the uploaded image. 
>
>If **"name": "Gift2"** does still exist, run `az storage blob list --container-name gifts --connection-string "[YOUR CONNECTION STRING]"` again in a few minutes 

### Step 6: Celebrate 🎉 

We now have a working Blob Trigger that automatically verifies our gifts have been perfectly wrapped!

We have successfully completed the [Day 18 Challenge](https://25daysofserverless.com/calendar/18) of [#25DaysOfServerless](https://25daysofserverless.com)!
