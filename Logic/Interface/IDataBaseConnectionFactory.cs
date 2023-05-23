using System.Data;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface
{
    public interface IDataBaseConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}
