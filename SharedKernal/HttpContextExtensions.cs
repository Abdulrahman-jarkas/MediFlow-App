using Microsoft.AspNetCore.Http;

namespace SharedKernal;

public static class HttpContextExtensions
{
	public static Guid GetUserId(this HttpContext context)
	{
		var userId = context?.User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;

		if (Guid.TryParse(userId, out var guid))
		{
			return guid;
		}

		return Guid.Empty;
	}
}
