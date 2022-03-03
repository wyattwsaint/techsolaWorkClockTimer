using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace techsolaWorkClockTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TechsolaClock Clock = new();

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            if (Clock.Segments[^1].End == null)
            {
                Clock.Segments[^1].End = DateTime.Now;
            }

            var cnn = new SqlConnection(@"Server=localhost; Database=techsolaclock; Integrated Security=True;");
            cnn.Open();

            var adapter = new SqlDataAdapter();

            foreach (var segment in Clock.Segments)
            {
                var command = new SqlCommand(
                    "Insert into segments (TimeSegmentStart, TimeSegmentEnd, Project) values(@start, @end, @project)",
                    cnn);

                var start = new SqlParameter("@start", SqlDbType.DateTime) { Value = segment.Start };
                var end = new SqlParameter("@end", SqlDbType.DateTime) { Value = segment.End };
                var project = new SqlParameter("@project", SqlDbType.VarChar) { Value = segment.Project };

                command.Parameters.AddRange(new[] { start, end, project });

                adapter.InsertCommand = command;
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
            }


            cnn.Close();
        }
    }
}