using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // verifica se o banco de dados foi criado, se não, cria

            var user = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com"); // tenta obter o utilizador pelo email fornecido

            if (user == null) // se o utilizador não existir, cria um novo utilizador
            {
                user = new User // cria um novo utilizador com os dados fornecidos
                {
                    FirstName = "Rafael",
                    LastName = "Santos",
                    Email = "rafaasfs@gmail.com",
                    UserName = "rafaasfs@gmail.com",
                    PhoneNumber = "123456789",
                };

                var result = await _userHelper.AddUserAsync(user, "123456"); // chama o método AddUserAsync do IUserHelper para adicionar o
                                                                             // utilizador com a senha "123456"

                if (result != IdentityResult.Success) // verifica se a criação do utilizador foi bem-sucedida
                {

                    throw new InvalidOperationException("Could not create the user in seeding"); // se não foi bem-sucedida,
                                                                                                 // lança uma exceção

                }
            }



            if (!_context.Products.Any()) // verifica se não existem produtos no banco de dados
            {
                AddProduct("iPhone X", user);
                AddProduct("Magic Mouse", user);
                AddProduct("iWatch Series 4", user);
                AddProduct("iPad Mini", user);
                await _context.SaveChangesAsync(); // salva as alterações no banco de dados
            }

        }

        private void AddProduct(string name, User user) // método privado para adicionar um produto ao banco de dados
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user,

            });

        }

    }
}
