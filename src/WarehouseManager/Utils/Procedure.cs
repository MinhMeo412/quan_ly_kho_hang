using MySql.Data.MySqlClient;

namespace MySQLUtility
{
    class Procedure
    {
        /**
        <summary>
        Executes a stored procedure and returns the number of affected rows.
        </summary>
        <param name="connectionString">Example: "server=localhost; user=root; password=1234; database=DatabaseName"</param>
        <param name="procedure">Name of the procedure to be executed.</param>
        <param name="parameters">Dictionary with parameter name as keys and their respective arguments as values.</param>
        <returns>int (number of affected rows).</returns>
        */
        public static int ExecuteNonQuery(string connectionString, string procedure, Dictionary<string, object> parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(procedure, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (string parameter in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                }

                return cmd.ExecuteNonQuery();
            }
        }

        /**
        <summary>
        Executes a stored procedure and returns the queried table as a list of lists of objects.
        </summary>
        <param name="connectionString">Example: "server=localhost; user=root; password=1234; database=DatabaseName"</param>
        <param name="procedure">Name of the procedure to be executed.</param>
        <param name="parameters">Dictionary with parameter name as keys and their respective arguments as values.</param>
        <returns> A list containing each row of the table as a sublist.</returns>
        */
        public static List<List<object>> ExecuteReader(string connectionString, string procedure, Dictionary<string, object> parameters)
        {
            List<List<object>> rows = new List<List<object>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(procedure, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (string parameter in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                }

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetValue(i));
                        }
                        rows.Add(row);
                    }
                }
            }

            return rows;
        }
    }
}