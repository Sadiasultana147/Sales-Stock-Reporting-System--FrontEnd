using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSRS.Web.Areas.Admin.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSRS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid form data." });

            var jsonContent = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsync("/api/Auth/register", content);

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, message = "Registration successful!" });

            return Json(new { success = false, message = "Registration failed. Please try again." });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromQuery] string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            var jsonContent = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsync("/api/Auth/login", content);

            if (!response.IsSuccessStatusCode)
                return Json(new { success = false, message = "Login failed. Check User/Password." });

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseBody);

            if (!jsonDoc.RootElement.TryGetProperty("token", out var tokenElement))
                return Json(new { success = false, message = "Token not received from API" });

            var token = tokenElement.GetString();

            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Received an empty token from API" });

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);

                var usernameClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

                if (usernameClaim == null)
                    return Json(new { success = false, message = "Username not found in the token" });

                var username = usernameClaim.Value;

                HttpContext.Session.SetString("JWToken", token);
                HttpContext.Session.SetString("Username", username);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("JWT", token)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return Json(new
                {
                    success = true,
                    message = "Login successful!",
                    redirectUrl = string.IsNullOrEmpty(returnUrl) ? "/Products/Product" : returnUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error decoding token: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
    }
}
