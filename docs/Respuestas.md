1. ¿Cuál es la diferencia entre una interfaz y una clase?
Una interfaz define qué métodos se pueden usar, pero no tiene el código que los ejecuta. En cambio, una clase contiene la implementación de esos métodos, es decir, cómo funcionan. Además, no se puede crear un objeto directamente de una interfaz, pero sí de una clase.
2. ¿Por qué conviene que el Controller dependa de IProductoService y no de ProductoService?
Porque el Controller depende de lo que define la interfaz y no de una implementación específica. Así, si en el futuro se cambia la forma de obtener los datos, por ejemplo, de datos fijos a una base de datos SQL Server, no hace falta modificar el Controller. Esto permite tener un bajo acoplamiento y hace que el código sea más fácil de mantener.

Actividad 9
Responder:
1.	¿Qué significa Transient?
Cuándo se crea una instancia cada vez que alguien la pide. Dos clases en el mismo request reciben instancias distintas.
2.	¿Qué significa Scoped?
Cuándo se crea una instancia una por cada pedido HTTP. Todo lo que pasa dentro de ese GET comparte la misma instancia.
3.	¿Qué significa Singleton?
Cuándo se crea una instancia una sola para toda la aplicación, mientras esté corriendo. Todos los usuarios la comparten
4.	¿Cuál estamos utilizando en este trabajo?
AddScoped
5.	¿Por qué creen que Scoped resulta apropiado para nuestro Service?
Porque cada pedido tiene su propia instancia, así que no se mezclan datos entre usuarios. Y cuando el Service use la base de datos, conviene que viva solo lo que dura el pedido y después se libere