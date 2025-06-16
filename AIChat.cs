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
            
            // List all project connections if any exist
            Console.WriteLine("Checking for project connections...");
            var connectionsClient = projectClient.GetConnectionsClient();
            var connections = connectionsClient.GetConnections().ToList();

            if (connections.Any()) {
                Console.WriteLine("Listing all project connections:");
                foreach (var connection in connections) {
                    Console.WriteLine($"Connection ID: {connection.Id}, Name: {connection.Name}");
                }
            } else {
                Console.WriteLine("No project connections found.");
            }

            //reference deployment name
            string modelDeployment = configuration["MODEL_DEPLOYMENT"];

            //get a chat client
            ChatCompletionsClient chat = projectClient.GetChatCompletionsClient();

            //provide intial prompt. Contributes to token count
            var prompt = new List<ChatRequestMessage>() {
                new ChatRequestSystemMessage("You are a helpful assistant that provides information about Azure AI services.")
            };

            //initialize chat
            Console.WriteLine($"{Environment.NewLine}Say hello! (or 'quit'):");
            string? userInput;
            userInput = Console.ReadLine();

            //enter chat loop
            while (userInput != null && userInput.ToLower() != "quit")
            {
                //add user input to the prompt
                prompt.Add(new ChatRequestUserMessage(userInput));
                //specify chat completion options (model to query, messages to use)
                var requestOptions = new ChatCompletionsOptions()
                {
                    Messages = prompt,
                    Model = modelDeployment,
                };

                //pass chat completion options to the chat client, request a chat completion
                Response<ChatCompletions> response = chat.Complete(requestOptions);

                //capture completion response and write to console
                var completion = response.Value.Content;
                Console.WriteLine(completion);

                //add completion to prompt for context in next request -  helps maintain context in the conversation
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