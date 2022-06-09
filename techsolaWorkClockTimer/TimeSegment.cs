using System;

namespace techsolaWorkClockTimer;

public sealed record TimeSegment
(DateTime Start, string Project, string? WorkItem, string? EmployeeNumber, string? ProjectFeature,
    string? Phase) : ObservableRecord
{
    // Used by dapper
    public TimeSegment(DateTime timeSegmentStart, DateTime timeSegmentEnd, string project, string? workItem,
        string? employeeNumber, string? projectFeature, string? phase) : this(timeSegmentStart, project, workItem,
        employeeNumber, projectFeature, phase)
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
            var abbrevDay = day != null ? day[..3] : "";
            return abbrevDay;
        }
        set => Set(ref day, value);
    }

    private string? date;

    public string? Date
    {
        get => DateTime.Today.ToShortDateString();
        set => Set(ref date, value);
    }
}