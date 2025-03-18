using CsvHelper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

class Program
{
    static void Main()
    {
        string csvPath = "data.csv";
        string connectionString = "tu_conexion_sql_server";

        var states = new Dictionary<string, (Guid Id, string Name)>();
        var cities = new List<(Guid, string, string)>();

        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                string stateCode = csv.GetField("CÓDIGO DANE DEL DEPARTAMENTO")!;
                string stateName = csv.GetField("DEPARTAMENTO")!;
                string cityCode = csv.GetField("CÓDIGO DANE DEL MUNICIPIO")!;
                string cityName = csv.GetField("MUNICIPIO")!;

                if (!states.ContainsKey(stateCode))
                {
                    states[stateCode] = (Guid.NewGuid(), stateName);
                }

                cities.Add((states[stateCode].Id, cityCode, cityName));
            }
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Insertar Estados
            foreach (var state in states)
            {
                using (var cmd = new SqlCommand("INSERT INTO TERRITORIAL.STATES (StateId, StateCode, Name) VALUES (@id, @code, @name)", connection))
                {
                    cmd.Parameters.AddWithValue("@id", state.Value.Id);
                    cmd.Parameters.AddWithValue("@code", state.Key);
                    cmd.Parameters.AddWithValue("@name", state.Value.Name); // 🔹 Usa el nombre correcto
                    cmd.ExecuteNonQuery();
                }
            }

            // Insertar Ciudades
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = "TERRITORIAL.CITIES";
                bulkCopy.ColumnMappings.Add("CityId", "CityId");
                bulkCopy.ColumnMappings.Add("CityCode", "CityCode");
                bulkCopy.ColumnMappings.Add("Name", "Name");
                bulkCopy.ColumnMappings.Add("StateId", "StateId");

                using (var table = new DataTable())
                {
                    table.Columns.Add("CityId", typeof(Guid));
                    table.Columns.Add("CityCode", typeof(string));
                    table.Columns.Add("Name", typeof(string));
                    table.Columns.Add("StateId", typeof(Guid));

                    foreach (var city in cities)
                    {
                        table.Rows.Add(Guid.NewGuid(), city.Item2, city.Item3, city.Item1);
                    }

                    bulkCopy.WriteToServer(table);
                }
            }
        }

        Console.WriteLine("Migración completada con éxito 🚀");
    }
}
