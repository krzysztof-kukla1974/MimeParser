using MimeKit;
using System.Text.Json;


class Program
{
    static void Main(string[] args)
    {
        string filePath = "";
        try
        {
            filePath = args[0]; // "/Users/krzysztof.kukla/Downloads/SampleEmails";
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("No folder path. Please try again.");
            return;
        }

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

        List<Thread> threads = new List<Thread>();

        DateTime startTime = DateTime.Now;

        for (int i = 0; i < 4; i++)
        {
            var o = new Worker("T" + (i+1), emlFiles);
            new Thread(new ThreadStart(o.Run)).Start();
        }

        while (Worker.ThreadsInUse > 0)
        {
            Console.WriteLine($"Threads in use: {Worker.ThreadsInUse.ToString()}");
            Thread.Sleep(5000);
        }

        DateTime endTime = DateTime.Now;
        Console.WriteLine($"Duration: {endTime - startTime}");

        return;
    }
}