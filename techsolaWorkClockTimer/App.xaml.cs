using Dapper;
using System;
using System.Windows;

namespace techsolaWorkClockTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TechsolaClock Clock = new();
        
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            //---- Get Settings ----

            Clock.GetSettings();

        }
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            if (Clock.Segments.Count > 0)
                Clock.Segments[^1].End ??= DateTime.Now;
            DataBase.RefreshTable();
            foreach (var segment in Clock.Segments)
            {
                DataBase.Connection.Execute(
                        @"Insert into segments (
                                TimeSegmentStart, 
                                TimeSegmentEnd, 
                                Project, 
                                WorkItem, 
                                EmployeeNumber, 
                                ProjectFeature,
                                WorkItemNumber,
                                Phase
                                ) 
                                values(
                                    @Start, 
                                    @End, 
                                    @Project, 
                                    @WorkItem, 
                                    @EmployeeNumber, 
                                    @ProjectFeature,
                                    @WorkItemNumber,
                                    @Phase
                                    )",
                        new
                        {
                            segment.Start,
                            segment.End,
                            segment.Project,
                            segment.WorkItem,
                            segment.EmployeeNumber,
                            segment.ProjectFeature,
                            segment.WorkItemNumber,
                            segment.Phase,
                        });
            }

            // ---- Save(Set) Settings ----

            Clock.SetSettings();

        }
    }
}