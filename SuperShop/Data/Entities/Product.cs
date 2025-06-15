using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    public class Product : IEntity // Implementa a interface IEntity para garantir que a classe Product tenha um
                                   // identificador único e outras propriedades definidas na interface.
    {

        public int Id { get; set; }

        [Required] // Preenchimento obrigatório
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")] // Tamanho máximo de 50 caracteres
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; } // com o ? indica que é opcional

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; } // com o ? indica que é opcional

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public User User { get; set; } // Propriedade de navegação para a entidade User, que representa o utilizador associado ao produto

        public string ImageFullPath
        {
            get
            {
                if(string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                return $"https://localhost:44360{ImageUrl.Substring(1)}";
            }
        }

    }
}
