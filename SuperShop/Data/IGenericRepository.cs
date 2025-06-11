using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Data
{
    public interface IGenericRepository<T> where T : class // Interface genérica para repositórios que
                                                           // operam com entidades do tipo T
    {                                                        

        IQueryable<T> GetAll(); // Método para obter todas as entidades do tipo T

        Task<T> GetByIdAsync(int id); // Método assíncrono para obter uma entidade do tipo T pelo ID

        Task CreateAsync(T entity); // Método assíncrono para criar uma nova entidade do tipo T

        Task UpdateAsync(T entity); // Método assíncrono para atualizar uma entidade do tipo T

        Task DeleteAsync(T entity); // Método assíncrono para excluir uma entidade do tipo T 

        Task<bool> ExistAsync (int id); // Método assíncrono para verificar se uma entidade do tipo T existe pelo ID


        // Não foi criado um método para salvar as alterações, pois a
        // interface é genérica e não sabe como as alterações devem ser salvas, em que tabela

    }
}
