Trabajo Práctico Integrador - Programación 3
API web hecha con ASP.NET Core (.NET 8) y C# que consulta productos de una base SQL Server usando ADO.NET.

Alumno: Luciano Poza

Qué hace
GET /api/productos: devuelve todos los productos.
GET /api/productos/{id}: devuelve un producto, o 404 si no existe.
Si falla la conexión con la base, responde 500 con un mensaje genérico (sin mostrar datos internos).

Cómo está armado
Controller -> Service -> Repository -> SQL Server
El Controller recibe el pedido, el Service tiene la lógica y el Repository accede a la base con ADO.NET. Las capas se conectan con interfaces e inyección de dependencias.

Cómo correrlo
Hace falta el .NET 8 SDK y acceso a la base Programacion3 (tabla Productos).

Clonar el repo y entrar a la carpeta del proyecto:
git clone https://github.com/Luchopoza/Trabajo-Practico-1-Prog3.git
cd Trabajo-Practico-1-Prog3/Trabajo-Practico-1-Prog3
Cargar la connection string. La contraseña no está en el repo, se guarda con User Secrets:
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=SERVIDOR,PUERTO;Initial Catalog=Programacion3;User ID=USUARIO;Password=CONTRASEÑA;Encrypt=False;TrustServerCertificate=True;"
(Reemplazar SERVIDOR,PUERTO, USUARIO y CONTRASEÑA por los datos del servidor.)

Ejecutar y abrir Swagger en https://localhost:7052/swagger:
