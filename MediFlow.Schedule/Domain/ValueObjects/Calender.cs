using Ardalis.Result;

namespace MediFlow.Schedule.Domain.ValueObjects;

public class Calender
{
	private readonly Dictionary<DateTime, (Guid ClinicId, Guid DoctorId, Guid PatientId)> _data = new();
	public IReadOnlyDictionary<DateTime, (Guid ClinicId, Guid DoctorId, Guid PatientId)> Data => _data.AsReadOnly();

	private Calender(Dictionary<DateTime, (Guid ClinicId, Guid DoctorId, Guid PatientId)> data)
	{
		_data = data;
	}

	public static Result<Calender> Create(Dictionary<DateTime, (Guid ClinicId, Guid DoctorId, Guid PatientId)> data)
	{
		var schedule = new Calender(data);

		return Result.Success();
	}

	public Result<Calender> Add(Guid clinicId, Guid doctorId, Guid patientId, DateTime dateTime)
	{
		if (_data.ContainsKey(dateTime))
			return Result.Invalid(new ValidationError(nameof(dateTime), "there is already appointemnt at this date time"));

		_data.Add(dateTime, (clinicId, doctorId, patientId));

		return Result.Success(this);
	}
}