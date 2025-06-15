using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository // Implementa a interface
                                                                                    // IProductRepository e
                                                                                    // herda de GenericRepository<Product>
    {
        private readonly DataContext _context;

        // Aqui podem ser adicionados métodos específicos para o repositório de produtos, se necessário.
        // Por exemplo, você pode adicionar métodos para buscar produtos por categoria, etc.
        public ProductRepository(DataContext context) : base(context) // Construtor que recebe o contexto de dados e
                                                                     // o passa para a classe base
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers() // Método para obter todos os produtos com os utilizadores associados
        {
            return _context.Products.Include(p => p.User);  // semelhante ao join no SQL, inclui a entidade User
        }





    }

}

