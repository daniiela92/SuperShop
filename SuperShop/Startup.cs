using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;

namespace SuperShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true; // Exige que o email do utilizador seja único
                cfg.Password.RequireDigit = false; // Não exige dígitos na senha
                cfg.Password.RequiredUniqueChars = 0; // Não exige caracteres únicos na senha
                cfg.Password.RequireUppercase = false; // Não exige letras maiúsculas na senha
                cfg.Password.RequireLowercase = false; // Não exige letras minúsculas na senha
                cfg.Password.RequireNonAlphanumeric = false; // Não exige caracteres não alfanuméricos na senha
                cfg.Password.RequiredLength = 6; // Exige um comprimento mínimo de 6 caracteres para a senha
            })
            .AddEntityFrameworkStores<DataContext>();// Configura o Identity para usar o DataContext como o contexto de dados

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            
            services.AddTransient<SeedDb>(); // Adiciona o serviço SeedDb para injeção de dependências.
                                             // Isso permite que o serviço seja resolvido e utilizado
                                             // em outros lugares da aplicação, como no método de seeding do banco de dados.


            // Adiciona os serviços personalizados para injeção de dependências.
            services.AddScoped<IUserHelper, UserHelper>(); 
            services.AddScoped<IImageHelper, ImageHelper>(); 
            services.AddScoped<IConverterHelper, ConverterHelper>(); 
            services.AddScoped<IProductRepository, ProductRepository>(); // Compila o interface do repositório com a implementação Repository.
                                                           // Isso permite que o repositório seja injetado em controladores e outros serviços.

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Adiciona o middleware de autenticação ao pipeline de solicitação HTTP.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
