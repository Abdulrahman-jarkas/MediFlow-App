using SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernal;

public class Entity
{
	public Guid Id { get; private init; }

	private List<DomainEventBase> _domainEvents = new();
	[NotMapped]
	public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

	protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
	public void ClearDomainEvents() => _domainEvents.Clear();

	public Entity()
	{
		Id = Guid.NewGuid();
	}
}
