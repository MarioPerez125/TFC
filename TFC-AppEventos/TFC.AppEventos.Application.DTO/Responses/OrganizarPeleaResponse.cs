﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Transversal.Utils;

namespace TFC.AppEventos.Application.DTO.Responses
{
    public class OrganizarPeleaResponse : BaseResponse
    {
        public string NombrePeleador1 { get; set; }
        public string NombrePeleador2 { get; set; }
    }
}
