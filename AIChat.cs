using System;
using Microsoft.Extensions.Configuration;
//run 'dotnet add package Microsoft.Extensions.Configuration.Json' to add the configuration package
using Azure;
//run 'dotnet add package Azure.Identity' to add the Azure Identity SDK
using Azure.Identity;
//run 'dotnet add package Azure.AI.Projects --prerelease' to add the Azure AI Projects SDK
using Azure.AI.Projects;
//run 'dotnet add package Azure.AI.Inference' to add the Azure AI Inference SDK
using Azure.AI.Inference;

namespace AzureAIFoundarySDKChat;
class Program {
    static void Main(string[] args) {
        try
        {
            Console.WriteLine("Azure AI Chat Client | gpt-4o | Azure AI Projects SDK");
            
            //get config - config file in bim\debug\net9.0
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();

            //get reference to the AI Model in Azure (project client)
            var projectEndpoint = configuration["PROJECT_ENDPOINT"];
            var projectClient = new AIProjectClient(new Uri(projectEndpoint), new DefaultAzureCredential());
            //DefaultAzureCredential will use the Azure CLI credentials or environment variables - connect with 'az login' or set AZURE_CLIENT_ID, AZURE_TENANT_ID, AZURE_CLIENT_SECRET environment variable
            //https://learn.microsoft.com/en-us/cli/azure/authenticate-azure-cli

            /*
            //list all project connections
            Console.WriteLine("Listing all project connections:");
            var connectionsClient = projectClient.GetConnectionsClient();
            foreach (var connection in connectionsClient.GetConnections()) {
                Console.WriteLine($"Connection ID: {connection.Id}, Name: {connection.Name}");
            }
            */

            //get a chat client
            ChatCompletionsClient chat = projectClient.GetChatCompletionsClient();

            //reference deployment name
            string modelDeployment = configuration["MODEL_DEPLOYMENT"];

            //provide system prompt
            var prompt = new List<ChatRequestMessage>()
            {
                new ChatRequestSystemMessage("You are a helpful assistant that provides information about Azure AI services.")
            };

            Console.WriteLine("Chat (or 'quit'):");
            string? userInput;
            userInput = Console.ReadLine();

            while (userInput != null && userInput.ToLower() != "quit")
            {
                //get a chat completion
                prompt.Add(new ChatRequestUserMessage(userInput));
                var requestOptions = new ChatCompletionsOptions()
                {
                    Messages = prompt,
                    Model = modelDeployment,
                };

                Response<ChatCompletions> response = chat.Complete(requestOptions);
                var completion = response.Value.Content;
                Console.WriteLine(completion);
                prompt.Add(new ChatRequestAssistantMessage(completion));
                
                //get next user input
                userInput = Console.ReadLine();
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error initializing project client: {ex.Message}");
            return;
        }
    }
}