using MimeKit;

string fileName = args[0];

using (var stream = File.OpenRead(fileName))
{
    var parser = new MimeParser(stream, MimeFormat.Entity);
    var message = parser.ParseMessage();
    Console.WriteLine(message.Subject);
    Console.WriteLine(message.From.ToString());
    Console.WriteLine(message.To.ToString());
    Console.WriteLine(message.Date.ToString());
    Console.WriteLine(message.TextBody);
    Console.WriteLine(message.HtmlBody);
}