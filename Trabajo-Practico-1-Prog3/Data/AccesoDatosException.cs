namespace Trabajo_Practico_1_Prog3.Data;

// Excepción propia: indica que falló el acceso a datos,
// sin exponer detalles de SQL Server a quien la reciba.
public class AccesoDatosException : Exception
{
    public AccesoDatosException(string mensaje, Exception innerException)
        : base(mensaje, innerException)
    {
    }
}