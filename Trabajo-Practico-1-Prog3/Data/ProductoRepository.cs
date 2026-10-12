using Microsoft.Data.SqlClient;
using Trabajo_Practico_1_Prog3.Models;

namespace Trabajo_Practico_1_Prog3.Data;

public class ProductoRepository : IProductoRepository
{
    private readonly string _connectionString;

    // Recibe la configuración por inyección de dependencias y lee la Connection String
    public ProductoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontró la Connection String 'DefaultConnection'.");
    }

    public IEnumerable<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();
        const string sql = "SELECT Id, Nombre, Precio, Stock FROM Productos";

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

    public Producto? ObtenerPorId(int id)
    {
        const string sql = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);   // Parámetro SQL: evita SQL Injection

        connection.Open();
        using var reader = command.ExecuteReader();

        return reader.Read() ? MapearProducto(reader) : null;
    }

    // Convierte una fila de SQL Server en un objeto Producto
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