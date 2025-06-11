using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using System.Threading.Tasks;



namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {

            _productRepository = productRepository;
        }

        // GET: Products
        public IActionResult Index() // a única coisa que faz é retornar uma lista de produtos
        {
            return View(_productRepository.GetAll());
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
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.CreateAsync(product); // chama o método CreateAsync do repositório
                                                               // para adicionar o produto

                return RedirectToAction(nameof(Index)); // redireciona para a lista de produtos
            }

            return View(product); // se o modelo não for válido, retorna a view com o produto para corrigir os erros
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateAsync(product); // chama o método UpdateProduct do
                                                                   // repositório para atualizar o produto

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(product.Id)) // verifica se o produto existe no banco de dados
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
            return View(product);
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
