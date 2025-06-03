using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Main;

namespace OrganizerWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAuthApplication _authApplication;

        public IndexModel(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }

        [BindProperty]
        public AuthDto Auth { get; set; }
        public string? Result { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            ChangeRoleResponse response = await _authApplication.RegisterAsOrganizer(Auth);

            if (response.IsSuccess)
                Result = "Registro exitoso";
            else
                Result = "Error en el registro";

            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            LoginResponse response = await _authApplication.Login(Auth);

            if (response.IsSuccess)
            {
                // Redirige a la página de torneos, pasando el UserId como parámetro
                return RedirectToPage("MyTournaments", new { organizerId = response.User.UserId });
            }
            else
            {
                Result = "Error en el login";
                return Page();
            }
        }
    }
}
