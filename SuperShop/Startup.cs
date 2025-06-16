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
                cfg.User.RequireUniqueEmail = true; // Exige que o email do utilizador seja �nico
                cfg.Password.RequireDigit = false; // N�o exige d�gitos na senha
                cfg.Password.RequiredUniqueChars = 0; // N�o exige caracteres �nicos na senha
                cfg.Password.RequireUppercase = false; // N�o exige letras mai�sculas na senha
                cfg.Password.RequireLowercase = false; // N�o exige letras min�sculas na senha
                cfg.Password.RequireNonAlphanumeric = false; // N�o exige caracteres n�o alfanum�ricos na senha
                cfg.Password.RequiredLength = 6; // Exige um comprimento m�nimo de 6 caracteres para a senha
            })
            .AddEntityFrameworkStores<DataContext>();// Configura o Identity para usar o DataContext como o contexto de dados

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            
            services.AddTransient<SeedDb>(); // Adiciona o servi�o SeedDb para inje��o de depend�ncias.
                                             // Isso permite que o servi�o seja resolvido e utilizado
                                             // em outros lugares da aplica��o, como no m�todo de seeding do banco de dados.


            // Adiciona os servi�os personalizados para inje��o de depend�ncias.
            services.AddScoped<IUserHelper, UserHelper>(); 
            services.AddScoped<IImageHelper, ImageHelper>(); 
            services.AddScoped<IConverterHelper, ConverterHelper>(); 
            services.AddScoped<IProductRepository, ProductRepository>(); // Compila o interface do reposit�rio com a implementa��o Repository.
                                                           // Isso permite que o reposit�rio seja injetado em controladores e outros servi�os.

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

            app.UseAuthentication(); // Adiciona o middleware de autentica��o ao pipeline de solicita��o HTTP.

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
