using FluentValidation;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Validation
{
    public class LeadValidator: AbstractValidator<Lead>
    {
        public LeadValidator() 
        {
            RuleFor(l => l.NOMBRE).NotEmpty().MaximumLength(40).WithMessage("{PropertyName} esta vacio o supero el maximo de 40 caracteres");
            RuleFor(l => l.PRIMER_APELLIDO).MaximumLength(30).WithMessage("PRIMER APELLIDO supero el maximo de 40 caracteres");
            RuleFor(l => l.SEGUNDO_APELLIDO).MaximumLength(30).WithMessage("SEGUNDO APELLIDO supero el maximo de 40 caracteres");
            RuleFor(l => l.DNI).NotEmpty().MaximumLength(10).WithMessage("DNI esta vacio o supero el maximo de 40 caracteres");
            RuleFor(l => l.PRODUCTO).NotEmpty().MaximumLength(20).WithMessage("PRODUCTO esta vacio o supero el maximo de 40 caracteres");
            RuleFor(l => l.TELEFONO).NotEmpty().MaximumLength(12).WithMessage("TELEFONO esta vacio o supero el maximo de 40 caracteres");
            RuleFor(l => l.IDIOMA).NotEmpty().MaximumLength(10).WithMessage("IDIOMA esta vacio o supero el maximo de 40 caracteres");
            RuleFor(l => l.MES_DEL_VENCIMIENTO).MaximumLength(2).WithMessage("MES VENCIMIENTO supero el maximo de 40 caracteres");
            RuleFor(l => l.DEPARTAMENTO).MaximumLength(40).WithMessage("DEPARTAMENTO supero el maximo de 40 caracteres");

        }
    }
}
