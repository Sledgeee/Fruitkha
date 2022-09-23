using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Client.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ProductController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("shop/product/{id:int}")]
    public IActionResult Index(int id)
    {
        return View();
    }
}