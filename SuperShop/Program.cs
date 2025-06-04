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
            RunSeeding(host); // Chama o método de seeding para popular o banco de dados
            host.Run(); // Inicia o host, que por sua vez inicia o servidor web e a aplicação ASP.NET Core
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>(); // Obtém o escopo de serviços para injeção de dependências
            using (var scope = scopeFactory.CreateScope()) // Cria um escopo para resolver os serviços necessários
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>(); // Resolve o serviço de SeedDb
                seeder.SeedAsync().Wait(); // Chama o método SeedAsync para popular o banco de dados, aguardando sua conclusão
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
