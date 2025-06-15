using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data
{
    public interface IProductRepository : IGenericRepository<Product> // Interface específica para repositórios de produtos
                                                                       // que herda de IGenericRepository<Product>
    {

        public IQueryable GetAllWithUsers(); // Método para obter todos os produtos com os utilizadores associados

    }
}
