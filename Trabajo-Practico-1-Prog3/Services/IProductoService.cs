using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Services;

public interface IProductoService
{
    IEnumerable<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
}