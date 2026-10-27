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

        // GET /api/productos
        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetAll()
        {
            var productos = _productoService.ObtenerTodos();
            return Ok(productos);
        }

        // GET /api/productos/{id}
        [HttpGet("{id:int}")]
        public ActionResult<Producto> GetById(int id)
        {
            var producto = _productoService.ObtenerPorId(id);

            if (producto == null)
            {
                return NotFound();   // 404: el producto no existe
            }

            return Ok(producto);     // 200: el producto existe
        }
    }
}