using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SLMM.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder().Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
        }
    }
}
