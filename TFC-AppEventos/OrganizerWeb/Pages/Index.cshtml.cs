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
        public string? Result { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("api/auth/register-as-organizer", Auth);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ChangeRoleResponse>();
                Result = result?.IsSuccess == true ? "Registro exitoso" : "Error en el registro";
            }
            else
            {
                Result = "Error en el registro";
            }

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
                    // Crear claims de autenticación
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.User.UserId.ToString()),
                new Claim(ClaimTypes.Name, result.User.Username)
                // Puedes agregar más claims si lo necesitas
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