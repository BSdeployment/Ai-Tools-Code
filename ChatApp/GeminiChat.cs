using System.Text.Json;
using System.Text;

var apiKey = "";
if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("נא להגדיר את משתנה הסביבה GEMINI_API_KEY עם המפתח שלך.");
    return;
}

var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

while (true)
{
    Console.Write("אתה: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput)) break;

    var requestBody = new
    {
        contents = new[]
        {
                    new {
                        parts = new[] {
                            new { text = userInput }
                        }
                    }
                }
    };

    var json = JsonSerializer.Serialize(requestBody);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    using var client = new HttpClient();
    var response = await client.PostAsync(endpoint, content);
    var responseContent = await response.Content.ReadAsStringAsync();

    try
    {
        using var doc = JsonDocument.Parse(responseContent);
        var message = doc.RootElement
                         .GetProperty("candidates")[0]
                         .GetProperty("content")
                         .GetProperty("parts")[0]
                         .GetProperty("text")
                         .GetString();

        Console.WriteLine($"ג'מיני: {message}\n");
    }
    catch
    {
        Console.WriteLine("⚠️ שגיאה בעיבוד התגובה מהשרת:");
        Console.WriteLine(responseContent);
    }
}
