using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OBS.Dashboard.Map
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Erstellt und konfiguriert den Webhost.
        /// </summary>
        /// <param name="args">Kommandozeilenargumente</param>
        /// <returns>
        /// => Eine Instanz von IHostBuilder
        /// </returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
