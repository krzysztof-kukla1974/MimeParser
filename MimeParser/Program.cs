using MimeKit;
using System.Text.Json;

namespace EmlFileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "/Users/krzysztof.kukla/Downloads/SampleEmails"; //args[0];

            // Validate folder path
            if (string.IsNullOrWhiteSpace(filePath) || !Directory.Exists(filePath))
            {
                Console.WriteLine("Invalid folder path. Please try again.");
                return;
            }

            // Get all .eml files in the folder
            string[] emlFiles = Directory.GetFiles(filePath, "*.eml");
            if (emlFiles.Length == 0)
            {
                Console.WriteLine("No .eml files found in the specified folder.");
                return;
            }

            Console.WriteLine($"Found {emlFiles.Length} .eml file(s). Processing...");

            var masterJson = new
            {
                emails = new List<Object>()
            };

            // Process each .eml file
            foreach (string emlFile in emlFiles)
            {
                try
                {
                    using (var stream = File.OpenRead(emlFile))
                    {
                        var parser = new MimeParser(stream, MimeFormat.Entity);
                        var message = parser.ParseMessage();

                        var email = new
                        {
                            Name = emlFile,
                            Subject = message.Subject,
                            From = message.From.ToString(),
                            To = message.To.ToString(),
                            Date = message.Date.ToString(),
                            TextBody = message.TextBody,
                            HtmlBody = message.HtmlBody
                        };

                        masterJson.emails.Add(email);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file '{emlFile}': {ex.Message}");
                }
            }

            // Completed
            Console.WriteLine(JsonSerializer.Serialize(masterJson));
            return;
        }
    }
}