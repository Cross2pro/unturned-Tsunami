using System.Collections.Generic;
using System.Data.SqlClient;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;

namespace TsunamiHack.Tsunami.Util
{
    internal class Db
    {
        private const string ds = "tidal.database.windows.net";
        private const string un = "genericuser";
        private const string pw = "tzIhX@3E3T";
        private const string prem = "premium";
        private const string ban = "ban";
        private const string beta = "beta";
        private const string controller = "controller";

        internal static void GetAll(out PremiumList premiumList, out BanList banlist, out BetaList betaList)
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
  
        }

        internal static void GetController(out HackController ctrl)
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
        }
    }
}






























