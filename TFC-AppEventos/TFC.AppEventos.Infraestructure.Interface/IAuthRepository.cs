using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Infraestructure.Interface.IAuthRepository
{
    public interface IAuthRepository
    {
        Task<LoginResponse> Login(AuthDto authDto);
        Task<RegisterResponse> Register(AuthDto authDto);
        Task<RegisterResponse> RegisterAsOrganizer(AuthDto authDto);
    }
}