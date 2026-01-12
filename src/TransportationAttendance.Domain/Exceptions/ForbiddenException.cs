namespace TransportationAttendance.Domain.Exceptions;

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message = "Access to this resource is forbidden.")
        : base(message, "FORBIDDEN")
    {
    }
}
