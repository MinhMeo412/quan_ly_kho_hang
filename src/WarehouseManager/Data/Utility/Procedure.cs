using MySql.Data.MySqlClient;

namespace WarehouseManager.Data.Utility
{
    static class Procedure
    {
        /**
        <summary>
        Executes a stored procedure and returns the number of affected rows.
        </summary>
        <param name="connectionString">Example: "server=localhost; user=root; password=1234; database=DatabaseName"</param>
        <param name="procedure">Name of the procedure to be executed.</param>
        <param name="inParameters">Dictionary with in parameter name as keys and their respective arguments as values.</param>
        <param name="outParameters">List of out parameters.</param>
        <returns>key value pairs of the procedure's output</returns>
        */
        public static Dictionary<string, object> ExecuteNonQuery(string connectionString, string procedure, Dictionary<string, object>? inParameters = null, List<string>? outParameters = null)
        {
            inParameters ??= new Dictionary<string, object>();
            outParameters ??= new List<string>();

            Dictionary<string, object> output = new Dictionary<string, object>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(procedure, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (string inParameter in inParameters.Keys)
                {
                    cmd.Parameters.AddWithValue(inParameter, inParameters[inParameter]);
                }

                foreach (string outParameter in outParameters)
                {
                    cmd.Parameters.Add(new MySqlParameter(outParameter, MySqlDbType.VarChar)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    });
                }

                cmd.ExecuteNonQuery();

                foreach (string outParameter in outParameters)
                {
                    output.Add(outParameter, cmd.Parameters[outParameter].Value);
                }

                return output;
            }
        }

        /**
        <summary>
        Executes a stored procedure and returns the queried table as a list of lists of objects.
        </summary>
        <param name="connectionString">Example: "server=localhost; user=root; password=1234; database=DatabaseName"</param>
        <param name="procedure">Name of the procedure to be executed.</param>
        <param name="inParameters">Dictionary with parameter name as keys and their respective arguments as values.</param>
        <returns> A list containing each row of the table as a sublist.</returns>
        */
        public static List<List<object>> ExecuteReader(string connectionString, string procedure, Dictionary<string, object>? inParameters = null)
        {
            inParameters ??= new Dictionary<string, object>();
            List<List<object>> rows = new List<List<object>>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(procedure, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (string inParameter in inParameters.Keys)
                {
                    cmd.Parameters.AddWithValue(inParameter, inParameters[inParameter]);
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