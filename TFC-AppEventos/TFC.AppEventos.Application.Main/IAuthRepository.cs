using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Application.Main
{
    public interface IAuthRepository
    {
        Task<LoginResponse?> Login(AuthDto authDto);
        Task<RegisterResponse> Register(AuthDto authDto);
    }
}