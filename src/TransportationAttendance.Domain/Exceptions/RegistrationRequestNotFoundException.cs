namespace TransportationAttendance.Domain.Exceptions;

public class RegistrationRequestNotFoundException : EntityNotFoundException
{
    public RegistrationRequestNotFoundException(Guid requestId)
        : base("RegistrationRequest", requestId)
    {
    }
}
