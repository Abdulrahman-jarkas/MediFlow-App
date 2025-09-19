using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace MediFlow.Api;

public class AccessTokenHandler : DelegatingHandler
{
	private readonly IHttpContextAccessor _contextAccessor;

	public AccessTokenHandler(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

		if (!string.IsNullOrWhiteSpace(accessToken))
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}
