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
        public ILog _log;

        public IValidator<Data> _validator;

        public LeadLogic(ILeadDataAcces leadDataAcces, IValidator<Data> validator, ILog log)
        {
            _leadDataAcces = leadDataAcces;
            _validator = validator;
            _log = log;
        }
        public string GetValidationMessage(Data data)
        {
            ValidationResult result = _validator.Validate(data);
            string validationMessage = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            return validationMessage;
        }
        public async Task<string> AddLeads(Data data)
        {
            try
            {
                _log.WriteLog(data);
                var response = await _leadDataAcces.AddLead(data);
                return response;
            }
            catch (Exception ex)
            {
                _log.WriteLog(null, "EXCEPTION", ex.ToString());
                throw;
            }


        }

       
    }
}
