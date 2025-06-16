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
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(
            IProductRepository productRepository, 
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {

            _productRepository = productRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
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

                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products"); // chama o método UploadImageAsync do
                                                                                             // IImageHelper para fazer o upload da imagem e obter o caminho
                }

                var product = _converterHelper.ToProduct(model, path, true); // converte o modelo ProductViewModel
                                                                         // para a entidade Product usando o método ToProduct do IConverterHelper
                //TODO: Modificar para o user que estiver logado
                product.User = await _userHelper.GetUserByEmailAsync("rafaasfs@gmail.com"); // atribui o utilizador atual ao produto
                await _productRepository.CreateAsync(product); // chama o método CreateAsync do repositório
                                                               // para adicionar o produto

                return RedirectToAction(nameof(Index)); // redireciona para a lista de produtos
            }

            return View(model); // se o modelo não for válido, retorna a view com o produto para corrigir os erros
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

            var model = _converterHelper.ToProductViewModel(product); // chama o método ToProductViewModel do
                                                                  // IConverterHelper para converter a entidade Product em um modelo ProductViewModel
            return View(model);
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
                      
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "products"); // chama o método UploadImageAsync do
                                                                                                 // IImageHelper para fazer o upload da imagem e obter o caminho
                    }

                    var product = _converterHelper.ToProduct(model, path, false); // converte o modelo ProductViewModel
                                                                                  // para a entidade Product usando o método ToProduct do IConverterHelper

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
