using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IUserHelper // Interface para auxiliar na manipulação/gestão de utilizadores
    {

        Task<User> GetUserByEmailAsync(string email); // Método assíncrono para obter um utilizador pelo email

        Task<IdentityResult> AddUserAsync(User user, string password); // Método assíncrono para adicionar um utilizador com uma senha

    }
}
