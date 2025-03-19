# Carga de Departamentos y Ciudades de Colombia desde CSV

Este proyecto realiza la carga de departamentos y ciudades de Colombia a una base de datos SQL Server a partir de un archivo CSV. Utiliza C# con CsvHelper para leer los datos y SqlBulkCopy para insertar eficientemente las ciudades.

## Fuente de Datos

Este proyecto utiliza información pública proporcionada por el **Departamento Administrativo Nacional de Estadística (DANE)** y el **Ministerio de Tecnologías de la Información y las Comunicaciones (MINTIC)** de Colombia. Los datos corresponden a la división político-administrativa del país e incluyen la relación entre departamentos y municipios.

La información se obtiene del conjunto de datos titulado **"Departamentos y municipios de Colombia"**, disponible en la plataforma de datos abiertos del gobierno colombiano:

🔗 **[Departamentos y municipios de Colombia](https://www.datos.gov.co/d/xdk5-pm3f)**

### Detalles del conjunto de datos:
- **Última actualización**: 20 de abril de 2024
- **Fuente**: Departamento Administrativo Nacional de Estadística (DANE)
- **Propietario**: Ministerio de Tecnologías de la Información y las Comunicaciones (MINTIC)
- **Frecuencia de actualización**: Anual
- **Licencia**: [Creative Commons Attribution | Share Alike 4.0 International](https://creativecommons.org/licenses/by-sa/4.0/)

### Estructura de los datos:
El conjunto de datos contiene **1,123 filas** y **5 columnas**, organizadas de la siguiente manera:

| Columna                           | Descripción                                  | Tipo de Dato |
|------------------------------------|----------------------------------------------|-------------|
| **REGION**                         | Nombre de la región                         | Texto       |
| **CÓDIGO DANE DEL DEPARTAMENTO**   | Código DANE del departamento                | Número      |
| **DEPARTAMENTO**                   | Nombre del departamento                     | Texto       |
| **CÓDIGO DANE DEL MUNICIPIO**       | Código DANE del municipio                   | Número      |
| **MUNICIPIO**                      | Nombre del municipio                        | Texto       |

Este código permite la transformación de estos datos en una estructura normalizada dentro de una base de datos SQL Server, almacenando los departamentos en la tabla `TERRITORIAL.STATES` y los municipios en `TERRITORIAL.CITIES`, asegurando la integridad y facilitando futuras consultas.


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

### Creación del esquema `TERRITORIAL`
Antes de crear las tablas, asegúrate de que el esquema `TERRITORIAL` exista en la base de datos. Si no existe, créalo con el siguiente comando:

```sql
CREATE SCHEMA TERRITORIAL;
GO
```

### Creación de las tablas `STATES` y `CITIES`
A continuación, se presentan las sentencias SQL para crear las tablas que almacenarán los departamentos y municipios:

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






