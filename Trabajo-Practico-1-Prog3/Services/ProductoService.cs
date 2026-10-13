using Trabajo_Practico_1_Prog3.Data;
using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    // El Repository llega por inyección de dependencias
    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public IEnumerable<Producto> ObtenerTodos()
    {
        return _productoRepository.ObtenerTodos();
    }

    public Producto? ObtenerPorId(int id)
    {
        return _productoRepository.ObtenerPorId(id);
    }
}