using Microsoft.AspNetCore.Http;
using SuperShop.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class ProductViewModel : Product
    {

        [Display(Name = "Image")] // Exibe o nome "Image" na interface do utilizador
        public IFormFile ImageFile { get; set; } // Propriedade para receber o ficheiro de imagem enviado pelo utilizador

    }
}
