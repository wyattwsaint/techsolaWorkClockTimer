using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace techsolaWorkClockTimer
{
    public class TechsolaClock : ObservableObject
    {
        public TechsolaClock()
        {
            if (DataBase.DoesTableContainData() && DataBase.IsDataFromPriorDay())
                DataBase.RefreshTable();

            var timeSegments =
                DataBase.Connection.Query<TimeSegment>(
                    "select TimeSegmentStart, TimeSegmentEnd, Project, WorkItem, EmployeeNumber, ProjectFeature, WorkItemNumber, Phase from segments;");

            foreach (var timeSegment in timeSegments) segments.Add(timeSegment);
        }

        private CancellationTokenSource? cancellationTokenSource;
        private TimeSpan? totalTime;
        public string? DisplayTime => totalTime is not null ? $@"{totalTime:hh\:mm\:ss}" : null;
        public string? DecimalDisplayTime => totalTime is not null ? $@"{totalTime.Value.TotalHours:0.00}" : null;

        private string? breakTimeLeft;

        public string? BreakTimeLeft
        {
            get => breakTimeLeft;
            set => Set(ref breakTimeLeft, value);
        }

        public const string DefaultProjectName = "Techsola Internal";

        public ObservableCollection<ProjectTime> Times { get; } = new()
        {
            new(DefaultProjectName, "White", workItem: null, employeeNumber: null),
            new("Heritage", "PaleVioletRed", workItem: null, employeeNumber: null),
            new("Exactis", "PaleTurquoise", workItem: null, employeeNumber: null),
            new("Capri Cork", "PaleGoldenrod", workItem: null, employeeNumber: null),
            new("Zeager", "Peru", workItem: null, employeeNumber: null),
            new("JanTrak", "CadetBlue", workItem: null, employeeNumber: null),
            new("Traverse-Sales", "RosyBrown", workItem: null, employeeNumber: null),
            new("Traverse-Enhance", "YellowGreen", workItem: null, employeeNumber: null),
            new("WRA Database", "DarkGoldenrod", workItem: null, employeeNumber: null),
            new("DSB", "Turquoise", workItem: null, employeeNumber: null),
            new("General", "Thistle", workItem: null, employeeNumber: null),
            new("Custom", "Tomato", workItem: null, employeeNumber: null),
        };

        public ObservableCollection<string>? ProjectFeature { get; } = new()
        {
            new("Capri Cork Traverse (Capri Cork LLC)"),
            new("Capri Cork Traverse - Capri Cork - Development"),
            new("Capri Cork Traverse - Capri Cork - Implementation"),
            new("Capri Cork Traverse - Capri Cork - Materials"),
            new("Capri Cork Traverse - Capri Cork - Non Billable"),
            new("Capri Cork Traverse - MATERIALS"),
            new("Capri Cork Traverse - NETWORK SUPPORT"),
            new("Custom Program (FIXED FEE)"),
            new("Custom Program - ACCOUNTING SUPPORT"),
            new("Custom Program - G2 Tracker Software v 1.1.x"),
            new("Custom Program - MATERIALS"),
            new("Custom Program - NONBILLABLE TIME"),
            new("Custom Program - SOFTWARE SUPPORT"),
            new("DSB Scanner Integration (D S Burkholder Inc)"),
            new("DSB Scanner Integration - Accounting Support"),
            new("DSB Scanner Integration - General"),
            new("DSB Scanner Integration - Materials"),
            new("DSB Scanner Integration - Non-Billable"),
            new("DSB Scanner Integration - Support"),
            new("Exactis (TSA Internal)"),
            new("Exactis - Exactis Hardware"),
            new("Exactis - General"),
            new("Exactis - Support"),
            new("G2 Tracker Software v 1.1.x - G2 Tracker Software v 1.1.x"),
            new("GENERAL (Alderfer's Poultry Farm) "),
            new("GENERAL - MATERIALS"),
            new("GENERAL - MATERIALS"),
            new("GENERAL - NETWORK SUPPORT"),
            new("GENERAL - NETWRK RECURR SRV"),
            new("GENERAL - NONBILLABLE TIME"),
            new("GENERAL - NONBILLABLE TIME"),
            new("GENERAL - SOFTWARE DEVELOPMENT"),
            new("GENERAL - SOFTWARE SUPPORT"),
            new("GENERAL - SOFTWARE SUPPORT"),
            new("General Project - NONBILLABLE TIME Software"),
            new("Heritage Cloud Pete & Gerry's (Heritage Poultry Mgnt Services) "),
            new("Heritage Cloud Pete & Gerry's - General"),
            new("Heritage Cloud Pete & Gerry's - Materials"),
            new("Heritage Cloud Pete & Gerry's - Non-Billable"),
            new("Heritage Cloud Pete & Gerry's - Performance Bonus"),
            new("Heritage Software (Heritage Poultry Mgnt Services)"),
            new("Heritage Software - Accounting Support"),
            new("Heritage Software - Contract Payments"),
            new("Heritage Software - Flock Financial Projection"),
            new("Heritage Software - General"),
            new("Heritage Software - Lotus Sheet Conversion"),
            new("Heritage Software - Materials"),
            new("Heritage Software - Non-Billable"),
            new("Heritage Software - Ownership Financial Statements"),
            new("Heritage Software - Performance Bonus"),
            new("Heritage Software - Pullet Financial Configuration"),
            new("Heritage Software - Support"),
            new("INTERNAL TSA (TSA Internal)"),
            new("INTERNAL TSA - ADMINISTRATION"),
            new("INTERNAL TSA - PAID TIME OFF"),
            new("JanTrak Version 2.0 (JAN Packaging)"),
            new("JanTrak Version 2.0 - MATERIALS"),
            new("JanTrak Version 2.0 - NONBILLABLE TIME"),
            new("JanTrak Version 2.0 - PERFORMANCE BONUS"),
            new("JanTrak Version 2.0 - SOFTWARE DEVELOPMENT"),
            new("TRAVERSE - Market Sales (S Clyde Weaver Inc)"),
            new("TRAVERSE - Market Sales - Accounting Support"),
            new("TRAVERSE - Market Sales - General"),
            new("TRAVERSE - Market Sales - Materials"),
            new("TRAVERSE - Market Sales - Non-Billable"),
            new("TRAVERSE - Market Sales - Performance Bonus"),
            new("TRAVERSE - Market Sales - Support"),
            new("Traverse Enhancements (Weavers Store Inc.)"),
            new("Traverse Enhancements - General"),
            new("Traverse Enhancements - Item Batch Update"),
            new("Traverse Enhancements - Item Batching"),
            new("Traverse Enhancements - Materials"),
            new("Traverse Enhancements - Non-Billable"),
            new("Traverse Enhancements - Performance Bonus"),
            new("Traverse Enhancements - PO Receiving"),
            new("Traverse Enhancements - Quick Items"),
            new("Traverse Enhancements - Support"),
            new("Traverse Importer (TSA Internal)"),
            new("Traverse Importer - General"),
            new("Traverse Importer - Materials"),
            new("WRA Database (Weinstein Realty Advisors)"),
            new("WRA Database - General"),
            new("WRA Database - Materials"),
            new("WRA Database - Non-Billable"),
            new("WRA Database - Performance Bonus"),
            new("WRA Database - Support"),
            new("Zeager Traverse eCom Design (Zeager Brothers Inc.)"),
            new("Zeager Traverse eCom Design - General"),
            new("Zeager Traverse eCom Design - Non-Billable"),
            new("Zeager Traverse eCom Design - Support"),
            new("Zeager Traverse Improve (ZT) (Zeager Brothers Inc.)"),
            new("Zeager Traverse Improve (ZT) - General"),
            new("Zeager Traverse Improve (ZT) - Non-Billable"),
            new("Zeager Traverse Improve (ZT) - Support"),
        };

        public ObservableCollection<string> Phases { get; } = new()
        {
            new("Research"),
            new("Development"),
            new("Code Review"),
            new("Testing"),
        };

        private readonly ObservableCollection<TimeSegment> segments = new();

        public ReadOnlyObservableCollection<TimeSegment> Segments => new(segments);

        private List<TimeSegment> endOfDaySegments = new();

        public List<TimeSegment> EndOfDaySegments
        {
            get
            {
                return Segments.GroupBy(s => s.WorkItem).Select(cs => new TimeSegment(DateTime.Now, "test", null, null, null, null, null)
                {
                    Day = cs.First().Day,
                    EmployeeNumber = cs.First().EmployeeNumber,
                    Date = cs.First().Date,
                    ProjectFeature = cs.First().ProjectFeature,
                    Phase = cs.First().Phase,
                    WorkItem = cs.First().WorkItem,
                    WorkItemNumber = cs.First().WorkItemNumber,
                    Hours = cs.Sum(c => c.Hours),
                }).ToList();
            }
        }

        private string? employeeNumberTechClock;

        public string? EmployeeNumberTechClock
        {
            get => employeeNumberTechClock;
            set { Set(ref employeeNumberTechClock, value); }
        }


        //-----WorkItemProperties-----Begin

        private string? workItemOneNumber;

        public string? WorkItemOneNumber
        {
            get => workItemOneNumber;
            set { Set(ref workItemOneNumber, value); }
        }

        private string? workItemTwoNumber;

        public string? WorkItemTwoNumber
        {
            get => workItemTwoNumber;
            set { Set(ref workItemTwoNumber, value); }
        }

        private string? workItemThreeNumber;

        public string? WorkItemThreeNumber
        {
            get => workItemThreeNumber;
            set { Set(ref workItemThreeNumber, value); }
        }

        private string? workItemFourNumber;

        public string? WorkItemFourNumber
        {
            get => workItemFourNumber;
            set { Set(ref workItemFourNumber, value); }
        }

        private string? workItemFiveNumber;

        public string? WorkItemFiveNumber
        {
            get => workItemFiveNumber;
            set { Set(ref workItemFiveNumber, value); }
        }

        private string? workItemSixNumber;

        public string? WorkItemSixNumber
        {
            get => workItemSixNumber;
            set { Set(ref workItemSixNumber, value); }
        }

        private string? workItemOneTechsolaClock;

        public string? WorkItemOneTechsolaClock
        {
            get => workItemOneTechsolaClock;
            set { Set(ref workItemOneTechsolaClock, value); }
        }

        private string? workItemTwoTechsolaClock;

        public string? WorkItemTwoTechsolaClock
        {
            get => workItemTwoTechsolaClock;
            set { Set(ref workItemTwoTechsolaClock, value); }
        }

        private string? workItemThreeTechsolaClock;

        public string? WorkItemThreeTechsolaClock
        {
            get => workItemThreeTechsolaClock;
            set { Set(ref workItemThreeTechsolaClock, value); }
        }

        private string? workItemFourTechsolaClock;

        public string? WorkItemFourTechsolaClock
        {
            get => workItemFourTechsolaClock;
            set { Set(ref workItemFourTechsolaClock, value); }
        }
         
        private string? workItemFiveTechsolaClock;

        public string? WorkItemFiveTechsolaClock
        {
            get => workItemFiveTechsolaClock;
            set { Set(ref workItemFiveTechsolaClock, value); }
        }

        private string? workItemSixTechsolaClock;

        public string? WorkItemSixTechsolaClock
        {
            get => workItemSixTechsolaClock;
            set { Set(ref workItemSixTechsolaClock, value); }
        }

        //-----WorkItemProperties-----End

        //-----ProjectFeatureProperties-----Start

        private string? workItemOneProjectFeature;

        public string? WorkItemOneProjectFeature
        {
            get => workItemOneProjectFeature;
            set { Set(ref workItemOneProjectFeature, value); }
        }

        private string? workItemTwoProjectFeature;

        public string? WorkItemTwoProjectFeature
        {
            get => workItemTwoProjectFeature;
            set { Set(ref workItemTwoProjectFeature, value); }
        }

        private string? workItemThreeProjectFeature;

        public string? WorkItemThreeProjectFeature
        {
            get => workItemThreeProjectFeature;
            set { Set(ref workItemThreeProjectFeature, value); }
        }

        private string? workItemFourProjectFeature;

        public string? WorkItemFourProjectFeature
        {
            get => workItemFourProjectFeature;
            set { Set(ref workItemFourProjectFeature, value); }
        }

        private string? workItemFiveProjectFeature;

        public string? WorkItemFiveProjectFeature
        {
            get => workItemFiveProjectFeature;
            set { Set(ref workItemFiveProjectFeature, value); }
        }

        private string? workItemSixProjectFeature;

        public string? WorkItemSixProjectFeature
        {
            get => workItemSixProjectFeature;
            set { Set(ref workItemSixProjectFeature, value); }
        }

        //-----ProjectFeatureProperties-----End

        //-----PhaseProperties-----Start

        private string? workItemOnePhase;

        public string? WorkItemOnePhase
        {
            get => workItemOnePhase;
            set { Set(ref workItemOnePhase, value); }
        }

        private string? workItemTwoPhase;

        public string? WorkItemTwoPhase
        {
            get => workItemTwoPhase;
            set { Set(ref workItemTwoPhase, value); }
        }

        private string? workItemThreePhase;

        public string? WorkItemThreePhase
        {
            get => workItemThreePhase;
            set { Set(ref workItemThreePhase, value); }
        }

        private string? workItemFourPhase;

        public string? WorkItemFourPhase
        {
            get => workItemFourPhase;
            set { Set(ref workItemFourPhase, value); }
        }

        private string? workItemFivePhase;

        public string? WorkItemFivePhase
        {
            get => workItemFivePhase;
            set { Set(ref workItemFivePhase, value); }
        }

        private string? workItemSixPhase;

        public string? WorkItemSixPhase
        {
            get => workItemSixPhase;
            set { Set(ref workItemSixPhase, value); }
        }

        //-----PhaseProperties-----End

        //-----EffortProperties-----Begin

        private string? effortOne;

        public string? EffortOne
        {
            get => effortOne;
            set { Set(ref effortOne, value); }
        }

        private string? effortTwo;

        public string? EffortTwo
        {
            get => effortTwo;
            set { Set(ref effortTwo, value); }
        }

        private string? effortThree;

        public string? EffortThree
        {
            get => effortThree;
            set { Set(ref effortThree, value); }
        }

        private string? effortFour;

        public string? EffortFour
        {
            get => effortFour;
            set { Set(ref effortFour, value); }
        }

        private string? effortFive;

        public string? EffortFive
        {
            get => effortFive;
            set { Set(ref effortFive, value); }
        }

        private string? effortSix;

        public string? EffortSix
        {
            get => effortSix;
            set { Set(ref effortSix, value); }
        }

        //-----EffortProperties-----End

        public TimeSegment? RunningSegment => segments.LastOrDefault() is { End: null } runningSegment
            ? runningSegment
            : null;

        public void Start(string project, string? workItem, string? employeeNumber, string? projectFeature, string? workItemNumber, string? phase)
        {
            if (RunningSegment is not null)
                throw new InvalidOperationException("Multiple segments must not run at the same time.");

            foreach (var segment in Segments)
            {
                segment.Day = DateTime.Now.DayOfWeek.ToString();
            }

            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);

            segments.Add(new TimeSegment(DateTime.Now, project, workItem, employeeNumber, projectFeature, workItemNumber, phase));
            OnPropertyChanged(nameof(RunningSegment));

            cancellationTokenSource = new();

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        totalTime = GetCurrentTime(project: null);
                        OnPropertyChanged(nameof(DisplayTime));
                        OnPropertyChanged(nameof(DecimalDisplayTime));
                        foreach (var projectTime in Times)
                            projectTime.Time = GetCurrentTime(projectTime.ProjectName);
                        await Task.Delay(250, cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                }
            });
        }

        public void Stop()
        {

            segments[^1].End = DateTime.Now;
            OnPropertyChanged(nameof(RunningSegment));

            segments[^1].Day = DateTime.Now.DayOfWeek.ToString();

            cancellationTokenSource!.Cancel();

            cancellationTokenSource = null;
        }

        public TimeSpan GetCurrentTime(string? project)
        {
            return segments
                .Where(segment => project is null || segment.Project == project)
                .Sum(s => (s.End ?? DateTime.Now) - s.Start);
        }

        public void CreateEndOfDayWindow()
        {
            WorkdayComplete endOfDayPopUp = new();

            endOfDayPopUp.Visibility = Visibility.Visible;
        }

        private TimeSpan? endOfDayTargetTime;

        public TimeSpan? EndOfDayTargetTime
        {
            get => endOfDayTargetTime;
            set => Set(ref endOfDayTargetTime, value);
        }

        public void ConvertTimeIntArrayToTimeSpan(int[] targetTimeInt)
        {
            if (targetTimeInt.Length == 1)
            {
                EndOfDayTargetTime = new TimeSpan(0, targetTimeInt[0] + 12, 0, 0);
            }

            if (targetTimeInt.Length == 2)
            {
                EndOfDayTargetTime = new TimeSpan(0, targetTimeInt[0] + 12, targetTimeInt[1], 0);
            }

            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);
        }

        private TimeSpan? workDayHours;

        public TimeSpan? WorkDayHours
        {
            get => workDayHours;
            set => Set(ref workDayHours, value);
        }

        public void GetWorkdayHoursFromComboBox(int targetHours)
        {
            WorkDayHours = new TimeSpan(0, targetHours, 0, 0);
            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);
        }

        public string UpdateBreaktimeLeft(TimeSpan? endOfDay, TimeSpan? workHours)
        {
            if (endOfDayTargetTime == null && workDayHours == null) return "";
            var breakTime = (endOfDay - DateTime.Now.TimeOfDay) -
                            (workHours - GetCurrentTime(project: null));
            return BreakTimeLeft = breakTime?.Ticks < 0 ? $@"-{breakTime:hh\:mm\:ss}" : $@"{breakTime:hh\:mm\:ss}";
        }


        //-----------End of day display stuff-----------

        private string? getEndOfDayTargetTime;

        public string? GetEndOfDayTargetTime
        {
            get => getEndOfDayTargetTime;
            set => Set(ref getEndOfDayTargetTime, value);
        }

        private string? getWorkDayHours;

        public string? GetWorkDayHours
        {
            get => getWorkDayHours;
            set => Set(ref getWorkDayHours, value);
        }

        private string ConvertTimeSpansToStringsForComboboxTwoWayBinding(TimeSpan time, bool addPm)
        {
            if (addPm)
            {
                var endOfDayTime = Convert.ToInt32(time.ToString().Substring(0, 2));
                if (endOfDayTime > 12) return (endOfDayTime - 12) + "PM";
                return endOfDayTime + "PM";
            }

            var workDayTime = Convert.ToInt32(time.ToString().Substring(0, 2));
            if (workDayTime > 12) return (workDayTime - 12) + " HRS";
            return workDayTime.ToString();
        }

        //----------Persistence----------

        public void GetSettings()
        {
            WorkDayHours = Properties.Settings.Default.workDayHours;
            EndOfDayTargetTime = Properties.Settings.Default.endOfDayTargetTime;

            getWorkDayHours =
                ConvertTimeSpansToStringsForComboboxTwoWayBinding(Properties.Settings.Default.workDayHours,
                    addPm: false);
            OnPropertyChanged(nameof(GetWorkDayHours));
            getEndOfDayTargetTime =
                ConvertTimeSpansToStringsForComboboxTwoWayBinding(Properties.Settings.Default.endOfDayTargetTime,
                    addPm: true);
            OnPropertyChanged(nameof(GetEndOfDayTargetTime));
            UpdateBreaktimeLeft(EndOfDayTargetTime, WorkDayHours);

            WorkItemOneNumber = Properties.Settings.Default.workItemOneNumber;
            WorkItemTwoNumber = Properties.Settings.Default.workItemTwoNumber;
            WorkItemThreeNumber = Properties.Settings.Default.workItemThreeNumber;
            WorkItemFourNumber = Properties.Settings.Default.workItemFourNumber;
            WorkItemFiveNumber = Properties.Settings.Default.workItemFiveNumber;
            WorkItemSixNumber = Properties.Settings.Default.workItemSixNumber;

            WorkItemOneTechsolaClock = Properties.Settings.Default.workItemOne;
            WorkItemTwoTechsolaClock = Properties.Settings.Default.workItemTwo;
            WorkItemThreeTechsolaClock = Properties.Settings.Default.workItemThree;
            WorkItemFourTechsolaClock = Properties.Settings.Default.workItemFour;
            WorkItemFiveTechsolaClock = Properties.Settings.Default.workItemFive;
            WorkItemSixTechsolaClock = Properties.Settings.Default.workItemSix;

            WorkItemOneProjectFeature = Properties.Settings.Default.workItemOneProjectFeature;
            WorkItemTwoProjectFeature = Properties.Settings.Default.workItemTwoProjectFeature;
            WorkItemThreeProjectFeature = Properties.Settings.Default.workItemThreeProjectFeature;
            WorkItemFourProjectFeature = Properties.Settings.Default.workItemFourProjectFeature;
            WorkItemFiveProjectFeature = Properties.Settings.Default.workItemFiveProjectFeature;
            WorkItemSixProjectFeature = Properties.Settings.Default.workItemSixProjectFeature;

            WorkItemOnePhase = Properties.Settings.Default.workItemOnePhase;
            WorkItemTwoPhase = Properties.Settings.Default.workItemTwoPhase;
            WorkItemThreePhase = Properties.Settings.Default.workItemThreePhase;
            WorkItemFourPhase = Properties.Settings.Default.workItemFourPhase;
            WorkItemFivePhase = Properties.Settings.Default.workItemFivePhase;
            WorkItemSixPhase = Properties.Settings.Default.workItemSixPhase;

            EmployeeNumberTechClock = Properties.Settings.Default.employeeNumber;
        }

        public void SetSettings()
        {
            Properties.Settings.Default.endOfDayTargetTime = EndOfDayTargetTime!.Value;
            Properties.Settings.Default.workDayHours = WorkDayHours!.Value;

            Properties.Settings.Default.workItemOneNumber = workItemOneNumber;
            Properties.Settings.Default.workItemTwoNumber = workItemTwoNumber;
            Properties.Settings.Default.workItemThreeNumber = workItemThreeNumber;
            Properties.Settings.Default.workItemFourNumber = workItemFourNumber;
            Properties.Settings.Default.workItemFiveNumber = workItemFiveNumber;
            Properties.Settings.Default.workItemSixNumber = workItemSixNumber;

            Properties.Settings.Default.workItemOne = workItemOneTechsolaClock;
            Properties.Settings.Default.workItemTwo = workItemTwoTechsolaClock;
            Properties.Settings.Default.workItemThree = workItemThreeTechsolaClock;
            Properties.Settings.Default.workItemFour = workItemFourTechsolaClock;
            Properties.Settings.Default.workItemFive = workItemFiveTechsolaClock;
            Properties.Settings.Default.workItemSix = workItemSixTechsolaClock;

            Properties.Settings.Default.workItemOneProjectFeature = workItemOneProjectFeature;
            Properties.Settings.Default.workItemTwoProjectFeature = workItemTwoProjectFeature;
            Properties.Settings.Default.workItemThreeProjectFeature = workItemThreeProjectFeature;
            Properties.Settings.Default.workItemFourProjectFeature = workItemFourProjectFeature;
            Properties.Settings.Default.workItemFiveProjectFeature = workItemFiveProjectFeature;
            Properties.Settings.Default.workItemSixProjectFeature = workItemSixProjectFeature;

            Properties.Settings.Default.workItemOnePhase = workItemOnePhase;
            Properties.Settings.Default.workItemTwoPhase = workItemTwoPhase;
            Properties.Settings.Default.workItemThreePhase = workItemThreePhase;
            Properties.Settings.Default.workItemFourPhase = workItemFourPhase;
            Properties.Settings.Default.workItemFivePhase = workItemFivePhase;
            Properties.Settings.Default.workItemSixPhase = workItemSixPhase;

            Properties.Settings.Default.employeeNumber = EmployeeNumberTechClock;

            Properties.Settings.Default.Save();
        }
    }
}