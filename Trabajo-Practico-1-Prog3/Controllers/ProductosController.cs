using Microsoft.AspNetCore.Mvc;
using Trabajo_Practico_1_Prog3.Data;
using Trabajo_Practico_1_Prog3.Models;
using Trabajo_Practico_1_Prog3.Services;

namespace Trabajo_Practico_1_Prog3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET /api/productos
        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetAll()
        {
            try
            {
                var productos = _productoService.ObtenerTodos();
                return Ok(productos);
            }
            catch (AccesoDatosException)
            {
                return ErrorInterno();
            }
        }

        // GET /api/productos/{id}
        [HttpGet("{id:int}")]
        public ActionResult<Producto> GetById(int id)
        {
            try
            {
                var producto = _productoService.ObtenerPorId(id);

                if (producto == null)
                {
                    return NotFound();   // 404: situación válida, el producto no existe
                }

                return Ok(producto);
            }
            catch (AccesoDatosException)
            {
                return ErrorInterno();   // 500: falla real, sin detalles internos
            }
        }

        // Respuesta genérica: no revela nada del servidor ni de la base
        private ObjectResult ErrorInterno()
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "No se pudo procesar la solicitud. Intente nuevamente más tarde." });
        }
    }
}