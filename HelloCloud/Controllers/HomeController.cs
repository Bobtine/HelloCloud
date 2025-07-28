using HelloCloud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HelloCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly TestProduitsContext _context;

        public HomeController(ILogger<HomeController> logger, TestProduitsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            IList<Category> categories = new List<Category>();

            try
            {
                categories = _context.Categories
                        .Include(c => c.Produits)
                            .ToListAsync().Result;
                Console.WriteLine("Connexion réussie robertTine !");
                _logger.LogDebug("Connexion réussie Log robertTine!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur robertTine : {ex.Message}");

                _logger.LogCritical($"Erreur Log robertTine : {ex.Message} ");
            }

            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("debug/config")]
        public IActionResult ConfigDebug([FromServices] IConfiguration config, [FromServices] IWebHostEnvironment env)
        {
            var conn = config.GetConnectionString("DefaultConnection");
            return Content($"ENV: {env.EnvironmentName} \nConnection: {conn}");
        }
    }
}
