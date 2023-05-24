using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {

        public ILeadLogic _leadLogic;
        public LeadController(ILeadLogic lead) {
            _leadLogic = lead;
        }

        [HttpGet("/api/Lead/Hola")]
        public async Task<ActionResult> Hola()
        {
            try
            {

                return Ok("holaa");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("/api/Lead/AgregarLeads")]
        public async Task<string> AgregarLeads(Data data) {
                var response = await _leadLogic.Agregar_lead(data);
            
            return response;
        }
    }
}
