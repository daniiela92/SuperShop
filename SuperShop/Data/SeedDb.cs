﻿using SuperShop.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // verifica se o banco de dados foi criado, se não, cria

            
            if (!_context.Products.Any())
            {
                AddProduct("iPhone X");
                AddProduct("Magic Mouse");
                AddProduct("iWatch Series 4");
                AddProduct("iPad Mini");
                await _context.SaveChangesAsync(); // salva as alterações no banco de dados
            }

        }

        private void AddProduct(string name)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100)

            });

        }

    }
}
