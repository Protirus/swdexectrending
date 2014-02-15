using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Text;
using Symantec.CWoC.APIWrappers;

namespace Symantec.CWoC {
    class SWDExecTrending {

        public static readonly int version = 1;
        public static readonly string version_message = @"
SWD Execution Trending - JavaScript builder, version " + version.ToString();
        public static readonly string usage = version_message + @"
Usage:

    Invoke this tool with no command line arguments. It will automatically read
    the content of EvT_AeX_SWD_Execution and will produce a JavaScript string
    to stdout.

    The javascript defines a global variable named object_count that matches the
    count of Policy Execution for which trending data was generated (100 max).

    Each policy execution trend data will match the following outline:

        var top_n = { // where n is the row id
            ""name"" : ""<SWD Policy name>"",
            "" stats"" : {
                ""hourly"" : [
                    [""Date"", ""Total"", ""Success"", ""Error""],
                    [ ... ], // Hourly statistics for the Policy
                    [ ... ],
                    [ ... ]
                ]
            }
        }

    The policies are order by execution count in descending order, limited to
    100 at most. The SQL was adjusted to take care of special cases:

        * Data that indicates the Policy execution took place in the future
            is not included
        * Data going back more than 30 days is not included

    The first is to avoid computers that are operating with an invalid date
    from disrupting the trend graphs, the later is to avoid computers coming
    back from long period of inactivity from disrupting the trend graphs (this
    is especially needed for SWD policies such as the ""Windows System Asses-
    -sment Scan"" that is largely deployed and almost always running on
    computers.

    The program returns 0 if it produces any JavaScript output.
    The program returns 1 if it prints out command line help or version message.
    The program returns -1 if the user is not member of the Altiris Admini-
        -strator group.
";

        static int Main(string[] args) {


            if (args.Length > 0) {
                if (args[0].ToLower() == "/version" || args[0].ToLower() == "--version") {
                    Console.WriteLine(version_message);
                } else {
                    Console.WriteLine(usage);
                }
                return 1;
            }

            // Check the user is Altiris admin or not?
            if (SecurityAPI.user_is_admin() == false) {
                return -1;
            }

            // Get the top 100 SWD Exec guids from the DB
            string sql = @"
select top 100 AdvertisementId
  from Evt_AeX_SWD_Execution e
 where e.Start < getdate()
 group by AdvertisementName, AdvertisementId
 order by COUNT(*) desc";

            DataTable t = DatabaseAPI.GetTable(sql);
            int i = 0;

            Console.WriteLine("var object_count = {0}", t.Rows.Count.ToString());

            foreach (DataRow r in t.Rows) {
                Console.WriteLine(GenerateJSString(r[0].ToString(), ++i));
            }

            return 0;

        }
        static string GenerateJSString(string advertisementid, int index) {

            string sql = @"
select AdvertisementName , DATEPART(yy, e.Start) as 'Year', DATEPART(MM, e.Start) as 'Month', DATEPART(DD, e.Start) as 'Day', DATEPART(hh, e.Start) as 'Hour', COUNT(*) as '#', ISNULL(SUM(CASE returncode WHEN 0 THEN 1 WHEN 3010 THEN 1 WHEN 1641 THEN 1 END), 0) as 'Success', SUM(CASE returncode WHEN 0 THEN 0 WHEN 3010 THEN 0 WHEN 1641 THEN 0 ELSE 1 END) as 'Error'
  from Evt_AeX_SWD_Execution e
 where e.AdvertisementId = '{0}'
   and e.Start < getdate()
   and e.Start > getdate() - 30
 group by AdvertisementName, DATEPART(yy, e.Start), DATEPART(MM, e.Start), DATEPART(DD, e.Start), DATEPART(hh, e.Start)
 order by DATEPART(yy, e.Start) desc, DATEPART(MM, e.Start) desc, DATEPART(DD, e.Start) desc, DATEPART(hh, e.Start) desc
";
            DataTable stats = DatabaseAPI.GetTable(String.Format(sql, advertisementid));
            return GetJSONFromTable(stats, index);
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
