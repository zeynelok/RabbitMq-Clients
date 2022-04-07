
using Consumer_ConsoleApp;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

try
{

    var host = CreateDefaultBuilder().Build();
    var serviceScope = host.Services.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var instance = provider.GetRequiredService<Subscriber>();
    instance.Subscribe();
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
          
            services.AddSingleton<Subscriber>();
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(new ConfigurationOptions() {
                EndPoints = { configuration.GetSection("RedisOptions:Endpoint").Value },
               Password=configuration.GetSection("RedisOptions:Password").Value

            }));
        })
        .ConfigureLogging(builder =>
        builder
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("Microsoft", LogLevel.Warning));
}
