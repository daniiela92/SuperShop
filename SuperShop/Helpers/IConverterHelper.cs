using SuperShop.Data.Entities;
using SuperShop.Models;

namespace SuperShop.Helpers
{
    public interface IConverterHelper
    {

        Product ToProduct(ProductViewModel model, string path, bool isNew); // Método para converter um modelo ProductViewModel em uma entidade Product,
                                                               // com um parâmetro isNew para indicar se é um novo produto 
                                                               
        ProductViewModel ToProductViewModel(Product product); // Método para converter uma entidade Product em
                                                              // um modelo ProductViewModel
    }
}
