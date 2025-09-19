using Ardalis.Result;
using SharedKernal;

namespace MediFlow.ClinicManagement.Domain.Entities;

internal class Doctor : Entity
{
	public string Name { get; private set; } = string.Empty;
	public Guid UserId { get; init; }

	private List<Clinic> _clinics = new();
	public IReadOnlyList<Clinic> Clinics => _clinics.AsReadOnly();

	private Doctor(Guid userId, string name) : this()
	{
		Name = name;
		UserId = userId;
	}

	public static Result<Doctor> Create(Guid userId, string name)
	{
		if (userId == Guid.Empty)
			return Result.Invalid(new ValidationError(nameof(UserId), "Invalid User Id"));

		var validateResult = Validate(name);

		if (!validateResult.IsSuccess)
			return Result.Invalid(validateResult.ValidationErrors);

		return Result.Created(new Doctor(userId, name));
	}

	public Result<Doctor> UpdateName(string name)
	{
		var validateResult = Validate(name);

		if (!validateResult.IsSuccess)
			return Result.Invalid(validateResult.ValidationErrors);

		Name = validateResult.Value;

		return Result.Success(this);
	}

	public static Result<string> Validate(string name)
	{
		if (string.IsNullOrEmpty(name.Trim()))
			return Result.Invalid(new ValidationError(nameof(Name), "The name of doctor must not be empty"));

		return Result.Success(name.Trim());
	}

	public Doctor() { }
}