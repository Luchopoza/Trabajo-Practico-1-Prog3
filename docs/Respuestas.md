# Respuestas - Programación 3 - TP Integrador

**Alumno:** Luciano Poza

---

## Actividad 2: Archivos del proyecto

**Program.cs**
Es el punto de entrada de la aplicación, lo primero que se ejecuta. Tiene dos partes. Antes de `builder.Build()` se registran los servicios, por ejemplo `AddControllers`, Swagger y las dependencias con `AddScoped`. Después de `Build()` se arma el pipeline, que es el camino por el que pasa cada pedido HTTP hasta llegar al Controller.

**appsettings.json**
Es el archivo de configuración de la aplicación. Guarda cosas como el nivel de los logs y la Connection String. La ventaja es que se puede cambiar la configuración sin tocar ni recompilar el código. En este trabajo tiene una connection string de ejemplo, y la real está en User Secrets para que la contraseña no se suba a GitHub.

**Carpeta Controllers**
Contiene las clases que reciben los pedidos HTTP y devuelven las respuestas. En este TP tiene `ProductosController`, que atiende `GET /api/productos` y `GET /api/productos/{id}`.

---

## Actividad 3: Modelo Producto

- **Id:** `int`, porque en SQL es `int` y es un número entero que identifica al producto.
- **Nombre:** `string`, porque es texto (en SQL es `varchar(200)`).
- **Precio:** `decimal`, porque en SQL es `decimal(10,2)` y es dinero, que necesita decimales exactos.
- **Stock:** `int`, porque son unidades enteras y no existe medio producto.

**¿Por qué Precio no debería ser `int`?**
Porque `int` no guarda decimales. Un precio como 45999,90 quedaría en 45999 y se perderían los centavos. Tampoco usamos `double`, porque tiene pequeños errores de redondeo (por ejemplo, 0.1 + 0.2 no da exactamente 0.3), y con dinero eso no se puede permitir. `decimal` es exacto y está pensado para valores monetarios.

---

## Actividad 7: Interfaces

**1. ¿Cuál es la diferencia entre una interfaz y una clase?**
Una interfaz define qué métodos se pueden usar, pero no tiene el código que los ejecuta. En cambio, una clase contiene la implementación de esos métodos, es decir, cómo funcionan. Además, no se puede crear un objeto directamente de una interfaz, pero sí de una clase.

**2. ¿Por qué conviene que el Controller dependa de IProductoService y no de ProductoService?**
Porque el Controller depende de lo que define la interfaz y no de una implementación específica. Así, si en el futuro se cambia la forma de obtener los datos, por ejemplo, de datos fijos a una base de datos SQL Server, no hace falta modificar el Controller. Esto permite tener un bajo acoplamiento y hace que el código sea más fácil de mantener.

---

## Actividad 9: Ciclos de vida

**1. ¿Qué significa Transient?**
Se crea una instancia nueva cada vez que alguien la pide. Dos clases en el mismo request reciben instancias distintas.

**2. ¿Qué significa Scoped?**
Se crea una instancia por cada pedido HTTP. Todo lo que pasa dentro de ese GET comparte la misma instancia.

**3. ¿Qué significa Singleton?**
Se crea una sola instancia para toda la aplicación, mientras esté corriendo. Todos los usuarios la comparten.

**4. ¿Cuál estamos utilizando en este trabajo?**
`AddScoped`.

**5. ¿Por qué creen que Scoped resulta apropiado para nuestro Service?**
Porque cada pedido tiene su propia instancia, así que no se mezclan datos entre usuarios. Y como el Service usa la base de datos, conviene que viva solo lo que dura el pedido y después se libere.

---

## Actividad 13: SQL Injection

**¿Qué es SQL Injection?**
Es un ataque en el que alguien envía como dato un texto que contiene código SQL. Si el programa pega ese texto directamente dentro de la consulta, el servidor ejecuta ese código como si fuera parte de la consulta original. Por ejemplo, si se arma `"SELECT * FROM Productos WHERE Nombre = '" + nombre + "'"` y el usuario escribe `' OR 1=1 --`, la condición siempre es verdadera y devuelve todos los registros. Con otros textos se podrían modificar o borrar datos.

