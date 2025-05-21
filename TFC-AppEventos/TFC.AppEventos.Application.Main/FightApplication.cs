using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Infraestructure.Interface;

namespace TFC.AppEventos.Application.Main
{
    public class FightApplication : IFightApplication
    {
        private readonly IFightRepository _fightRepository;
        public FightApplication(IFightRepository fightRepository)
        {
            _fightRepository = fightRepository;
        }

        public async Task<OrganizarPeleaResponse> ScheduleFight(FightDto fightDto)
        {
            OrganizarPeleaResponse response = new OrganizarPeleaResponse();
            try
            {
                if (fightDto == null )
                    {
                    throw new Exception("Los datos de la pelea no pueden ser nulos");

                }

                return await _fightRepository.ScheduleFight(fightDto);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error durante la programación de la pelea: {ex.Message}";
                return response;
            }
        }
    }
}
