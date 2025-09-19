using Ardalis.Result;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace SharedKernal;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TResponse : IResult
	where TRequest : class
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

		if(!validationResults.Any(v => v.IsValid))
		{
			var errors = validationResults.Select(v => v.ToValidationErrors()).SelectMany(x => x).ToList();

			var genericResultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);

			var method = genericResultType
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.First(m =>
					m.Name == nameof(Result<object>.Invalid) &&
					m.GetParameters().Length == 1 &&
					m.GetParameters()[0].ParameterType == typeof(IEnumerable<ValidationError>)
				);

			var result = method.Invoke(null, new object[] { errors });
			return (TResponse)result!;
		}

		return await next();
	}
}