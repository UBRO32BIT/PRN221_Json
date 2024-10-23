using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using SalesWPFApp.Repositories.Interfaces;

namespace SalesWPFApp_RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IMemberRepository _memberRepository;
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();
        public string ReturnUrl { get; set; } = string.Empty;
        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public LoginModel(IMemberRepository accountService)
        {
            _memberRepository = accountService;
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }
        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
        }

        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var account = _memberRepository.GetMemberByEmailAndPassword(Input.Email, Input.Password);

                if (account == null)
                {
                    ModelState.AddModelError(string.Empty, "Login Attempt Failed");
                    return Page();
                }

                // Create claims (you can add more if needed, like role, name, etc.)
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()) // If you have roles
            };

                // Create identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create principal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return Redirect(returnUrl);
            }

            return Page();
        }
    }
}
