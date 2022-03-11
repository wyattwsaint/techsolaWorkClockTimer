using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Dapper;

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
            if (Clock.Segments.Count > 0)
                Clock.Segments[^1].End ??= DateTime.Now;

            foreach (var segment in Clock.Segments)
            {
                DataBase.Connection.Execute(
                    "Insert into segments (TimeSegmentStart, TimeSegmentEnd, Project) values(@Start, @End, @Project)",
                    new { segment.Start, segment.End, segment.Project });
            }
        }
    }
}