namespace INTERNAL_SOURCE_LOAD.Services
{
    using MySql.Data.MySqlClient;

    public class MariaDbExecutor : ISqlExecutor
    {
        private readonly string _connectionString;

        public MariaDbExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes a SQL query on the MariaDB database.
        /// </summary>
        /// <param name="sqlQuery">The SQL query to execute.</param>
        public void Execute(string sqlQuery)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand(sqlQuery, connection);
            command.ExecuteNonQuery();
        }
    }
}
