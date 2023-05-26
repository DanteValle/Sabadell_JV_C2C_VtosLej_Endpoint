using FluentValidation;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Validation
{
    public class DataValidator: AbstractValidator<Data>
    {
        public DataValidator()
        {
            RuleFor(data => data.Leads).NotNull().WithMessage("La lista de leads no puede ser nula.");

            RuleForEach(data => data.Leads)
                .SetValidator(new LeadValidator());
        }
    }
}
