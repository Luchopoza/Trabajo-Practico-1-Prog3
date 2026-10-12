using Microsoft.AspNetCore.Mvc;
using Trabajo_Practico_1_Prog3.Models;
using Trabajo_Practico_1_Prog3.Services;

namespace Trabajo_Practico_1_Prog3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        // ASP.NET Core inyecta el Service automáticamente por el constructor
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetAll()
        {
            var productos = _productoService.ObtenerTodos();
            return Ok(productos);
        }
    }
}
