using Microsoft.AspNetCore.Mvc;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic
{
    public class LeadLogic : ILeadLogic
    {
        public ILeadDataAcces _leadDataAcces;

        public LeadLogic(ILeadDataAcces leadDataAcces) { 
            _leadDataAcces = leadDataAcces;
        }
       public async Task<string> Agregar_lead(Data lead) {

            var response = await _leadDataAcces.AddLead(lead);

            return response;       
        }
    }
}
