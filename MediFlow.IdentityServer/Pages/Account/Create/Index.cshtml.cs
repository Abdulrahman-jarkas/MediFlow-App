using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Marvin.IDP.Entities;
using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace MediFlow.IdentityServer.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly ILocalUserService _localUserService;
    private readonly IIdentityServerInteractionService _interaction;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public Index(
        IIdentityServerInteractionService interaction,
        ILocalUserService localUserService = null)
    {
        // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
        _localUserService = localUserService ?? throw new InvalidOperationException("Please call 'AddTestUsers(TestUsers.Users)' on the IIdentityServerBuilder in Startup or remove the TestUserStore from the AccountController.");

        _interaction = interaction;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "create")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl ?? "~/");
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if (await _localUserService.GetUserByUserNameAsync(Input.Username) != null)
        {
            ModelState.AddModelError("Input.Username", "Invalid username");
        }

        if (ModelState.IsValid)
        {
            var localUser = new User()
            {
                Active = false,
                UserName = Input.Username,
                Subject = Guid.NewGuid().ToString(),
                Email = Input?.Email ?? ""
            };

			localUser.SecurityCode = Convert.ToBase64String(RandomNumberGenerator.GetBytes(120));
            localUser.SecurityCodeExpirationDate = DateTime.UtcNow.AddSeconds(30);

            var activationLinke = Url.PageLink("/Account/Activation/Index",
                values: new { securityCode = localUser.SecurityCode }
                );

            Console.WriteLine(activationLinke);

			 _localUserService.AddUser(localUser, Input.Password!);
            await _localUserService.SaveChangesAsync();

            localUser.Claims.Add(new UserClaim()
            {
                Type = "role",
                Value= "clinic"
            });

            return Redirect("/Account/RegisterConfirmation/Index");

            // issue authentication cookie with subject ID and username
            //var isuser = new IdentityServerUser(localUser.Subject)
            //{
            //    DisplayName = localUser.UserName
            //};

            //await HttpContext.SignInAsync(isuser);

            //if (context != null)
            //{
            //    if (context.IsNativeClient())
            //    {
            //        // The client is native, so this change in how to
            //        // return the response is for better UX for the end user.
            //        return this.LoadingPage(Input.ReturnUrl);
            //    }

            //    // we can trust Input.ReturnUrl since GetAuthorizationContextAsync returned non-null
            //    return Redirect(Input.ReturnUrl ?? "~/");
            //}

            //// request for a local page
            //if (Url.IsLocalUrl(Input.ReturnUrl))
            //{
            //    return Redirect(Input.ReturnUrl);
            //}
            //else if (string.IsNullOrEmpty(Input.ReturnUrl))
            //{
            //    return Redirect("~/");
            //}
            //else
            //{
            //    // user might have clicked on a malicious link - should be logged
            //    throw new ArgumentException("invalid return URL");
            //}
        }

        return Page();
    }
}
