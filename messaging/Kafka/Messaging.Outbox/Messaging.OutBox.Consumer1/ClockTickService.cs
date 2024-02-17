using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace Messaging.OutBox.Consumer1;

public class ClockTickService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            AnsiConsole.MarkupLine($"[bold grey]Testing {DateTime.UtcNow.ToLongTimeString()}[/]");
            await Task.Delay(1000, stoppingToken);
        }
    }
}