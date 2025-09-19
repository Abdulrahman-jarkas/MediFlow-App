
using Microsoft.AspNetCore.Routing;

public interface ISlice
{
	void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}
