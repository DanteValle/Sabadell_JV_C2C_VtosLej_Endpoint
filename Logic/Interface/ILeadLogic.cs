using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface
{
    public interface ILeadLogic
    {
        string GetValidationMessage(Data data);
        Task<string> AddLeads(Data data);
    }
}
