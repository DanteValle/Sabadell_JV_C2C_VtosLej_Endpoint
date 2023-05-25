using System.ComponentModel.DataAnnotations;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Models
{
    public class Lead
    {
        public int ID_LEAD { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El campo NOMBRE solo puede contener letras")]
        [Required(ErrorMessage = "El campo NOMBRE es obligatorio")]
        [MaxLength(40, ErrorMessage = "el campo NOMBRE debe tener menos de 40 caracteres")]
        public string NOMBRE { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El campo PRIMER APELLIDO solo puede contener letras")]
        [MaxLength(30, ErrorMessage = "el campo PRIMER APELLIDO debe tener menos de 30 caracteres")]
        public string? PRIMER_APELLIDO { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El campo SEGUNDO APELLIDO solo puede contener letras")]
        [MaxLength(30, ErrorMessage = "el campo SEGUNDO APELLIDO debe tener menos de 30 caracteres")]
        public string? SEGUNDO_APELLIDO { get; set; }

        [Required(ErrorMessage = "El campo DNI es obligatorio")]
        [MaxLength(10, ErrorMessage = "el campo DNI debe tener menos de 10 caracteres")]
        public string DNI { get; set; }
        [Required(ErrorMessage = "El campo PRODUCTO es obligatorio")]
        [MaxLength(20, ErrorMessage = "el campo PRODUCTO debe tener menos de 20 caracteres")]
        public string PRODUCTO { get; set; }
        [Required(ErrorMessage = "El campo TELEFONO es obligatorio")]
        [MaxLength(12, ErrorMessage = "el campo PRODUCTO debe tener menos de 12 caracteres")]
        public string TELEFONO { get; set; }
        [Required(ErrorMessage = "El campo IDIOMA es obligatorio")]
        [MaxLength(10, ErrorMessage = "el campo IDIOMA debe tener menos de 10 caracteres")]
        public string IDIOMA { get; set; }
        [MaxLength(2, ErrorMessage = "el campo MES NACIMIENTO debe tener menos de 2 caracteres")]
        public string? MES_NACIMIENTO { get; set; }
        [MaxLength(30, ErrorMessage = "el campo DEPARTAMENTO debe tener menos de 30 caracteres")]
        public string? DEPARTAMENTO { get; set; }
    }
}
