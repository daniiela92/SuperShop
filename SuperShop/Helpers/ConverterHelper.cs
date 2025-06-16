using SuperShop.Data.Entities;
using SuperShop.Models;
using System.IO;

namespace SuperShop.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel model, string path, bool isNew)
        {
            return new Product // converte o modelo ProductViewModel para a entidade Product
            {
                Id = isNew ? 0 : model.Id, // se for um novo produto, o ID é 0, caso contrário, usa o ID do modelo
                ImageUrl = path, // atribui o caminho da imagem ao produto
                IsAvailable = model.IsAvailable, // atribui a disponibilidade do modelo ao produto
                LastPurchase = model.LastPurchase, // atribui a última compra do modelo ao produto
                LastSale = model.LastSale, // atribui a última venda do modelo ao produto
                Name = model.Name, // atribui o nome do modelo ao produto
                Price = model.Price, // atribui o preço do modelo ao produto
                Stock = model.Stock, // atribui o stock do modelo ao produto
                User = model.User // atribui o utilizador do modelo ao produto
            };
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel // converte a entidade Product para o modelo ProductViewModel
            {
                Id = product.Id, // atribui o ID do produto ao modelo
                IsAvailable = product.IsAvailable, // atribui a disponibilidade do produto ao modelo
                LastPurchase = product.LastPurchase, // atribui a última compra do produto ao modelo
                LastSale = product.LastSale, // atribui a última venda do produto ao modelo
                ImageUrl = product.ImageUrl, // atribui o caminho da imagem do produto ao modelo
                Name = product.Name, // atribui o nome do produto ao modelo
                Price = product.Price, // atribui o preço do produto ao modelo
                Stock = product.Stock, // atribui o stock do produto ao modelo
                User = product.User // atribui o utilizador do produto ao modelo
            };
        }
    }
}
