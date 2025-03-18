# Carga de Departamentos y Ciudades de Colombia desde CSV

Este proyecto realiza la carga de departamentos y ciudades de Colombia a una base de datos SQL Server a partir de un archivo CSV. Utiliza C# con CsvHelper para leer los datos y SqlBulkCopy para insertar eficientemente las ciudades.

## 📂 Estructura del CSV
El archivo CSV debe contener los siguientes campos:

```
REGION,CÓDIGO DANE DEL DEPARTAMENTO,DEPARTAMENTO,CÓDIGO DANE DEL MUNICIPIO,MUNICIPIO
Región Eje Cafetero - Antioquia,5,Antioquia,5.001,Medellín
Región Eje Cafetero - Antioquia,5,Antioquia,5.002,Abejorral
Región Centro Oriente,15,Boyacá,15.832,Tununguá
```

## 🛠️ Tecnología Utilizada
- **C# .NET** para la implementación del cargador de datos.
- **CsvHelper** para la lectura del archivo CSV.
- **SQL Server** como base de datos destino.
- **SqlBulkCopy** para inserciones masivas de ciudades.

## 📌 Esquema de Base de Datos
```sql
CREATE TABLE TERRITORIAL.STATES (
    StateId     UNIQUEIDENTIFIER DEFAULT (NEWID()) NOT NULL,
    StateCode   VARCHAR(10) NOT NULL,
    Name        VARCHAR(100) NOT NULL,
    CONSTRAINT PK_States PRIMARY KEY (StateId),
    CONSTRAINT UQ_States_StateCode UNIQUE (StateCode)
);

CREATE TABLE TERRITORIAL.CITIES (
    CityId      UNIQUEIDENTIFIER DEFAULT (NEWID()) NOT NULL,
    CityCode    VARCHAR(10) NOT NULL,
    Name        VARCHAR(100) NOT NULL,
    StateId     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_Cities PRIMARY KEY (CityId),
    CONSTRAINT FK_Cities_States FOREIGN KEY (StateId)
        REFERENCES TERRITORIAL.STATES (StateId) ON DELETE CASCADE,
    CONSTRAINT UQ_Cities_CityCode UNIQUE (CityCode)
);
```

## 🚀 Flujo de Carga de Datos
1. **Leer el CSV** usando CsvHelper.
2. **Crear un diccionario de departamentos** para evitar duplicados.
3. **Insertar los departamentos en la base de datos**.
4. **Realizar una carga masiva de las ciudades** utilizando SqlBulkCopy.

## 🔧 Configuración y Ejecución
1. Instalar dependencias:
   ```sh
   dotnet add package CsvHelper
   ```
2. Modificar la conexión a la base de datos en `connectionString`.
3. Ejecutar el programa:
   ```sh
   dotnet run
   ```

## 📢 Notas
- Se asegura que no haya duplicados mediante restricciones `UNIQUE` en `StateCode` y `CityCode`.
- Se utiliza `GUID` para los identificadores primarios.

¡Carga de datos completada con éxito! 🎉

