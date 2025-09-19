using Ardalis.Result;
using SharedKernal;

namespace MediFlow.ClinicManagement.Domain.Entities;

internal class Medicine : Entity
{
	public string Name { get; private set; } = string.Empty;
	public decimal Price { get; private set; }
	public int Quantity { get; private set; }

	private Medicine(string name, decimal price, int quantity)
	{
		Name = name;
		Price = price;
		Quantity = quantity;
	}

	public static Result<Medicine> Create(string name, decimal price, int quantity)
	{
		var validateResult = Validate(name, price, quantity);

		if (!validateResult.IsSuccess)
			return Result.Invalid(validateResult.ValidationErrors);

		var (nameValue, priceValue, quantityValue) = validateResult.Value;

		return Result.Created(new Medicine(nameValue, priceValue, quantityValue));
	}

	public Result<Medicine> Update(string name, decimal price, int quantity)
	{
		var validateResult = Validate(name, price, quantity);

		if (!validateResult.IsSuccess)
			return Result.Invalid(validateResult.ValidationErrors);

		Name = name;
		Price = price;
		Quantity = quantity;

		return Result.Success(this);
	}

	public static Result<(string name, decimal price, int quntity)>
		Validate(string name, decimal price, int quantity)
	{
		var errors = new List<ValidationError>();

		if (string.IsNullOrEmpty(name.Trim()))
			errors.Add(new ValidationError(nameof(Name), "The name of medicine must not be empty"));


		if (price < 0)
			errors.Add(new ValidationError(nameof(Price), "The price of medicine must not be less than zero"));

		if (quantity < 0)
			errors.Add(new ValidationError(nameof(Quantity), "The quantity of medicine must not be less than zero"));

		return errors.Any()
			? Result.Invalid(errors) : Result.Success((name.Trim(), price, quantity));
	}

	public Medicine() { }
}