using Sabadell_JV_C2C_VtosLej_Endpoint.Models;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using System.Text.Json;
using System.Text;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Infrastructure
{
    public class Log : ILog
    {
        public Log()
        {
        }

        public void WriteLog(Data? objCTC, string method = "", string? message = null)
        {
            try
            {
                StringBuilder strRequest = new StringBuilder();
                strRequest.Append(method switch
                {
                    "INFO" => " - INFO        : ",
                    "ERROR" => " - ERROR       : ",
                    "EXCEPTION" => " - EXCEPTION   : ",
                    _ => $" - POST WebMethod {method} : Parameters --> "
                });
                if (objCTC != null)
                {
                    strRequest.Append(JsonSerializer.Serialize(objCTC.Leads));
                }
                if (!string.IsNullOrEmpty(message))
                {
                    strRequest.Append($" DETAIL : {message} ");
                }
                //var logFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", $"{DateTime.Now:yyyyMMdd}_SabadelBlink.log");
                var logFolder = Path.Combine(AppContext.BaseDirectory, "Log");
                var logFileName = Path.Combine(logFolder, $"{DateTime.Now:yyyyMMdd}_SabadelBlink.log");

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                var fileInfo = new FileInfo(logFileName);
                var append = fileInfo.Exists && fileInfo.Length <= 25000000;

                var now = DateTime.Now;
                var fullline = $"{now:yyyy-MM-dd HH.mm.ss.fff}\t{strRequest}";

                using (StreamWriter writer = new StreamWriter(logFileName, append))
                {
                    writer.WriteLine(fullline);
                }
            }
            catch (Exception ex)
            {

                WriteLog(null, "EXCEPTION", $" In Method WriteLog: {ex.Message}");
            }
        }
    }
}
