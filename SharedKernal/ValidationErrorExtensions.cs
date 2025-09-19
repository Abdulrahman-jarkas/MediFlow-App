using Ardalis.Result;
using FluentValidation.Results;

public static class ValidationResultExtensions
{
	public static IEnumerable<ValidationError> ToValidationErrors(this ValidationResult validationResult)
	{
		return validationResult.Errors
			.Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
			.ToList();
	}
}
