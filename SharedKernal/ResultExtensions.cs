using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using SharedKernal;

public static class ResultExtensions
{
	private static IEnumerable<ApiError> Errros<T>(Result<T> result)
	{
		return result.ValidationErrors
		.Select(e => new ApiError(e.ErrorMessage, e.Identifier))
		.Concat(result.Errors.Select(e => new ApiError(e, string.Empty)));
	}

	public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult<T>(this Result<T> result, string? uri = "")
	{
		return result.Status switch
		{
			ResultStatus.Ok => Results.Ok(ApiResult.Ok(result.Value)),

			ResultStatus.Created => !string.IsNullOrWhiteSpace(result.Location) && result.Value is not null
				? Results.Created(result.Location, ApiResult.Ok(result.Value))
				: Results.Ok(ApiResult.Ok(result.Value)),

			ResultStatus.Conflict => Results.Conflict(ApiResult.Error(Errros(result))),

			ResultStatus.NotFound => Results.NotFound(ApiResult.Error(Errros(result))),

			ResultStatus.Invalid => Results.BadRequest(ApiResult.Error(Errros(result))),

			ResultStatus.Unauthorized => Results.Unauthorized(),

			ResultStatus.Forbidden => Results.StatusCode(StatusCodes.Status403Forbidden),

			ResultStatus.NoContent => Results.NoContent(),

			ResultStatus.Error => Results.Problem(
				title: "Internal Server Error",
				detail: string.Join("; ", ApiResult.Error(Errros(result))),
				statusCode: StatusCodes.Status500InternalServerError),

			_ => Results.StatusCode(StatusCodes.Status500InternalServerError)
		};
	}
}
