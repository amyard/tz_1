using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            /* override default Main for creating db in beginning if it's not exists */
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<StoreContext>();

                    /* add migrations if db does not exists */
                    await context.Database.MigrateAsync();

                    /* generate data if empty database */
                    await GenerateTransaction(context);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migrations.");
                }
            }

            host.Run();
        }

        private static async Task GenerateTransaction(StoreContext context)
        {
            if (!context.TransactionMDs.Any())
            {
                var dataFromSJsonFile = File.ReadAllText("../Backend/Data/SeedData/trans_examples.json");
                var data = JsonSerializer.Deserialize<List<TransactionMD>>(dataFromSJsonFile);

                foreach(var item in data)
                {
                    TransactionMD obj = new TransactionMD()
                    {
                        TransactionId = item.TransactionId,
                        Status = item.Status,
                        Type = item.Type,
                        ClientName = item.ClientName,
                        Price = item.Price
                    };

                    await context.TransactionMDs.AddAsync(obj);
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
