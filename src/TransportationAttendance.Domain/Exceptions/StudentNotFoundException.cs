namespace TransportationAttendance.Domain.Exceptions;

public class StudentNotFoundException : EntityNotFoundException
{
    public StudentNotFoundException(object studentId)
        : base("Student", studentId)
    {
    }

    public StudentNotFoundException(string studentExternalId)
        : base("Student", studentExternalId)
    {
    }
}
