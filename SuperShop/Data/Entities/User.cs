using Microsoft.AspNetCore.Identity;

namespace SuperShop.Data.Entities
{
    public class User : IdentityUser // Classe User que herda de IdentityUser, representando um utilizador no sistema
    {
        public string FirstName { get; set; } // Propriedade para o primeiro nome do utilizador

        public string LastName { get; set; } // Propriedade para o último nome do utilizador
    }
}
