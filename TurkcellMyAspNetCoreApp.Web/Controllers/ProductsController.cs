using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        //Dependency Design Injection Desing Pattern
        public ProductsController(AppDbContext context, IMapper mapper)
        {
            //DI Container -> Constructor da nesne örneği geçip almaya denir
            _productRepository = new ProductRepository();
            _context = context;
            if (!_context.Products.Any())
            {
                _context.Products.Add(new Product() { Name = "Kalem 1", Price = 100, Stock = 200, Color = "Red" });
                _context.Products.Add(new Product() { Name = "Kalem 2", Price = 200, Stock = 400, Color = "Blue" });
                _context.Products.Add(new Product() { Name = "Kalem 3", Price = 300, Stock = 500, Color = "Black" });
            }
            _context.SaveChanges();
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }

        public IActionResult Remove(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Add()
        {


            ViewBag.Expire = new Dictionary<string, int>() {
                { "1 Ay",1},
                { "3 Ay",3},
                { "6 Ay",6},
                { "12 Ay",12}
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
            {
                new (){ Data="Mavi", Value="Mavi"},
                new (){ Data="Kırmızı", Value="Kırmızı"},
                new (){ Data="Siyah", Value="Siyah"},
                new (){ Data="Beyaz", Value="Beyaz"},
                new (){ Data="Yeşil", Value="Yeşil"}

            }, "Value", "Data");
            return View();
        }
        [HttpPost]
        public IActionResult Add(ProductViewModel newProduct)
        {
            //veri alma yöntemi 1
            /*var name = HttpContext.Request.Form["Name"];
			var price = decimal.Parse(HttpContext.Request.Form["Price"]);
			var stock = int.Parse(HttpContext.Request.Form["Stock"]);
			var color = HttpContext.Request.Form["Color"];*/
            //if (newProduct.Name.StartsWith("A"))
            //{
            //    ModelState.AddModelError(String.Empty,"Ürün ismi A ile başlayamaz");
            //}
            ViewBag.Expire = new Dictionary<string, int>() {
                    { "1 Ay",1},
                    { "3 Ay",3},
                    { "6 Ay",6},
                    { "12 Ay",12}
                };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
                {
                    new (){ Data="Mavi", Value="Mavi"},
                    new (){ Data="Kırmızı", Value="Kırmızı"},
                    new (){ Data="Siyah", Value="Siyah"},
                    new (){ Data="Beyaz", Value="Beyaz"},
                    new (){ Data="Yeşil", Value="Yeşil"}
                }, "Value", "Data");

            if (ModelState.IsValid)
            {
                try
                {
                    //throw new Exception("db hatası");
                    _context.Products.Add(_mapper.Map<Product>(newProduct));
                    _context.SaveChanges();
                    TempData["status"] = "Ürün başarıyla eklendi";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    ModelState.AddModelError(String.Empty, "Kayıt esnasında hata meydana geldi");
                    return View();
                }

            }
            else
            {

                return View();
            }

        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.Expire = new Dictionary<string, int>() {
                { "1 Ay",1},
                { "3 Ay",3},
                { "6 Ay",6},
                { "12 Ay",12}
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
            {
                new (){ Data="Mavi", Value="Mavi"},
                new (){ Data="Kırmızı", Value="Kırmızı"},
                new (){ Data="Siyah", Value="Siyah"},
                new (){ Data="Beyaz", Value="Beyaz"},
                new (){ Data="Yeşil", Value="Yeşil"}

            }, "Value", "Data");

            var product = _context.Products.Find(id);
            return View(_mapper.Map<ProductViewModel>(product));
        }
        //parametre olarak sadece 1 tane class alabilir
        [HttpPost]
        public IActionResult Update(ProductViewModel updateProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(_mapper.Map<Product>(updateProduct));

                _context.SaveChanges();
                TempData["status"] = "Güncelleme işlemi başarıyla gerçekleşti.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Expire = new Dictionary<string, int>() {
                    { "1 Ay",1},
                    { "3 Ay",3},
                    { "6 Ay",6},
                    { "12 Ay",12}
                };

                ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
                {
                    new (){ Data="Mavi", Value="Mavi"},
                    new (){ Data="Kırmızı", Value="Kırmızı"},
                    new (){ Data="Siyah", Value="Siyah"},
                    new (){ Data="Beyaz", Value="Beyaz"},
                    new (){ Data="Yeşil", Value="Yeşil"}
                }, "Value", "Data");

                return View(_mapper.Map<ProductViewModel>(updateProduct));
            }
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult HasProductName(string Name)
        {
            var anyProduct = _context.Products.Any(x => x.Name.ToLower() == Name.ToLower());

            if (anyProduct)
            {
                return Json("Kaydetmeye çalıştığınız ürün ismi veritabanında mevcut");
            }
            else
            {
                return Json(true);
            }
        }
    }
}
