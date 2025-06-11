using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace OrganizerWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public AuthDto Auth { get; set; }
        [BindProperty]
        public RegisterDTO Register { get; set; } // Para el formulario de registro
        public string? Result { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            // 1. Registro normal
            var registerResponse = await client.PostAsJsonAsync("api/auth/register", Register);
            if (!registerResponse.IsSuccessStatusCode)
            {
                Result = "Error en el registro: " + await registerResponse.Content.ReadAsStringAsync();
                return Page();
            }

            // 2. Registro como organizador (usa username y password del registro)
            var authDto = new AuthDto { Username = Register.Username, Password = Register.Password };
            var organizerResponse = await client.PostAsJsonAsync("api/auth/register-as-organizer", authDto);
            if (!organizerResponse.IsSuccessStatusCode)
            {
                Result = "Error al asignar rol de organizador: " + await organizerResponse.Content.ReadAsStringAsync();
                return Page();
            }

            // 3. Login automático
            var loginResponse = await client.PostAsJsonAsync("api/auth/login", authDto);
            if (loginResponse.IsSuccessStatusCode)
            {
                var result = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.IsSuccess == true)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.User.UserId.ToString()),
                        new Claim(ClaimTypes.Name, result.User.Username)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToPage("MyTournaments", new { organizerId = result.User.UserId });
                }
            }

            Result = "Registro exitoso, pero error al iniciar sesión.";
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("api/auth/login", Auth);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.IsSuccess == true)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.User.UserId.ToString()),
                        new Claim(ClaimTypes.Name, result.User.Username)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToPage("MyTournaments", new { organizerId = result.User.UserId });
                }
                else
                {
                    Result = "Error en el login";
                }
            }
            else
            {
                Result = "Error en el login";
            }

            return Page();
        }
    }
}