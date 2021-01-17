# MSSQL

Gestor de conexiones a SQL Server.

## Uso
Importar la referencia del proyecto MSSQL, asi como el espacio de nombre ```MSSQL.Transact```, ejemplo:
~~~
using MSSQL.Transact;

    static class Program
    {
        static void Main()
        {
            MSSqlConnection Con = new MSSqlConnection
            {
                ConnectionString = "Cadena de conexion"
            };
        }
    }
~~~
---
## Clase MSSqlConnection

### StatusError
Verifica si ocurrio un error al ejecutar el SP o Query. Retorna un valor boleano.

### MessageError
Obtiene el mensaje de error, si el StatusError es verdadero.

### ConnectionString
Obtiene o establece la cadena que se utiliza para abrir una base de datos de SQL Server.

### SetCommandTimeout
Obtiene o establece el tiempo de espera (en segundos) hasta que se interrumpe el intento de ejecutar un comando y se genera un error.

### Exec(string query)
Ejecuta la Consulta en DB, recibe como parametro el query a ejecutar. Este metodo devuelve una clase ```MSSQL.Transact.MSSqlRegisters```, para poder manejar los registros (ver mas adelante).

### Exec(string storedProcedure, List<SqlParameter> parameters)
Ejecuta un Stored Procedure (SP) en DB, recibe dos parametro el primero es el nombre del SP  y el segundo son la lista de parametros del SP. Este metodo devuelve una clase ```MSSQL.Transact.MSSqlRegisters```, para poder manejar los registros (ver mas adelante).

### Exec(string query, Data.Type type)
Ejecuta una consulta en DB y devuelve una clase ```MSSQL.Transact.Data.DataGrid```. Esta Tiene dos atributos, DataSet y DataTable.
Este metodo recibe 2 parametros, el primero es la consulta y el segundo es el tipo de rellenado ```MSSQL.Transact.Data.Type```
- DataSet
- DataTable
- Both

### Exec(string storedProcedure, List<SqlParameter> parameters, Data.Type type)
Ejecuta un SP en DB y devuelve una clase ```MSSQL.Transact.Data.DataGrid```. Esta Tiene dos atributos, DataSet y DataTable.
Este metodo recibe 2 parametros, el primero es la consulta y el segundo es el tipo de rellenado ```MSSQL.Transact.Data.Type```
- DataSet
- DataTable
- Both
---

## Clase MSSqlRegisters
Esta clase se encarga de recorrer los registros

### Fetch()
Verifica si hay registros del Query o SP que se ejecuto con el metodo Exec. Retorna un valor boleano.

### StatusError
Verifica si ocurrio un error al ejecutar el SP o Query. Retorna un valor boleano.

### MessageError
Obtiene el mensaje de error, si el StatusError es verdadero.

### Record
obtiene el registro el tipo de datos es: ```SqlDataReader```



