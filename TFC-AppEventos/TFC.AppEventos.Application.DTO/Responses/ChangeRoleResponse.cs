using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Transversal.Utils;

namespace TFC.AppEventos.Application.DTO.Responses
{
    public class ChangeRoleResponse : BaseResponse
    {
        public UserDto User { get; set; }
    }
}
