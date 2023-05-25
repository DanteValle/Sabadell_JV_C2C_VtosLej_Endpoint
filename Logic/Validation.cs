using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic
{
    public class Validation : IValidation
    {
        
        public async Task<bool> ValidateLeads(Data leave) {
            foreach (var item in leave.Leads)
            {
                return await ValidateLead(item);
            }
            return true;
        }

        public async Task<bool> ValidateLead(Lead lead)
        {
            var validationResults = new List<ValidationResult>();

            if (!await Task.Run(() => Validator.TryValidateObject(lead, new ValidationContext(lead), validationResults, true)))
            {
                // El objeto Lead no es válido. Puedes manejar los errores de validación aquí.
                throw new ValidationException("El objeto lead no es válido");
            }

            return true;
        }


       

        public async Task<int> RunProcessAsAdmi(string pathExecutable) {
            try
            {
               await WriteLog(null, "INFO", $"Executing file --> {pathExecutable}");

                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", $"/c {pathExecutable}");
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.ErrorDialog = true;
                startInfo.Verb = "runas";
                startInfo.CreateNoWindow = true;

                await WriteLog(null, "INFO", $"Start process Altitud for: {pathExecutable}");
                using (Process process = Process.Start(startInfo))
                {
                    while (!process.HasExited)
                    {
                        await Task.Delay(100);
                    }

                    await WriteLog(null, "INFO", $"End process Altitud with code: {process.ExitCode}");
                    return 1;
                }
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                await WriteLog(null, "ERROR", $"In RunProcessAsAdmin: {ex.Message}");
                return ex.NativeErrorCode;
            }
            catch (Exception ex)
            {
                await WriteLog(null, "ERROR", $"In RunProcessAsAdmin: {ex.Message}");
                return -2;
            }
        }

        public async Task WriteLog(Data objCTC, string method, string message) {
            try
            {
                StringBuilder strRequest = new StringBuilder();

                switch (method)
                {
                    case "INFO":
                        strRequest.Append(" - INFO        : ");
                        break;
                    case "ERROR":
                        strRequest.Append(" - ERROR       : ");
                        break;
                    case "EXCEPTION":
                        strRequest.Append(" - EXCEPTION   : ");
                        break;
                    default:
                        strRequest.Append($" - POST WebMethod {method} : Parameters --> ");
                        break;
                }

                if (objCTC != null)
                {
                    strRequest.Append(JsonSerializer.Serialize(objCTC.Leads));
                }

                if (!string.IsNullOrEmpty(message))
                {
                    strRequest.Append($" DETAIL : {message} ");
                }

                string logFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", $"{DateTime.Now.ToShortDateString().Replace("/", "")}_SabadelBlink.log");

                FileInfo fileInfo = new FileInfo(logFileName);
                bool append = fileInfo.Exists && fileInfo.Length <= 25000000;

                string fullline = $"{DateTime.Now:yyyy-MM-dd HH.mm.ss.fff}\t{strRequest}";

                using (StreamWriter writer = new StreamWriter(logFileName, append))
                {
                    await writer.WriteLineAsync(fullline);
                }
            }
            catch (Exception ex)
            {
                await WriteLog(null, "EXCEPTION", $" In Method WriteLog: {ex.Message}");
            }
        }

        public bool IsNull(object toCheck) => toCheck == null;

    }
}
