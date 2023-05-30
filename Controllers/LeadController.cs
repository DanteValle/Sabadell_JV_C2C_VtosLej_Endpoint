using Microsoft.AspNetCore.Components.Forms;
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

        public ILog _log;
        public ILeadLogic _leadLogic;
        public LeadController(ILeadLogic lead, ILog log)
        {
            _leadLogic = lead;
            _log = log;
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
        public async Task<IActionResult> AgregarLeads(Data data) {
            try
            {
                _log.WriteLog(null, "C2C", "Inicio C2C");
                string validationMessage = _leadLogic.GetValidationMessage(data);

                if (!string.IsNullOrEmpty(validationMessage))
                {
                    _log.WriteLog(data, "ERROR", "VALIDACION"+ validationMessage);
                    return BadRequest(validationMessage);
                }
                var response = await _leadLogic.AddLeads(data);
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //static void LogMessage(string message)
        //{
        //    string logFolderPath = "Log"; // Especifica el nombre de la carpeta "Log" dentro de tu proyecto
        //    string fileName = "archivo.txt"; // Especifica el nombre del archivo

        //    // Combina la ruta de la carpeta de logs y el nombre del archivo
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFolderPath, fileName);
        //    // Obtiene la fecha y hora actual para incluir en el mensaje de log
        //    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //    // Construye el mensaje de log con la fecha, hora y el mensaje proporcionado
        //    string logMessage = $"[{timestamp}] {message}";

        //    // Escribe el mensaje de log en el archivo
        //    using (StreamWriter writer = new StreamWriter(filePath, true))
        //    {
        //        writer.WriteLine(logMessage);
        //    }
        //}
    }
}
