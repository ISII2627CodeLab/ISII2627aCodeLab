namespace LogViewer;
using Microsoft.Extensions.Configuration;
class Program
{
    static async Task Main(string[] args)
    {
        var configuration = BuildConfiguration();
        var subscriber = new Subscriber(configuration);
        await subscriber.StartAsync();

        Console.WriteLine("LogViewer listo. Esperando logs... [Enter] para salir.");
        Console.ReadLine();
    }
    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
    }
    
}
