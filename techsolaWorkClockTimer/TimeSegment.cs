using System;
using System.Collections.ObjectModel;

namespace techsolaWorkClockTimer;

public sealed record TimeSegment
    (DateTime Start, string Project, string? WorkItem, string? EmployeeNumber, string? ProjectFeature) : ObservableRecord
{
    // Used by dapper
    public TimeSegment(DateTime timeSegmentStart, DateTime timeSegmentEnd, string project, string? workItem,
        string? employeeNumber, string? projectFeature) : this(timeSegmentStart, project, workItem, employeeNumber, projectFeature)
    {
        End = timeSegmentEnd;
    }

    private DateTime? end;

    public DateTime? End
    {
        get => end;
        set => Set(ref end, value);
    }

    private string? day;

    public string? Day
    {
        get
        {
            var testDay = day != null ? day[..3] : "";
            return testDay;
        }
        set => Set(ref day, value);
    }

    private DateTime? date;

    public DateTime? Date
    {
        get => date?.Date.Date ?? DateTime.Today;

        set => Set(ref date, value);
    }
}