**¿Por qué utilizar parámetros es una buena práctica?**
Porque con un parámetro, como `@Id` en nuestro Repository, el valor viaja separado del texto de la consulta. SQL Server lo trata siempre como un dato y nunca como código, así que aunque alguien mande texto malicioso no se ejecuta. Además el código queda más claro y se evitan problemas con las comillas.

---

## Actividad 17: Excepciones

**1. ¿Qué es una excepción?**
Un evento inesperado durante la ejecución que interrumpe el flujo normal del programa. En C# es un objeto (`Exception`) que se "lanza" con `throw`. Por ejemplo, en este TP, cuando la contraseña de la base estaba mal, se lanzó una `SqlException` (error 18456).

**2. ¿Qué hace try?**
Encierra el código que puede fallar. En el Repository, dentro del `try` están `connection.Open()` y `ExecuteReader()`, que son las líneas que fallan si SQL Server no responde.

**3. ¿Qué hace catch?**
Captura la excepción y permite registrar el error o responder algo controlado, en lugar de que la aplicación se caiga. En nuestro Repository, `catch (SqlException)` registra el error en el log y lanza una excepción propia con un mensaje genérico.

**4. ¿Qué hace finally?**
Se ejecuta siempre, haya error o no. Sirve para liberar recursos, como cerrar una conexión. Es lo que hace `using` por dentro.

**5. Excepción vs. error de validación**
La validación es algo esperable y se controla con un `if`. Por ejemplo, un producto inexistente se resuelve con `if (producto == null)` y responde 404. La excepción es una falla inesperada de ejecución, como SQL Server caído, y se maneja con `try/catch` y responde 500.

---

## Actividad 19: using

**Objetos del Repository que deben liberarse**
`SqlConnection`, `SqlCommand` y `SqlDataReader`. La conexión mantiene un enlace de red abierto con el servidor, y el reader, mientras está abierto, deja la conexión ocupada.

**¿Por qué se usa `using`?**
Estas clases implementan `IDisposable`, que tiene un método `Dispose()` para liberar los recursos. `using` llama a `Dispose()` automáticamente cuando termina el bloque o el método, incluso si ocurre una excepción. Si no se usara, ante un error las conexiones podrían quedar abiertas y, con muchos pedidos, el servidor se quedaría sin conexiones disponibles. En el código está como `using var connection`, `using var command` y `using var reader`, dentro de `ObtenerTodos()` y `ObtenerPorId()`.

---

## Cuestionario final

### Controllers

**¿Qué es una Web API?**
Es una aplicación que expone operaciones a través de HTTP para que otros programas las consuman. No tiene pantallas: recibe pedidos (como un GET) y responde con datos, normalmente en formato JSON.

**¿Qué función cumple un Controller?**
Recibe la solicitud HTTP, le delega el trabajo al Service y devuelve la respuesta con el código adecuado (200, 404, 500). No tiene lógica de negocio ni SQL.

**¿Qué función cumple [ApiController]?**
Indica que la clase es un controller de API y activa comportamientos automáticos, como las validaciones y las respuestas de error estándar.

**¿Qué función cumple [Route]?**
Define la URL a la que responde el controller. Con `[Route("api/[controller]")]`, el `[controller]` se reemplaza por el nombre de la clase sin "Controller", así que la ruta queda `api/productos`.

**¿Qué función cumple [HttpGet]?**
Indica que ese método atiende los pedidos HTTP de tipo GET, o sea las consultas.

### Models

**¿Qué representa la clase Producto?**
Representa un producto de la tabla `Productos`, con sus cuatro datos: Id, Nombre, Precio y Stock. Cada objeto `Producto` equivale a una fila de la tabla.

**¿Por qué utilizamos una clase para representar los datos de un producto?**
Porque agrupa todos los datos del producto en un solo objeto con tipos claros. Además el compilador nos avisa si cometemos errores y ASP.NET Core la convierte a JSON automáticamente.

### Services

**¿Qué responsabilidad tiene un Service?**
Implementar la lógica de la aplicación y coordinar el acceso a los datos. Recibe los pedidos del Controller y le pide los datos al Repository.

