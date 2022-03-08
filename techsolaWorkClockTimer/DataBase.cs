using System;
using System.Data.SqlClient;
using Dapper;

namespace techsolaWorkClockTimer
{
    public static class DataBase
    {
#if DEBUG
        public static SqlConnection Connection = new SqlConnection(@"Server=localhost; Database=techsolaclockdev; Integrated Security=True;");
#else
        public static SqlConnection Connection = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");
#endif
        public static bool IsDataFromPriorDay()
        {
            var latestDbDateTime = Connection.QuerySingle<DateTime>("select top 1 TimeSegmentEnd from segments order by TimeSegmentEnd desc");
            Connection.Close();

            return latestDbDateTime.Day < DateTime.Now.Day;
        }
        public static void RefreshTable()
        {
            var refreshSegmentsTable = Connection.Execute("delete from segments;");

            Connection.Close();
        }

        public static bool DoesTableContainData()
        {
            var isDataPresent = Connection.ExecuteScalar("select count(*) from segments");
            
            return (int)isDataPresent > 0;
        }
    }
}