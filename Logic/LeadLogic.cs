using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;


namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic
{
    public class LeadLogic : ILeadLogic
    {
        public ILeadDataAcces _leadDataAcces;

        public IValidator<Data> _validator;

        public LeadLogic(ILeadDataAcces leadDataAcces, IValidator<Data> validator) { 
            _leadDataAcces = leadDataAcces;
            _validator = validator;
        }
        public string GetValidationMessage(Data data)
        {
            ValidationResult result = _validator.Validate(data);
            string validationMessage = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
            return validationMessage;
        }
        public async Task<List<int>> AddLeads(Data data) {
            try
            {
                var response = await _leadDataAcces.AddLead(data);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
            
                 
        }
    }
}
