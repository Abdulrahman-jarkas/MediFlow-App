using Ardalis.Result;
using SharedKernal;

namespace MediFlow.ClinicManagement.Domain.Entities;

internal class Room : Entity
{
	public string Name { get; private set; } = string.Empty;

	private Room(string name)
	{
		Name = name;
	}

	public static Result<Room> Create(string name)
	{
		var validationResult = Validate(name);

		if (!validationResult.IsSuccess)
			return Result.Invalid(validationResult.ValidationErrors);

		return new Room(validationResult.Value);
	}

	public Result<Room> Update(string name)
	{
		var validationResult = Validate(name);

		if (!validationResult.IsSuccess)
			return Result.Invalid(validationResult.ValidationErrors);

		Name = validationResult.Value;

		return Result.Success(this);
	}

	public static Result<string> Validate(string name)
	{
		var nameValue = name.Trim();

		if (string.IsNullOrEmpty(nameValue))
			return Result.Invalid(
				new ValidationError(nameof(Name), "The name of the Room must not be empty")
				);

		return Result.Success(nameValue);
	}

	public Room() { }
}
