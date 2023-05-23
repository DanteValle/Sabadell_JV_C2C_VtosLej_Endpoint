using Dapper;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.DataAcces
{
    
    public class LeadDataAcces: ILeadDataAcces
    {
        private readonly IDataBaseConnectionFactory _dataBaseConnection;

        public LeadDataAcces(IDataBaseConnectionFactory dataBaseConnection) { 
            _dataBaseConnection = dataBaseConnection;
        }
        public async Task<int>  AddLead(Data lead) {
            try
            {
                int id_log = -1;
                using (var connection = _dataBaseConnection.GetDbConnection())
                {
                    foreach (var item in lead.Leads)
                    {

                        item.FECHA_ALTA = DateTime.Now;
                        var parameters = new DynamicParameters();
                        parameters.Add("@FechaAlta", item.FECHA_ALTA, DbType.DateTime);
                        parameters.Add("@Nombre", item.NOMBRE, DbType.String);
                        parameters.Add("@PrimerApellido", item.PRIMER_APELLIDO, DbType.String);
                        parameters.Add("@SegundoApellido", item.SEGUNDO_APELLIDO, DbType.String);
                        parameters.Add("@DNI", item.DNI, DbType.String);
                        parameters.Add("@Producto", item.PRODUCTO, DbType.String);
                        parameters.Add("@Telefono", item.TELEFONO, DbType.String);
                        parameters.Add("@Idioma", item.IDIOMA, DbType.String);
                        parameters.Add("@MesDelVencimiento", item.MES_NACIMIENTO, DbType.String);
                        parameters.Add("@Departamento", item.DEPARTAMENTO, DbType.String);
                        parameters.Add("@idLog", DbType.Int32, direction: ParameterDirection.Output);

                        await connection.ExecuteAsync("WS_C2C_VtoLej_LOG ", parameters, commandType: CommandType.StoredProcedure);

                        id_log = parameters.Get<int>("@idLog");
                        
                    }
                    return id_log;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
