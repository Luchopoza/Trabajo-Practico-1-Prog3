using Microsoft.Data.SqlClient;
using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Data;

public class ProductoRepository : IProductoRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ProductoRepository> _logger;

    public ProductoRepository(IConfiguration configuration, ILogger<ProductoRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontró la Connection String 'DefaultConnection'.");
        _logger = logger;
    }

    public IEnumerable<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();
        const string sql = "SELECT Id, Nombre, Precio, Stock FROM Productos";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                productos.Add(MapearProducto(reader));
            }

            return productos;
        }
        catch (SqlException ex)
        {
            // El detalle técnico queda en el log del servidor, no se muestra al usuario
            _logger.LogError(ex, "Error de SQL Server al obtener todos los productos.");
            throw new AccesoDatosException("No se pudo acceder a los datos de productos.", ex);
        }
    }

    public Producto? ObtenerPorId(int id)
    {
        const string sql = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();

            return reader.Read() ? MapearProducto(reader) : null;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error de SQL Server al obtener el producto con Id {Id}.", id);
            throw new AccesoDatosException("No se pudo acceder a los datos del producto.", ex);
        }
    }

    private static Producto MapearProducto(SqlDataReader reader)
    {
        return new Producto
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
        };
    }
}