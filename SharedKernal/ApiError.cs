using System.Text.Json.Serialization;

namespace SharedKernal;

public class ApiError
{
	public string Message { get; } = string.Empty;
	public string Path { get; } = string.Empty;

	public ApiError(string message, string path)
	{
		Message = message;
		Path = path;
	}
}

public class ApiResult
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public List<ApiError>? Errors { get; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public object? Data { get; }

	private ApiResult(IEnumerable<ApiError>? errors, object? data)
	{
		if (errors is not null) Errors = errors.ToList();
		if (data is not null) Data = data;
	}

	public static ApiResult Ok(object? data)
	{
		return new ApiResult(null, data);
	}

	public static ApiResult Error(IEnumerable<ApiError> errors)
	{
		return new ApiResult(errors, null);
	}
}
