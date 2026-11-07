using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteriaManagement.Controllers;

public class AccountController(IConfiguration configuration) : Controller
{
    [AllowAnonymous, HttpGet]
    public IActionResult Login(string? returnUrl = null) { ViewBag.ReturnUrl = returnUrl; return View(); }

    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        var expectedUser = configuration["EmployeeAccount:Username"] ?? "";
        var expectedPassword = configuration["EmployeeAccount:Password"] ?? "";
        var valid = expectedUser.Length > 0 && expectedPassword.Length > 0 &&
            FixedEquals(username ?? "", expectedUser) && FixedEquals(password ?? "", expectedPassword);
        if (!valid) { ViewBag.ReturnUrl = returnUrl; ModelState.AddModelError("", "Invalid username or password."); return View(); }

        var claims = new[] { new Claim(ClaimTypes.Name, expectedUser), new Claim(ClaimTypes.Role, "Employee") };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction("Index", "Management");
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private static bool FixedEquals(string supplied, string expected)
    {
        var left = Encoding.UTF8.GetBytes(supplied);
        var right = Encoding.UTF8.GetBytes(expected);
        return left.Length == right.Length && System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(left, right);
    }
}
