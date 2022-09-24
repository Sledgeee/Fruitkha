using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Fruitkha.Client.Models;
using Fruitkha.Services.Emailer;
using Fruitkha.Services.Emailer.Models;

namespace Fruitkha.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public HomeController(
            ILogger<HomeController> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [Route("/send-email")]
        public IActionResult SendEmailAsync()
        {
            var settings = _configuration.GetSection("EmailSettings").Get<MailSettings>();
            var vm = new SendEmailVM()
            {
                Subject = "test",
                Message = "test",
                To = "bazisc.qa@gmail.com"
            };
            var result = _emailSender.SendEmail(vm, settings);
            return Ok(result);
        }

        [Route("")]
        [Route("home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("cart")]
        public IActionResult Cart()
        {
            return View();
        }

        [Route("cart/checkout")]
        public IActionResult Checkout()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("news")]
        public IActionResult News()
        {
            return View();
        }

        [Route("shop")]
        public IActionResult Shop()
        {
            return View();
        }

        [Route("contact")]
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