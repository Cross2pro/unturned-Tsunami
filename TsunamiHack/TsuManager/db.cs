using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace TsuManager
{
    public class db
    {
        private const string ds = "tidal.database.windows.net";
        private const string un = "genericuser";
        private const string pw = "tzIhX@3E3T";

        private MainWindow _mainWindow;
        
        public db(MainWindow parent)
        {
            _mainWindow = parent;
        }
        
        internal void GetData()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ds;
            builder.UserID = un;
            builder.Password = pw;
            builder.InitialCatalog = "TsunamiUnturned";

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                var cmd = "select * from installer where id = '1'";
                connection.Open();

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _mainWindow.MenuText = reader["text"].ToString();
                        }
                    }
                }
            }
            
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                var cmd = "select * from controller where dev = '76561198129863498'";
                connection.Open();

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bl = reader["status"].ToString();
                            _mainWindow.Disabled = bl == "0";
                            _mainWindow.Reason = reader["reason"].ToString();
                        }
                    }
                }
                
                
            }
        }
        
    }
}