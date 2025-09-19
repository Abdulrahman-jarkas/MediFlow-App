using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MediFlow.IdentityServer.Pages.Account.Activation;

[AllowAnonymous]
[SecurityHeaders]
public class IndexModel : PageModel
{
	private readonly ILocalUserService localUserService;

	public IndexModel(ILocalUserService localUserService)
	{
		this.localUserService = localUserService;
	}

	[BindProperty]
	public InputModel Input { get; set; } = new InputModel();

	public async Task OnGet(string securityCode)
    {
		
		var res = await localUserService.ActivateUser(securityCode);

		if (res)
		{
			Input.Message = "Account Activated Successfully";
		}
		else
		{
			Input.Message = "Faild to Activate Account";
		}

		await localUserService.SaveChangesAsync();
	}
}
