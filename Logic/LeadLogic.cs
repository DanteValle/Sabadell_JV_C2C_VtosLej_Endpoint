using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;
using System.Text;
using System.Text.Json;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic
{
    public class LeadLogic : ILeadLogic
    {
        public ILeadDataAcces _leadDataAcces;

        public IValidator<Data> _validator;

        public LeadLogic(ILeadDataAcces leadDataAcces, IValidator<Data> validator)
        {
            _leadDataAcces = leadDataAcces;
            _validator = validator;
        }
        public string GetValidationMessage(Data data)
        {
            ValidationResult result = _validator.Validate(data);
            string validationMessage = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            return validationMessage;
        }
        public async Task<List<int>> AddLeads(Data data)
        {
            try
            {
                WriteLog(data, "C2C", "INGRESO NORMAL");
                var response = await _leadDataAcces.AddLead(data);
                WriteLog(data, "C2C", "despues de bd:"+response);
                response.ForEach(x => WriteLog(null, "INFO", " Tablas de negocio alimentadas " + x));
                return response;
            }
            catch (Exception ex)
            {
                WriteLog(null, "C2C",ex.ToString());
                throw;
            }


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
