using Ardalis.Result;
using SharedKernal;

namespace MediFlow.Schedule.Domain;

public class Patient : Entity
{
	public Guid UserId { get; init; }
	private readonly List<Appointment> _appointments = new();
	public IReadOnlyList<Appointment> Appointemnts => _appointments.AsReadOnly();

	public Patient(Guid userId)
	{
		UserId = userId;
	}

	public Result<Appointment> RequestAppointemnt(Guid clinicId, Guid doctorId, DateTime dateTime)
	{

		//validations here 
		var appointemnt = new Appointment(doctorId, clinicId, Id, dateTime);

		_appointments.Add(appointemnt);

		return Result.Success(appointemnt);
	}
}