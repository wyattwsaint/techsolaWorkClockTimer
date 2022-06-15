using System;
using System.Linq;

namespace techsolaWorkClockTimer;

public sealed record TimeSegment
(DateTime Start, string Project, string? WorkItem, string? EmployeeNumber, string? ProjectFeature, string? WorkItemNumber,
    string? Phase) : ObservableRecord
{
    // Used by dapper
    public TimeSegment(DateTime timeSegmentStart, DateTime timeSegmentEnd, string project, string? workItem,
        string? employeeNumber, string? projectFeature, string? workItemNumber, string? phase) : this(timeSegmentStart, project, workItem,
        employeeNumber, projectFeature, workItemNumber, phase)
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
        get => day;
        set
        {
            value = day != null ? day[..3] : "";
            Set(ref day, value);
        }
    }

    private string? date;

    public string? Date
    {
        get => DateTime.Today.ToShortDateString();
        set => Set(ref date, value);
    }

    private double? hours;

    public double? Hours
    {
        get => hours;
        set
        {
            value = Math.Round((End is not null ? End - Start : new TimeSpan(0, 0, 0, 0)).Value.TotalHours * 4,
                MidpointRounding.ToEven) / 4;
            Set(ref hours, value);
        }
    }
}