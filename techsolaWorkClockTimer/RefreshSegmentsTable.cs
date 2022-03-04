using System;
using System.Data.SqlClient;
using Dapper;

namespace techsolaWorkClockTimer
{
    public static class RefreshSegmentsTable
    {
        public static bool IsDataFromPriorDay()
        {
            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");

            var latestDbDateTime = cnn.QuerySingle<DateTime>("select top 1 TimeSegmentEnd from segments order by TimeSegmentEnd desc");
            cnn.Close();

            return latestDbDateTime.Day < DateTime.Now.Day;
        }
        public static void RefreshTable()
        {
            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");

            var refreshSegmentsTable = cnn.Execute("delete from segments;");

            cnn.Close();
        }

        public static bool DoesTableContainData()
        {
            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");

            var isDataPresent = cnn.ExecuteScalar("select count(*) from segments");
            
            return (int)isDataPresent > 0;
        }
    }
}