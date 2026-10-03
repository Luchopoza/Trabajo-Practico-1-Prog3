using Microsoft.AspNetCore.Mvc;
using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetAll()
        {
            // Datos fijos SOLO para probar la ruta.
            // Más adelante esto lo va a traer el Service desde la base.
            var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Producto de prueba A", Precio = 100.50m, Stock = 10 },
                new Producto { Id = 2, Nombre = "Producto de prueba B", Precio = 250.00m, Stock = 5 }
            };

            return Ok(productos);
        }
    }
}