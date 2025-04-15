using Azure.AI.Inference;
using Azure;

string? token = "";
if (string.IsNullOrWhiteSpace(token))
{
    Console.Error.WriteLine("❌ Missing or empty GITHUB_TOKEN environment variable.");
    return;
}

var endpoint = new Uri("https://models.inference.ai.azure.com");
var credential = new AzureKeyCredential(token);
var model = "gpt-4.1";

var client = new ChatCompletionsClient(endpoint, credential, new AzureAIInferenceClientOptions());

var messages = new List<ChatRequestMessage>
        {
            new ChatRequestSystemMessage("You are a helpful assistant. Answer clearly and politely.")
        };

Console.WriteLine("🤖 Chat started! Type your message (or type 'exit' to quit).");

while (true)
{
    Console.Write("👤 You: ");
    string? userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput)) continue;
    if (userInput.Trim().ToLower() == "exit") break;

    messages.Add(new ChatRequestUserMessage(userInput));

    var request = new ChatCompletionsOptions
    {
        Model = model,
        Temperature = 0.7f,
        NucleusSamplingFactor = 0.95f,
        Messages = { }
    };

    foreach (var message in messages)
    {
        request.Messages.Add(message);
    }

    try
    {
        Response<ChatCompletions> response = client.Complete(request);
        string reply = response.Value.Content;

        Console.WriteLine($"🤖 AI: {reply}\n");

        // הוספת תגובת הבוט להקשר
        messages.Add(new ChatRequestAssistantMessage(reply));
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Error: {ex.Message}\n");
        Console.ResetColor();
    }
}

Console.WriteLine("👋 Chat ended. Have a great day!");
