using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Fruitkha.Client.Models;

namespace Fruitkha.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/cart")]
        public IActionResult Cart()
        {
            return View();
        }

        [Route("/cart/checkout")]
        public IActionResult Checkout()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/news")]
        public IActionResult News()
        {
            return View();
        }

        [Route("/shop")]
        public IActionResult Shop()
        {
            return View();
        }

        [Route("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("news/post/{id:int?}")]
        public IActionResult Post(int? id)
        {
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}