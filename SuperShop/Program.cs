using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;

namespace SuperShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build(); // Cria o host builder
            RunSeeding(host); // Chama o m�todo de seeding para popular o banco de dados
            host.Run(); // Inicia o host, que por sua vez inicia o servidor web e a aplica��o ASP.NET Core
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>(); // Obt�m o escopo de servi�os para inje��o de depend�ncias
            using (var scope = scopeFactory.CreateScope()) // Cria um escopo para resolver os servi�os necess�rios
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>(); // Resolve o servi�o de SeedDb
                seeder.SeedAsync().Wait(); // Chama o m�todo SeedAsync para popular o banco de dados, aguardando sua conclus�o
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
