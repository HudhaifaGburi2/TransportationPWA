namespace TransportationAttendance.Domain.Interfaces;

public interface IEntity<TKey>
{
    TKey Id { get; }
}

public interface IEntity : IEntity<Guid>
{
}
