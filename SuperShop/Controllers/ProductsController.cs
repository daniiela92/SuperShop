using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;


namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository _repository;

        public ProductsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: Products
        public IActionResult Index() // a única coisa que faz é retornar uma lista de produtos
        {
            return View(_repository.GetProducts()); 
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id) // o ? indica que o ID é opcional, ou seja, pode ser nulo
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value); // procura o produto pelo ID

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
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
                _repository.AddProduct(product); // adiciona o produto ao repositório
                await _repository.SaveAllAsync(); // salva as alterações no banco de dados
                return RedirectToAction(nameof(Index)); // redireciona para a lista de produtos
            }
            return View(product); // se o modelo não for válido, retorna a view com o produto para corrigir os erros
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id) //tem o ? para não forçar o utilizador a colocar um ID
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value); // procura o produto pelo ID, se não encontrar retorna null
            
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
                    _repository.UpdateProduct(product);
                    await _repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.ProductExists(product.Id)) // verifica se o produto existe no banco de dados
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
        public IActionResult Delete(int? id) // não apaga, só mostra o que for para apagar. Por defeito usa um GET
        {
            if (id == null) // se o ID for nulo, retorna NotFound
            {
                return NotFound();
            }

            var product = _repository.GetProduct(id.Value); // procura o produto pelo ID no repositório

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
            var product = _repository.GetProduct(id); // procura o produto pelo ID no repositório
            _repository.RemoveProduct(product);  // Remove o produto do contexto
            await _repository.SaveAllAsync();  // Salva as alterações no banco de dados
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de produtos
        }
    }
}
