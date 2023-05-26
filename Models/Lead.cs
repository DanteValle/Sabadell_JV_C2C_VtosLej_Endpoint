using System.ComponentModel.DataAnnotations;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Models
{
    public class Lead
    {
        public int ID_LEAD { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public string? NOMBRE { get; set; }
        public string? PRIMER_APELLIDO { get; set; }
        public string? SEGUNDO_APELLIDO { get; set; }
        public string? DNI { get; set; }
        public string? PRODUCTO { get; set; }
        public string? TELEFONO { get; set; }
        public string? IDIOMA { get; set; }
        public string? MES_NACIMIENTO { get; set; }
        public string? DEPARTAMENTO { get; set; }
    }
}
