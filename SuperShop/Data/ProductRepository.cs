using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository // Implementa a interface
                                                                                    // IProductRepository e
                                                                                    // herda de GenericRepository<Product>
    {
        // Aqui podem ser adicionados métodos específicos para o repositório de produtos, se necessário.
        // Por exemplo, você pode adicionar métodos para buscar produtos por categoria, etc.
        public ProductRepository(DataContext context) : base(context) // Construtor que recebe o contexto de dados e
                                                                     // o passa para a classe base
        {




        }
        
        
            
        

    }

}

