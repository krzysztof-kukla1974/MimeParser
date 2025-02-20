using MimeKit;
using System.Text.Json;

public class Worker
{
    private string me;
    private string[] files;
    public static int ThreadsInUse = 0;
    public Worker(string me, string[] files)
    {
        this.me = me;
        this.files = files;
        Interlocked.Increment(ref ThreadsInUse);
    }

    public void Run()
    {
        Log("STARTED");

        foreach (string emlFile in this.files)
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

                    //Log(JsonSerializer.Serialize(email));
                }
            }
            catch (Exception ex)
            {
                Log($"Error processing file '{emlFile}': {ex.Message}");
            }
        }

        Log("COMPLETED");
        Interlocked.Decrement(ref ThreadsInUse);
    }

    private void Log(string msg)
    {
        Console.WriteLine($"{this.me}: {msg}");
    }
}