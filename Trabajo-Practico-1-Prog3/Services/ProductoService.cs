using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Services;

public class ProductoService : IProductoService
{
    private readonly List<Producto> _productos = new()
    {
        new Producto { Id = 1, Nombre = "Producto de prueba A", Precio = 100.50m, Stock = 10 },
        new Producto { Id = 2, Nombre = "Producto de prueba B", Precio = 250.00m, Stock = 5 }
    };

    public IEnumerable<Producto> ObtenerTodos()
    {
        return _productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        return _productos.FirstOrDefault(p => p.Id == id);
    }
}