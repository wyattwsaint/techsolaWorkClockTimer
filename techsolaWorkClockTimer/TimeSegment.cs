using System;

namespace techsolaWorkClockTimer;

public sealed record TimeSegment(DateTime Start, string Project, string? WorkItem) : ObservableRecord
{
    // Used by dapper
    public TimeSegment(DateTime timeSegmentStart, DateTime timeSegmentEnd, string project, string? workItem) : this(timeSegmentStart, project, workItem)
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