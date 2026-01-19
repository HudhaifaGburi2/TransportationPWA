namespace TransportationAttendance.Domain.Entities;

public class StudentLeave : BaseEntity
{
    public Guid StudentId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string? AttachmentUrl { get; private set; }
    public string? AttachmentFileName { get; private set; }
    public bool IsApproved { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public Guid? ApprovedBy { get; private set; }
    public bool IsCancelled { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public Guid? CancelledBy { get; private set; }
    public string? CancellationReason { get; private set; }
    public Guid CreatedByUserId { get; private set; }

    // Navigation
    public Student? Student { get; private set; }

    private StudentLeave() { }

    public static StudentLeave Create(
        Guid studentId,
        DateTime startDate,
        DateTime endDate,
        string reason,
        Guid createdBy,
        string? attachmentUrl = null,
        string? attachmentFileName = null)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date");

        return new StudentLeave
        {
            StudentId = studentId,
            StartDate = startDate,
            EndDate = endDate,
            Reason = reason,
            AttachmentUrl = attachmentUrl,
            AttachmentFileName = attachmentFileName,
            CreatedByUserId = createdBy,
            IsApproved = false,
            IsCancelled = false
        };
    }

    public void Approve(Guid approvedBy)
    {
        IsApproved = true;
        ApprovedAt = DateTime.UtcNow;
        ApprovedBy = approvedBy;
    }

    public void Cancel(Guid cancelledBy, string? reason = null)
    {
        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;
        CancelledBy = cancelledBy;
        CancellationReason = reason;
    }

    public bool IsActive()
    {
        var now = DateTime.UtcNow.Date;
        return IsApproved && !IsCancelled && StartDate.Date <= now && EndDate.Date >= now;
    }

    public bool IsPending() => !IsApproved && !IsCancelled;
}
