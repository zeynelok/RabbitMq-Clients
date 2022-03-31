
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Producer_ConsoleApp;

try
{
    var host = CreateDefaultBuilder().Build();
    var serviceScope = host.Services.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var instance = provider.GetRequiredService<Publisher>();
    instance.PublishToRabbit();
    host.Run();


}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadLine();
}


static IHostBuilder CreateDefaultBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((services) =>
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json", false).Build();

            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqOptions"));
            services.AddSingleton<Publisher>();
        });
}