**¿Por qué no colocamos toda la lógica dentro del Controller?**
Porque se mezclarían responsabilidades. El Controller debe ocuparse solo de HTTP. Si además tuviera la lógica y el SQL, sería una clase enorme, difícil de mantener, de reutilizar y de probar.

### Interfaces

**¿Qué es una interfaz?**
Es un contrato que define qué métodos tiene que tener una clase, sin decir cómo se implementan. Por ejemplo, `IProductoService` dice que existen `ObtenerTodos()` y `ObtenerPorId()`.

**¿Qué diferencia existe entre una interfaz y una clase?**
La interfaz solo tiene las firmas de los métodos, no tiene código y no se puede instanciar. La clase tiene la implementación y sí se puede instanciar. Además, una clase puede implementar varias interfaces.

**¿Por qué utilizamos IProductoService?**
Para que el Controller dependa del contrato y no de la clase concreta. En nuestro TP pasamos de un Service con datos fijos a uno que lee de SQL Server, y el Controller no cambió ni una línea.

### Inyección de Dependencias

**¿Qué es una dependencia?**
Es cuando una clase necesita a otra para poder hacer su trabajo. El Controller depende del Service y el Service depende del Repository.

**¿Qué es la Inyección de Dependencias?**
Es un mecanismo por el cual ASP.NET Core crea las dependencias y se las entrega a las clases por el constructor, en lugar de que cada clase las cree con `new`.

**¿Qué problema resuelve?**
Evita que las clases queden atadas a implementaciones concretas. Reduce el acoplamiento, facilita cambiar una implementación por otra y permite usar versiones de prueba.

**¿Qué significa AddScoped?**
Que el contenedor crea una instancia nueva por cada pedido HTTP y la comparte dentro de ese pedido. Se registra en `Program.cs`, por ejemplo: `AddScoped<IProductoService, ProductoService>()`.

**¿Qué diferencia existe entre Transient, Scoped y Singleton?**
Transient crea una instancia nueva cada vez que se pide. Scoped crea una por cada pedido HTTP. Singleton crea una sola para toda la aplicación y todos la comparten.

### Repository

**¿Qué responsabilidad tiene un Repository?**
Acceder a los datos. Ejecuta las consultas SQL y convierte las filas de la base en objetos `Producto`.

**¿Por qué el Repository es independiente del Controller?**
Porque no sabe nada de HTTP, solo trabaja con datos. Por eso se puede cambiar o reutilizar sin tocar el Controller.

**¿Por qué el Service no debería contener directamente el código SQL?**
Para separar responsabilidades. Si mañana cambia la forma de acceder a los datos, solo hay que modificar el Repository y el Service queda igual.

### ADO.NET

**¿Qué es ADO.NET?**
Es el conjunto de clases de .NET que permite conectarse a una base de datos y ejecutar consultas. Para SQL Server usamos el paquete `Microsoft.Data.SqlClient`.

**¿Qué función cumple SqlConnection?**
Representa la conexión con SQL Server. Con `Open()` se abre y, al terminar, se cierra.

**¿Qué función cumple SqlCommand?**
Representa la consulta SQL que se ejecuta sobre la conexión. También lleva los parámetros, como `@Id`.

**¿Qué función cumple SqlDataReader?**
Lee los resultados de la consulta fila por fila, hacia adelante. `Read()` pasa a la siguiente fila y devuelve `false` cuando no quedan más.

**¿Qué función cumple una Connection String?**
Indica cómo conectarse a la base: el servidor y el puerto, la base de datos, el usuario, la contraseña y otras opciones.

### SQL

**¿Por qué utilizamos parámetros SQL?**
Para que el valor viaje separado de la consulta y se trate siempre como un dato. Así se evita la SQL Injection.

**¿Qué es SQL Injection?**
Es insertar código SQL malicioso a través de un dato para que se ejecute en la base de datos.

**¿Qué problema podría existir al concatenar directamente valores dentro de una consulta SQL?**
Que un valor malicioso puede modificar la consulta original y permitir leer, modificar o borrar datos que no corresponden.

