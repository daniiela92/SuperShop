using SuperShop.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperShop.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity // Repositório genérico que implementa a interface IGenericRepository<T>
                                                                                       // para operar com entidades do tipo T
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll() // Método para obter todas as entidades do tipo T
        {
            return _context.Set<T>().AsNoTracking(); // AsNoTracking() é usado para melhorar o 
                                                     // desempenho ao ler dados sem rastreamento de alterações
        }

        public async Task<T> GetByIdAsync(int id) // Método assíncrono para obter uma entidade do tipo T pelo ID
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id); // FindAsync é usado para buscar a entidade pelo ID
        }

        public async Task CreateAsync(T entity) // Método assíncrono para criar uma nova entidade do tipo T
        {
            await _context.Set<T>().AddAsync(entity); // AddAsync é usado para adicionar a entidade ao contexto
            await SaveAllAsync(); // Chama o método SaveAllAsync para salvar as alterações no banco de dados
        }

        public async Task UpdateAsync(T entity) // Método assíncrono para atualizar uma entidade do tipo T
        {
            _context.Set<T>().Update(entity); // Update é usado para marcar a entidade como modificada
            await SaveAllAsync(); // Chama o método SaveAllAsync para salvar as alterações no banco de dados
        }

        public async Task DeleteAsync(T entity)
        {
                _context.Set<T>().Remove(entity);
                await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id) // Método assíncrono para atualizar uma entidade do tipo T
        {
           return await _context.Set<T>().AnyAsync(e => e.Id == id); // AnyAsync é usado para
                                                                     // verificar se existe alguma entidade com o ID fornecido
        }


        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; // Salva as alterações no banco de dados
                                                          // e retorna true se houver alterações
        }

       
    }
}
