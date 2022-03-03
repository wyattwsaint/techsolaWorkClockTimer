using System.Data.SqlClient;

namespace techsolaWorkClockTimer
{
    public class RefreshSegmentsTable
    {

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
    }
}