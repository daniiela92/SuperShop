using SuperShop.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context) // contrutor do repoiitório que recebe o contexto do banco de dados
        {
            _context = context;
        }



        //Método que retorna todos os produtos do banco de dados
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Name); // retorna a lista de produtos do banco de dados,
                                                           // or ordem alfabética pelo nome
        }

        // Método que retorna um produto pelo seu ID
        public Product GetProduct(int id)
        {
            return _context.Products.Find(id); // procura o produto pelo ID no banco de dados e retorna o produto encontrado

        }


        // Método que adiciona um produto ao banco de dados
        public void AddProduct(Product product)
        {
            _context.Products.Add(product); // adiciona o produto ao contexto do banco de dados

        }

        //Método que atualiza um produto no banco de dados
        public void UpdateProduct(Product product)
        {

            _context.Products.Update(product); // atualiza o produto no contexto do banco de dados
        }

        // Método que remove um produto do banco de dados

        public void RemoveProduct(Product product)
        {
            _context.Products.Remove(product); // remove o produto do contexto do banco de dados
        }

        // Método que salva as alterações no banco de dados
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; // salva as alterações no banco de dados 
                                                          // e retorna true se houver alterações salvas,
                                                          // caso contrário, retorna false


        }
        // Método que verifica se um produto existe no banco de dados pelo ID, mas não o retorna
        public bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id); // verifica se o produto existe no banco de dados
        }
    }
}
