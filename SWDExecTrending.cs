using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Text;
using Symantec.CWoC.APIWrappers;

namespace Symantec.CWoC {
    class SWDExecTrending {
        static int Main(string[] args) {

            // Check the user is Altiris admin or not?
            if (!SecurityAPI.is_user_admin()) {
                return -1;
            }

            // Get the top 100 SWD Exec guids from the DB
            string sql = @"
select top 100 AdvertisementId
  from Evt_AeX_SWD_Execution
 where e.Start < getdate()
 group by AdvertisementName, AdvertisementId
 order by COUNT(*) desc";

            DataTable t = DatabaseAPI.GetTable(sql);
            int i = 0;

            Console.WriteLine("var object_count = {0}", t.Rows.Count.ToString());

            foreach (DataRow r in t.Rows) {
                GenerateJSONFile(r[0].ToString(), ++i);
            }

            return 0;

        }
        static void GenerateJSONFile(string advertisementid, int index) {

            string sql = @"
select AdvertisementName , DATEPART(yy, e.Start) as 'Year', DATEPART(MM, e.Start) as 'Month', DATEPART(DD, e.Start) as 'Day', DATEPART(hh, e.Start) as 'Hour', COUNT(*) as '#', ISNULL(SUM(CASE returncode WHEN 0 THEN 1 WHEN 3010 THEN 1 WHEN 1641 THEN 1 END), 0) as 'Success', SUM(CASE returncode WHEN 0 THEN 0 ELSE 1 END) as 'Error'
  from Evt_AeX_SWD_Execution e
 where e.AdvertisementId = '{0}'
   and e.Start < getdate()
 group by AdvertisementName, DATEPART(yy, e.Start), DATEPART(MM, e.Start), DATEPART(DD, e.Start), DATEPART(hh, e.Start)
 order by DATEPART(MM, e.Start) desc, DATEPART(DD, e.Start) desc, DATEPART(hh, e.Start) desc
";
            DataTable stats = DatabaseAPI.GetTable(String.Format(sql, advertisementid));
            
            string json_table = GetJSONFromTable(stats, index);

            Console.WriteLine(json_table);
        }

        public static string GetJSONFromTable(DataTable t, int index) {
            StringBuilder b = new StringBuilder();

            if (t.Rows.Count > 0) {

                b.AppendFormat("var top_{0} = ", index.ToString());
                b.AppendFormat("{{\n\t\"name\" : \"{0}\", \n", t.Rows[0][0]);
                b.AppendLine("\t\"stats\" : {\n\t\t\"hourly\" : [");

                b.AppendLine("\t\t\t[\"Date\", \"Total\", \"Success\", \"Error\"],");

                foreach (DataRow r in t.Rows) {

                    string date = (((int)r[2] > 9) ? r[2] : "0" + r[2])
                            + "-" + (((int)r[3] > 9) ? r[3] : "0" + r[3])
                            + " " + (((int)r[4] > 9) ? r[4] + "00" : "0" + r[4] + "00");

                    b.AppendLine("\t\t\t[\"" + date + "\", " + r[5] + ", " + r[6] + ", " + r[7] + "],");
                }
                // Remove the last comma we inserted
                b.Remove(b.Length - 3, 1);
                b.AppendLine("\t\t]\n\t}\n}");

                return b.ToString();
            } else {
                return "";
            }
        }

    }
}