### Excepciones

**¿Qué es una excepción?**
Un evento inesperado durante la ejecución que interrumpe el flujo normal del programa.

**¿Qué función cumple try?**
Encierra el código que puede fallar.

**¿Qué función cumple catch?**
Captura la excepción para tratarla sin que la aplicación se caiga.

**¿Qué función cumple finally?**
Se ejecuta siempre, haya error o no, y sirve para liberar recursos.

**¿Por qué no debemos mostrar al usuario los detalles internos de una excepción de base de datos?**
Porque esos detalles pueden revelar la connection string, nombres de tablas o versiones del servidor. Esa información le sirve a un atacante y al usuario común no le sirve de nada. Por eso el detalle se guarda en el log del servidor y el usuario ve solo un mensaje genérico. Nos pasó con el error de contraseña: antes la API mostraba todo el detalle técnico y ahora devuelve un mensaje neutro.

**¿Qué diferencia existe entre un producto inexistente y una falla de conexión con SQL Server?**
Un producto inexistente es una situación válida de la aplicación: se controla con un `if` y se responde 404. Una falla de conexión es un problema inesperado de ejecución: se maneja con `try/catch` y se responde 500.

### Arquitectura

**Recorrido completo de GET /api/productos/5**
El pedido llega al `ProductosController`, que toma el 5 de la URL y llama a `IProductoService.ObtenerPorId(5)`. El `ProductoService` llama a `IProductoRepository.ObtenerPorId(5)`. El `ProductoRepository` abre una `SqlConnection`, crea un `SqlCommand` con `SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id` y le pasa el 5 como parámetro. Después ejecuta la consulta, lee la fila con `SqlDataReader` y arma un objeto `Producto`; si no hay fila, devuelve `null`. El resultado vuelve por el Service hasta el Controller, que responde 200 con el producto en JSON, o 404 si fue `null`. Si falla la base, el Repository registra el error en el log y lanza una `AccesoDatosException`, y el Controller responde 500 con un mensaje genérico.

**Responsabilidad de cada componente**
- **Controller:** recibe el pedido HTTP y devuelve la respuesta.
- **Service:** la lógica de la aplicación.
- **Repository:** el acceso a los datos.
- **ADO.NET:** la comunicación técnica con SQL Server (conexión, comando y lectura).
- **SQL Server:** almacena los datos.

**¿Qué ventajas tiene separar estas responsabilidades?**
El código queda más ordenado y fácil de mantener. Los cambios afectan a una sola capa, los errores se encuentran más rápido y cada parte se puede probar por separado.

**¿Qué problemas podrían aparecer si colocáramos todo el código en ProductosController?**
Tendríamos una clase enorme que mezcla HTTP, lógica y SQL. Cualquier cambio en la base obligaría a modificar el Controller, el código no se podría reutilizar ni probar por partes y sería muy difícil de mantener.

---

## Reflexión final

**¿Qué ventajas encontraron al separar la aplicación en Controller, Service y Repository en lugar de colocar todo el código en una única clase?**
Cada clase tiene una sola tarea, así que el código es más claro y los cambios quedan acotados. Lo comprobamos en el trabajo: pasamos de datos fijos a SQL Server y el Controller no se modificó. También nos ayudó a encontrar errores más rápido, porque sabíamos en qué capa buscar.

**¿Qué relación existe entre la separación de responsabilidades y la Inyección de Dependencias?**
Al separar en capas, unas clases pasan a depender de otras. La Inyección de Dependencias permite conectarlas sin que se conozcan las clases concretas, solo las interfaces: el contenedor crea el Repository, se lo da al Service y el Service al Controller. Sin inyección, cada capa tendría que crear a la siguiente con `new` y quedarían todas atadas entre sí.

**¿Qué parte de la aplicación cambiarían si mañana se reemplazara ADO.NET por otra tecnología de acceso a datos?**
Solo `ProductoRepository` y su registro en `Program.cs`. El Controller y el Service no cambian porque dependen de interfaces (`IProductoService` e `IProductoRepository`), no de ADO.NET. Por eso la separación y las interfaces valen la pena.
