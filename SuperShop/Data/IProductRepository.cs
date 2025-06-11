using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    public interface IProductRepository : IGenericRepository<Product> // Interface específica para repositórios de produtos
                                                                       // que herda de IGenericRepository<Product>
    {


    }
}
