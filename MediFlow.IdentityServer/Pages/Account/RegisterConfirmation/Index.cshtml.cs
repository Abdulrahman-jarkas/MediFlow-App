using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediFlow.IdentityServer.Pages.RegisterConfirmation;

[AllowAnonymous]
public class IndexModel : PageModel
{

    public IActionResult OnGet()
    {
        return Page();
    }
}

