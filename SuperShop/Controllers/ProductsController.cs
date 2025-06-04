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
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index() // a única coisa que faz é retornar uma lista de produtos
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id) //tem o ? para não forçar o utilizador a colocar um ID
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            // FindAsync é usado para procurar um produto pelo ID, se não encontrar retorna null. 
            //
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
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            var product = await _context.Products // procura o produto pelo ID, se não encontrar retorna null
                .FirstOrDefaultAsync(m => m.Id == id); // FirstOrDefaultAsync retorna o primeiro produto
                                                       // que corresponde ao ID ou null se não encontrar
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
            var product = await _context.Products.FindAsync(id); // antes de apagar ainda vai verificar se o produto existe
            _context.Products.Remove(product);  // Remove o produto do contexto
            await _context.SaveChangesAsync();  // Salva as alterações no banco de dados
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de produtos
        }

        private bool ProductExists(int id) // verifica se o produto existe no banco de dados, método auxiliar
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
