namespace Adria.Domain.Models;

public class NotificationMessage(string title, string body, string url)
{
    public string Title { get; private set; } = title;
    public string Body { get; private set; } = body;
    public string Url { get; private set; } = url;
}