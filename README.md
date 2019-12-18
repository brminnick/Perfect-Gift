# Perfect Gift

As part of the [Day 18 Challenge](https://25daysofserverless.com/calendar/18) of [#25DaysOfServerless](https://25daysofserverless.com), we are using [Azure Blob Storage](https://azure.microsoft.com/services/storage/blobs?WT.mc_id=25daysofserverless-github-bramin), [Azure Cognitive Services](https://azure.microsoft.com/services/cognitive-services?WT.mc_id=25daysofserverless-github-bramin), and [Azure Functions](https://docs.microsoft.com/azure/azure-functions/?WT.mc_id=25daysofserverless-github-bramin) to ensure that each gift is perfectly wrapped.

Each present must be wrapped according to the following rules

1. Placed in a box
2. Box is wrapped
3. A bow / ribbon placed on top

## Example

Using this [example of a perfectly wrapped gift](https://user-images.githubusercontent.com/13558917/70572373-88876980-1b54-11ea-8cd5-af07306b6d19.jpg), the Computer Vision API confirms the following **Tags**:

-   [x] Box
-   [x] Gift Wrapping
-   [x] Ribbon
-   [x] Present

[![Computer Vision Results Example](https://user-images.githubusercontent.com/13558917/70573740-71964680-1b57-11ea-9126-e71f2de14a45.png)](https://azure.microsoft.com/services/cognitive-services/computer-vision?WT.mc_id=25daysofserverless-github-bramin)

## Creating the Solution

### Step 0: Install Tools

1. Install .NET Core v3.1
- In a browser, navigate to the [Download .NET Core Website](https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=25daysofserverless-github-bramin)
- On the **Download .NET Core Website**, install .NET Core 3.1

2. Install the [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest?WT.mc_id=25daysofserverless-github-bramin)
- (Windows) [Download the MSI Installer](https://aka.ms/installazurecliwindows)
- (macOS) In the terminal, enter the following command:

```bash
brew update && brew install azure-cli
```

3. Install [Azure Functions Core Tools v3.x](https://docs.microsoft.com/azure/azure-functions/functions-run-local?WT.mc_id=25daysofserverless-github-bramin#v3)
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

4. Install Git Command Line Tools
- In a browser, navigate to the [Git Downloads Page](https://git-scm.com/downloads)
- On the Git Downloads Page, install git for your specific operating system

### Step 1: Downloading the Solution Repo

0. Star the [Solution Repo](https://github.com/brminnick/Perfect-Gift)
> **Note** Starring the repo will help it become more discoverable, helping more folks achieve the solution
- In a browser, navigate to the [Perfect Gift repo](https://github.com/brminnick/Perfect-Gift)
- In the browser, tt the top of the page, click **Star**
![GitHub Star](https://user-images.githubusercontent.com/13558917/71127002-02e06b00-219f-11ea-9c10-347049d4fcf7.png)

1. Fork the [Solution Repo](https://github.com/brminnick/Perfect-Gift)
- In a browser, navigate to the [Perfect Gift repo](https://github.com/brminnick/Perfect-Gift)
- In the browser, tt the top of the page, click **Star**
![GitHub Fork](https://user-images.githubusercontent.com/13558917/71126991-fa883000-219e-11ea-9da5-26a6f893b439.png)

2. Clone the newly Forked Repo
- In the terminal, enter the following command:

```bash
git clone https://github.com/[your github user name]/Perfect-Gift
```
> **Note** Replace `[your github user name]` with your [GitHub User name](https://stackoverflow.com/a/19077217/5953643)
