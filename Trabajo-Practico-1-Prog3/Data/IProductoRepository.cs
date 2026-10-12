using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Data;

public interface IProductoRepository
{
    IEnumerable<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
}