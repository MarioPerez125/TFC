using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Transversal.Utils;

namespace TFC.AppEventos.Application.DTO.Responses
{
    public class LoginResponse : BaseResponse
    {
        public AuthDto AuthDto { get; set; }
        public UserDto User { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
