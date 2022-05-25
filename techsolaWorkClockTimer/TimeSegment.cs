using System;

namespace techsolaWorkClockTimer;

public sealed record TimeSegment(DateTime Start, string Project, string? WorkItem, string? EmployeeNumber) : ObservableRecord
{
    // Used by dapper
    public TimeSegment(DateTime timeSegmentStart, DateTime timeSegmentEnd, string project, string? workItem, string? employeeNumber) : this(timeSegmentStart, project, workItem, employeeNumber)
    {
        End = timeSegmentEnd;
    }

    private DateTime? end;

    public DateTime? End
    {
        get => end;
        set => Set(ref end, value);
    }
}