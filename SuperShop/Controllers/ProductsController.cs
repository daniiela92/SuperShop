using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        public ProductsController(
            IProductRepository productRepository, 
            IUserHelper userHelper)
        {

            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index() // a única coisa que faz é retornar uma lista de produtos
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id) // o ? indica que o ID é opcional, ou seja, pode ser nulo
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value); // procura o produto pelo ID

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create() // só chama a View e mostra o Form para ser preenchido
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {

                var path = string.Empty; // inicializa a variável path como uma string vazia


                if(model.ImageFile != null && model.ImageFile.Length > 0) // verifica se o ficheiro de imagem foi enviado
                {

                    var guid = Guid.NewGuid().ToString(); // gera um novo GUID para garantir que o nome do ficheiro é único

                    var file = $"{guid}.jpg"; // cria um novo nome de ficheiro com o GUID e o nome original do ficheiro


                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\products", 
                        file); // define o caminho onde a imagem será guardada

                    using(var stream = new FileStream(path, FileMode.Create)) // cria um novo ficheiro ou substitui o existente
                    {
                        await model.ImageFile.CopyToAsync(stream); // copia o conteúdo do ficheiro de imagem para o caminho definido
                    }

                    path = $"~/images/products/{file}"; // define o caminho relativo da imagem para ser usado na view
                }

                var product = this.ToProduct(model, path); // converte o modelo ProductViewModel para a entidade Product usando o método ToProduct

                //TODO: Modificar para o user que estiver logado
                product.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com"); // atribui o utilizador atual ao produto
                await _productRepository.CreateAsync(product); // chama o método CreateAsync do repositório
                                                               // para adicionar o produto

                return RedirectToAction(nameof(Index)); // redireciona para a lista de produtos
            }

            return View(model); // se o modelo não for válido, retorna a view com o produto para corrigir os erros
        }

        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product // converte o modelo ProductViewModel para a entidade Product
            {
                Id = model.Id, // atribui o ID do modelo ao produto
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

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id) //tem o ? para não forçar o utilizador a colocar um ID
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value); // procura o produto pelo ID, se não
                                                                           // encontrar retorna null

            if (product == null)
            {
                return NotFound();
            }

            var model = this.ToProductViewModel(product); // converte o produto para o modelo ProductViewModel

            return View(model);
        }

        private ProductViewModel ToProductViewModel(Product product)
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl; // inicializa a variável path com o caminho da imagem atual

                    if (model.ImageFile != null && model.ImageFile.Length > 0) // verifica se o ficheiro de imagem foi enviado
                    {
                        var guid = Guid.NewGuid().ToString(); // gera um novo GUID para garantir que o nome do ficheiro é único

                        var file = $"{guid}.jpg"; // cria um novo nome de ficheiro com o GUID e o nome original do ficheiro

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\products", 
                            file); // define o caminho onde a imagem será guardada
                        using (var stream = new FileStream(path, FileMode.Create)) // cria um novo ficheiro ou substitui o existente
                        {
                            await model.ImageFile.CopyToAsync(stream); // copia o conteúdo do ficheiro de imagem para o caminho definido
                        }
                        path = $"~/images/products/{file}"; // define o caminho relativo da imagem para ser usado na view
                    }

                    var product = this.ToProduct(model, path); // converte o modelo ProductViewModel para a entidade Product usando o método ToProduct


                    //TODO: Modificar para o user que estiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com"); // atribui o utilizador atual ao produto
                    await _productRepository.UpdateAsync(product); // chama o método UpdateProduct do
                                                                   // repositório para atualizar o produto

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id)) // verifica se o produto existe no banco de dados
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id) // não apaga, só mostra o que for para apagar. Por defeito usa um GET
        {
            if (id == null) // se o ID for nulo, retorna NotFound
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value); // procura o produto pelo ID no repositório

            if (product == null) // se o produto for nulo, retorna NotFound
            {
                return NotFound();
            }

            return View(product); // retorna a view com o produto para ser confirmado o delete
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")] //especifica que quando há uma action Delete que seja feita com Post, chama o DeleteConfirmed
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id); // procura o produto pelo ID no repositório
            await _productRepository.DeleteAsync(product);  // Remove o produto do contexto
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de produtos
        }
    }
}
