# Azure AI Foundry SDK Chat – C# Console App

This is a simple C# console application that demonstrates how to integrate with Azure AI Foundry services using the Azure AI Foundry SDK. It provides a basic chat interface where users can input prompts and receive AI-generated responses in real time.

## 🔧 Features

- Chat with Azure AI via the Foundry SDK
- Console-based interface for simplicity
- Uses Azure's latest AI endpoint and model APIs
- Easy to configure and extend

## 🚀 How It Works

The app initializes an AI client using the provided Azure endpoint and API key, and then enters a console-based chat loop.

User input is sent to Azure AI, and the response is printed to the terminal.

## 📦 Requirements

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or higher
- An Azure AI Foundry resource with:
  - Endpoint URL
  - API Key

## 🗂️ Project Structure

```
.
├── AIChat.cs                                    # Main chat logic
├── AzureAIFoundarySDKChat-CSharp.sln            # C# Solution file
├── AzureAIFoundarySDKChat-CSharp.csproj         # C# Project file
└── README.md
```

## 📌 Notes

I built this as a minimal working example for fun, demoing and learning. It is a capable framework for extensibility.

It can be expanded to support conversation session history, streaming responses, or UI interfaces.

## 🛠️ Setup

**1. Clone the repository**  
```bash
git clone https://github.com/adrianmuzzi/AzureAIFoundarySDKChat-CSharp.git
cd AzureAIFoundarySDKChat-CSharp
```

**2. Reference your Azure AI Resources**

On Windows, update the App Settings (bin\Debug\net9.0\appsettings.json):
```
"PROJECT_ENDPOINT": "<your-endpoint-url>",
"MODEL_DEPLOYMENT": "<your-model-deployment-name>"
```

You could, alternatively, create and reference env variables:
```
$env:"PROJECT_ENDPOINT": "<your-endpoint-url>"
$env:"MODEL_DEPLOYMENT": "<your-model-deployment-name>"
```

The project then uses ```DefaultAzureCredential``` to authenticate against the Azure Subscription that houses your resources. Populate the credential with an Azure CLI session by signing in with ```az login```

Please note that this is not intended for production environments.

**3. Build and Run!**

In the root directory:
```
dotnet run
```
### 🙌 Contributing
Contributions are welcome! Feel free to open issues or submit pull requests.
