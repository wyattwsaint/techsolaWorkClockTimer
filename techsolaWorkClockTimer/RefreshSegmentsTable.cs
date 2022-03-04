using System;
using System.Data.SqlClient;

namespace techsolaWorkClockTimer
{
    public class RefreshSegmentsTable
    {
        public bool IsDataFromPriorDay()
        {
            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");
            cnn.Open();
            var getLatestTimeStampCommand = new SqlCommand(
                "select top 1 TimeSegmentEnd from segments order by TimeSegmentEnd desc",
                cnn);
            var latestDbDateTime = DateTime.Parse(getLatestTimeStampCommand.ExecuteScalar().ToString()!);
            getLatestTimeStampCommand.Dispose();
            cnn.Close();

            return latestDbDateTime.Day < DateTime.Now.Day;
        }
        public void RefreshTable()
        {
            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");
            cnn.Open();

            var adapter = new SqlDataAdapter();

            var dropCommand = new SqlCommand(
                "drop table segments",
                cnn);

            adapter.InsertCommand = dropCommand;
            adapter.InsertCommand.ExecuteNonQuery();
            dropCommand.Dispose();

            var createTableCommand = new SqlCommand(
                "create table segments (TimeSegmentStart datetime, TimeSegmentEnd datetime, Project varchar(50))",
                cnn);

            adapter.InsertCommand = createTableCommand;
            adapter.InsertCommand.ExecuteNonQuery();
            createTableCommand.Dispose();

            cnn.Close();
        }

        public bool DoesTableContainData()
        {
            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");
            cnn.Open();

            var adapter = new SqlDataAdapter();

            var dropCommand = new SqlCommand(
                "select count(*) from segments",
                cnn);
            if ((int)dropCommand.ExecuteScalar() == 0) return false;
            
            dropCommand.Dispose();
            
            return true;
        }
    }
}