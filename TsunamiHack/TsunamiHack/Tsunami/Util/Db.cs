using System;
using System.Collections.Generic;
using System.Globalization;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
// ReSharper disable InconsistentNaming


namespace TsunamiHack.Tsunami.Util
{
    internal class Db
    {
        /*private const string ds = "tidal.database.windows.net";
        private const string un = "genericuser";
        private const string pw = "tzIhX@3E3T";
        private const string prem = "premium";
        private const string ban = "ban";
        private const string beta = "beta";
        private const string controller = "controller";
        private const string users = "users";*/

       /* internal static void GetAll(out PremiumList premiumList, out BanList banlist, out BetaList betaList)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ds;
            builder.UserID = un;
            builder.Password = pw;
            builder.InitialCatalog = "TsunamiUnturned";

            premiumList = new PremiumList();
            premiumList.UserList = new List<string>();

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                
                connection.Open();
                var cmd = $"SELECT steamid FROM {prem}";

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            premiumList.UserList.Add(reader["steamid"].ToString());
                        }
                    }
                }
            }

            banlist = new BanList();
            banlist.UserList = new List<string>();

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var cmd = $"SELECT steamid FROM {ban}";

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            banlist.UserList.Add(reader["steamid"].ToString());
                        }
                    }
                }
            }
            
            betaList = new BetaList();
            betaList.UserList = new List<string>();

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var cmd = $"SELECT steamid FROM {beta}";

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            betaList.UserList.Add(reader["steamid"].ToString());
                        }
                    }
                }
            }
  
        }*/

       /* internal static void GetController(out HackController ctrl)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ds;
            builder.UserID = un;
            builder.Password = pw;
            builder.InitialCatalog = "TsunamiUnturned";
            
            ctrl = new HackController();

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var cmd = $"SELECT * FROM {controller}";

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["status"].ToString() == "1")
                                ctrl.Disabled = true;
                            else
                                ctrl.Disabled = false;

                            ctrl.Reason = reader["reason"].ToString();
                            ctrl.AuthorizedBy = reader["authorizer"].ToString();
                            ctrl.Version = reader["version"].ToString();
                            ctrl.Dev = ulong.Parse(reader["dev"].ToString());
                        }
                    }
                }
            }
        }*/

       /* internal static void CheckUsers(ulong id, string name)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ds;
            builder.UserID = un;
            builder.Password = pw;
            builder.InitialCatalog = "TsunamiUnturned";

            var list = new List<string>();
            
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var cmd = $"SELECT * FROM {users} where steamid = '{id}'";

                using (var command = new SqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["steamid"].ToString());
                        }
                    }
                }
            }

            if (list.Count == 0)
            {
                var ip = DataCollector.GetIp();

                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    var cmd = $"INSERT INTO users (steamname, steamid, ipaddress, lastuse, uses) values (@0, @1, @2, @3, @4)";

                    using (var command = new SqlCommand(cmd, connection))
                    {
                        command.Parameters.AddWithValue("@0", $"{name}");
                        command.Parameters.AddWithValue("@1", $"{id}");
                        command.Parameters.AddWithValue("@2", $"{ip}");
                        command.Parameters.AddWithValue("@3",
                            $"'{DateTime.Now.ToString(CultureInfo.InvariantCulture)}'");
                        command.Parameters.AddWithValue("@4", "1");

                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                var use = 999;
                
                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    var cmd = $"SELECT * FROM {users} where steamid = '{id}'";

                    using (var command = new SqlCommand(cmd, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                use = int.Parse(reader["uses"].ToString());
                            }
                        }
                    }
                }
                
                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    var newuse = use + 1;
                    
                    var cmd = $"update users set lastuse = '{DateTime.Now}', uses = '{newuse}' where steamid = '{id}'";

                    using (var command = new SqlCommand(cmd, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
                
        }*/
    }
}






























