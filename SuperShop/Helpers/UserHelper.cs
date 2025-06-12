using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public class UserHelper : IUserHelper // Implementação da interface IUserHelper
                                          // para auxiliar na manipulação/gestão de utilizadores
    {
        private readonly UserManager<User> _userManager;

        public UserHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password); // Método assíncrono para adicionar um utilizador com uma senha
        }

        public async Task<User> GetUserByEmailAsync(string email) 
        {
            return await _userManager.FindByEmailAsync(email); // Método assíncrono para obter um utilizador pelo email
        }
    }
}
