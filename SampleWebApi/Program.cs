using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Kafka;
using System;

namespace SampleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.development.json", true, true)
                .Build();

            var kafkaConfiguration = new KafkaConfiguration();
            configuration.GetSection(nameof(KafkaConfiguration)).Bind(kafkaConfiguration);

            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>()
                           .UseSerilog((hostingContext, loggerConfiguration) =>
                           {
                               loggerConfiguration
                                   .Enrich.FromLogContext()
                                   .Enrich.WithClientIp()
                                   .Enrich.WithClientAgent()
                                   .Enrich.WithMachineName()
                                   .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                                   .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment)
                                   .WriteTo.Console()
                                   .WriteTo.Kafka(kafkaConfiguration.BootstrapServers, kafkaConfiguration.Topic, kafkaConfiguration.SaslUsername, kafkaConfiguration.SaslPassword);

                               Serilog.Debugging.SelfLog.Enable(message => Console.WriteLine(message));
                           });
                   });
        }
    }
}
