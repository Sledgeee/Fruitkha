using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Client.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger _logger;

        public CartController(ILogger logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Coupon applied");

            return Ok();
        }
    }
}
