using Ardalis.Result;
using SharedKernal;

namespace MediFlow.ClinicManagement.Domain.Entities;

internal class Equipment : Entity
{
	public string Name { get; private set; } = string.Empty;
	public decimal Fees { get; private set; }

	private Equipment(string name, decimal fees)
	{
		Name = name;
		Fees = fees;
	}

	public static Result<Equipment> Create(string name, decimal fees)
	{
		var validationResult = Validate(name, fees);

		if (!validationResult.IsSuccess)
			return Result.Invalid(validationResult.ValidationErrors);

		var (nameValue, feeValue) = validationResult.Value;

		return Result.Created(new Equipment(nameValue, feeValue));
	}

	public Result<Equipment> Update(string name, decimal fee)
	{
		Fees = fee;
		Name = name;

		return Result.Success(this);
	}

	public static Result<(string name, decimal fee)> Validate(string name, decimal fee)
	{
		var errors = new List<ValidationError>();

		if (string.IsNullOrEmpty(name.Trim()))
			errors.Add(new ValidationError(nameof(Name), "The name of tool must not be empty"));


		if (fee < 0)
			errors.Add(new ValidationError(nameof(Fees), "The fee of tool must not be less than zero"));

		return errors.Any()
			? Result.Invalid(errors) : Result.Success((name.Trim(), fee));
	}

	public Equipment() { }
}
