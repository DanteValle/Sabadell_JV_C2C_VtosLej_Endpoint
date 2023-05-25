using Microsoft.AspNetCore.Mvc;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic
{
    public class LeadLogic : ILeadLogic
    {
        public ILeadDataAcces _leadDataAcces;

        public IValidation _validation;

        public LeadLogic(ILeadDataAcces leadDataAcces, IValidation validation) { 
            _leadDataAcces = leadDataAcces;
            _validation = validation;
        }
       public string response = "no añadido";
       public async Task<string> Agregar_lead(Data lead) {
            var validation = await _validation.ValidateLeads(lead);
            if (validation) {
                response = await _leadDataAcces.AddLead(lead);
                return response = "añadido correctamente";
            }
            return response;
            
                 
        }
    }
}
