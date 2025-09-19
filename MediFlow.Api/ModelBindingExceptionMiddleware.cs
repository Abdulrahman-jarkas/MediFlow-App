using SharedKernal;
using System.Text.Json;

namespace MediFlow.Api;

public class ModelBindingExceptionMiddleware
{
	private readonly RequestDelegate _next;

	public ModelBindingExceptionMiddleware(RequestDelegate next) => _next = next;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (BadHttpRequestException ex) when (ex.InnerException is JsonException jsonEx)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.ContentType = "application/json";

			var field = ExtractFieldName(jsonEx.Path);

			await context.Response.WriteAsJsonAsync(ApiResult.Error([new ApiError("Invalid Data", field)]));
		}
	}

	private string ExtractFieldName(string? jsonPath)
	{
		if (string.IsNullOrWhiteSpace(jsonPath))
			return "requestBody";

		// Remove root "$." if present
		if (jsonPath.StartsWith("$."))
			jsonPath = jsonPath.Substring(2);

		return jsonPath;
	}
}