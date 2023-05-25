using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface
{
    public interface IValidation
    {
        Task<bool> ValidateLeads(Data leave);

        Task<int> RunProcessAsAdmi(string pathExecutable);

        Task WriteLog(Data objCTC, string method, string message);

    }
}
