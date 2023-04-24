namespace DemoDataPublisher.Models;

public class DemoEventModel
{
    public string? ServiceName { get; set; }
    public string? Environment { get; set; }
    public Exception? Exception { get; set; }
}