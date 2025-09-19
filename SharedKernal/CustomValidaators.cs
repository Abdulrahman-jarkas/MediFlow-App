using Ardalis.Result;
using FluentValidation;

namespace SharedKernal;

public static class CustomValidators
{
	public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TValueObject>(
		this IRuleBuilder<T, TElement> ruleBuilder,
		Func<TElement, Result<TValueObject>> factoryMethod)
		where TValueObject : Entity
	{
		return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
		{
			Result<TValueObject> result = factoryMethod(value);

			if (!result.IsSuccess)
			{
				foreach (var error in result.ValidationErrors)
				{
					//context.AddFailure(error.ErrorMessage);
					context.AddFailure(
						new FluentValidation.Results.ValidationFailure(
							context.PropertyPath + "." + error.Identifier,
							error.ErrorMessage));
				}
			}
		});
	}
}
