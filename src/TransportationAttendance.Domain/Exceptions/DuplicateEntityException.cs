namespace TransportationAttendance.Domain.Exceptions;

public class DuplicateEntityException : DomainException
{
    public string EntityType { get; }
    public string Property { get; }
    public object Value { get; }

    public DuplicateEntityException(string entityType, string property, object value)
        : base($"{entityType} with {property} '{value}' already exists.", "DUPLICATE_ENTITY")
    {
        EntityType = entityType;
        Property = property;
        Value = value;
    }
}
