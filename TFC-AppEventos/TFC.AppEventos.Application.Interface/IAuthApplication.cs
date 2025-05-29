using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Application.Main
{
    public interface IAuthApplication
    {
        Task<LoginResponse> Login(AuthDto authDto);
        Task<RegisterResponse> Register(RegisterDTO authDto);
        Task<ChangeRoleResponse> RegisterAsOrganizer(AuthDto authDto);
    }
}