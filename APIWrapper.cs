using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Altiris.Database;
using Altiris.NS.ContextManagement;
using Altiris.NS.Security;

namespace Symantec.CWoC.APIWrappers
{
    class SecurityAPI
    {
        public static bool user_is_admin()
        {
            bool is_altiris_admin = false;
            string identity = string.Empty;

            try
            {
                SecurityContextManager.SetContextData();
                Role role = SecurityRoleManager.Get(new Guid("{2E1F478A-4986-4223-9D1E-B5920A63AB41}"));
                if (role != null)
                    identity = role.Trustee.Identity;

                if (identity != string.Empty)
                {
                    foreach (string admin in SecurityTrusteeManager.GetCurrentUserMemberships())
                    {
                        if (admin == identity)
                        {
                            is_altiris_admin = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            return is_altiris_admin;
        }
    }

    class DatabaseAPI {
        public static DataTable GetTable(string sqlStatement) {
            DataTable t = new DataTable();
            try {
                using (DatabaseContext context = DatabaseContext.GetContext()) {
                    SqlCommand cmdAllResources = context.CreateCommand() as SqlCommand;
                    cmdAllResources.CommandText = sqlStatement;

                    using (SqlDataReader r = cmdAllResources.ExecuteReader()) {
                        t.Load(r);
                    }
                }
                return t;
            } catch {
                throw new Exception("Failed to execute SQL command...");
            }
        }

        public static int ExecuteNonQuery(string sqlStatement) {
            using (DatabaseContext context = DatabaseContext.GetContext()) {
                SqlCommand sql_cmd = context.CreateCommand() as SqlCommand;
                sql_cmd.CommandText = sqlStatement;

                return sql_cmd.ExecuteNonQuery();
            }
        }

        public static int ExecuteScalar(string sqlStatement) {
            using (DatabaseContext context = DatabaseContext.GetContext()) {
                SqlCommand cmd = context.CreateCommand() as SqlCommand;

                cmd.CommandText = sqlStatement;
                Object result = cmd.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }
    }

    class Timer {
        private static Stopwatch chrono;

        public static void Init() {
            chrono = new Stopwatch();
            chrono.Start();
        }

        public static void Start() {
            chrono.Start();
        }
        public static void Stop() {
            chrono.Stop();
        }
        public static string tickCount() {
            return chrono.ElapsedTicks.ToString();
        }
        public static string duration() {
            return chrono.ElapsedMilliseconds.ToString();
        }
    }
}
