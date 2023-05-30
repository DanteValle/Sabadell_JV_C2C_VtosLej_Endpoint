using Sabadell_JV_C2C_VtosLej_Endpoint.Models;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface
{
    public interface ILog
    {
        void WriteLog(Data? objCTC, string method = "", string? message = null);
    }
}
