﻿using Dapper;
using Microsoft.Data.SqlClient;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using Sabadell_JV_C2C_VtosLej_Endpoint.Models;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.DataAcces
{
    
    public class LeadDataAcces: ILeadDataAcces
    {
        private readonly IDataBaseConnectionFactory _dataBaseConnection;
        private readonly ILog _log;

        public LeadDataAcces(IDataBaseConnectionFactory dataBaseConnection,ILog log) { 
            _dataBaseConnection = dataBaseConnection;
            _log = log;
        }
        public async Task<string>AddLead(Data lead) {
            try
            {
                string DATs = "";

                string response = "";

                var idCliente = 0;


                using (var connection = _dataBaseConnection.GetDbConnection())
                {
                    foreach (var item in lead.Leads)
                    {

                        item.FECHA_ALTA = DateTime.Now;
                        var parameters = new DynamicParameters();
                        //parameters.Add("@FechaAlta", item.FECHA_ALTA, DbType.DateTime);
                        parameters.Add("@NOMBRE", item.NOMBRE, DbType.String);
                        parameters.Add("@PRIMER_APELLIDO", item.PRIMER_APELLIDO, DbType.String);
                        parameters.Add("@SEGUNDO_APELLIDO", item.SEGUNDO_APELLIDO, DbType.String);
                        parameters.Add("@DNI", item.DNI, DbType.String);
                        parameters.Add("@PRODUCTO", item.PRODUCTO, DbType.String);
                        parameters.Add("@TELEFONO", item.TELEFONO, DbType.String);
                        parameters.Add("@IDIOMA", item.IDIOMA, DbType.String);
                        parameters.Add("@MES_DEL_VENCIMIENTO", item.MES_DEL_VENCIMIENTO, DbType.String);
                        parameters.Add("@DEPARTAMENTO", item.DEPARTAMENTO, DbType.String);
                      //  parameters.Add("@id_cliente_salida", DbType.Int32, direction: ParameterDirection.Output);
                  
                        //await connection.ExecuteAsync("WSCargarMuestra ", parameters, commandType: CommandType.StoredProcedure);
                        //int idCliente = parameters.Get<int>("@id_cliente_salida");

                        idCliente = await connection.QuerySingleAsync<int>("dbo.WSCargarMuestra", parameters, commandType: CommandType.StoredProcedure)    
                    }
                    _log.WriteLog(null, "INFO", " Tablas de negocio alimentadas " + idCliente);
                    DATs = await GenerateDat();
                    ProcessLoadAndUpdateMarkers(idCliente, DATs);
                    return response;

                }
            }
            catch (Exception ex)
            {
                _log.WriteLog(null, "EXCEPTION", ex.ToString());
                throw;
            }
        }
        /// <summary>
        ///generarDat
        ///aunque le estamos pasando el idCliente al procedure, no se esta utilizando porque hacemos otro cruce que nos devuelve todos los dats de los registros que no estan en la tabla ct
        /// </summary>
        /// <param name="idCliente"></param>
        public async Task<string> GenerateDat()
        {
            try
            {

                //using (var con = _dataBaseConnection.GetDbConnection())
                //{
                //    //NO VENTAS
                //    var results = await con.QueryAsync<string>("dbo.WSGenerarDAT", commandType: CommandType.StoredProcedure);

                //    var builder = new StringBuilder();

                //    foreach (var result in results)
                //    {
                //        if (!string.IsNullOrEmpty(result))
                //        {
                //            builder.AppendLine(result);
                //        }
                //    }
                //    string DATs = builder.ToString();
                //    _log.WriteLog(null, "INFO", " Generar DAT recuperado " + DATs);
                //    return DATs;

                //}
                using (SqlConnection? con = _dataBaseConnection.GetDbConnection() as SqlConnection)
                { 
                    //NO VENTAS
                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = con;
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "dbo.WSGenerarDAT";
                        SqlDataReader reader = await com.ExecuteReaderAsync();
                        string DATs = "";
                        while (reader.Read())
                        {
                            if (reader.GetString(0) != null)
                            {
                                DATs += reader.GetString(0) + "\r\n";
                            }
                        }
                        _log.WriteLog(null, "INFO", " Generar DAT recuperado " + DATs);
                        return DATs;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        ///crearDat y alimentar tabla CT del marcador mediante UciLoader = UCI8 
        /// </summary>
        public void ProcessLoadAndUpdateMarkers(int idCliente, string DATs)
        {
            if (DATs.Length > 0)
            {
                String ruta = AppDomain.CurrentDomain.BaseDirectory + (@"Cargas\");
                string nomFicheroDat = idCliente + "Sabadell_JV_C2C_VtosLej_Endpoint" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + ".dat";
                string nomFicheroConRuta = ruta + nomFicheroDat;
                StreamWriter sw = new StreamWriter(nomFicheroConRuta, true);
                sw.Write(DATs);
                sw.Close();

                try
                {
                    //Le ponemos un nombre al fichero .bat que va a contener los comandos utilizados para realizar la carga en ALTITUDE.
                    string nomFicheroBat = idCliente + "Sabadell_JV_C2C_VtosLej_Test" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString();
                    //Establecemos la localización del fichero, la ruta.
                    string BatLocation = AppDomain.CurrentDomain.BaseDirectory + (@"Cargas\Carga_" + nomFicheroBat + ".bat");

                    FileStream fs2 = new FileStream(BatLocation, FileMode.Create, FileAccess.Write, FileShare.Write);

                    StreamWriter sw2 = new StreamWriter(fs2);

                    //sw2.Write("C:" + "\r\n");
                    sw2.Write("E:" + "\r\n");
                    sw2.Write("cd " + AppDomain.CurrentDomain.BaseDirectory + (@"Cargas\") + "\r\n");

                    //PRODUCCION
                    //sw2.Write("uciLoader\\uciLoader.exe -m uci0401:1500 -l uciCarga:uciCarga -f SABADELL_JV_C2C.TYP -i " + nomFicheroDat + " -c");// + "\r\n");// + "\r\n");

                    //TEST - comentar
                    //sw2.Write("uciLoader\\uciLoader.exe -m uci0401:1500 -l uciCarga:uciCarga -f Sabadell_JV_C2C_VtosLej.TYP -i " + nomFicheroDat + " -c");
                    sw2.Write("uciLoader\\uciLoader.exe -m uci0401:1500 -l uciCarga:uciCarga -f Sabadell_JV_C2C_VtosLej_Test.TYP -i " + nomFicheroDat + " -c ");// + "\r\n");// + "\r\n");
                    //sw2.Write("pause");
                    sw2.WriteLine();

                    //Cerramos los objetos relacionados con ficheros.
                    sw2.Close();
                    fs2.Close();

                    _log.WriteLog(null, "INFO", " Star EjecutarCarga " + idCliente);
                    int resp = RunProcessAsAdmin(BatLocation);
                    _log.WriteLog(null, "INFO", " End EjecutarCarga " + idCliente + " Con Respuesta: " + resp.ToString());
                }

                catch (Exception e)
                {
                    _log.WriteLog(null, "ERROR", " Error al realizar la carga en Altitude desde el método 'EjecutarCarga': " + e.Message);
                   
                }

                try
                {
                    //ActualizarEasyCodes
                    Thread.Sleep(1000);
                    using (var connection = _dataBaseConnection.GetDbConnection())
                    {
                        //var sql = "[dbo].[WSActualizarEasycodes]";
                        //var returnValue = connection.Execute(sql, commandType: CommandType.StoredProcedure);

                    }
                }
                catch (Exception ex)
                {
                    _log.WriteLog(null, "ERROR", " Error al ejecutar el SP WSActualizarEasycodes ': " + ex.Message);
                    throw;
                }
            }
        }

        public int RunProcessAsAdmin(string pathExecutable)
        {
            try
            {
                _log.WriteLog(null, "INFO", string.Concat(" Executing  file --> ", pathExecutable));
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", "/c " + pathExecutable);
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.ErrorDialog = true;
                startInfo.Verb = "runas";
                startInfo.CreateNoWindow = true;
                _log.WriteLog(null, "INFO", string.Concat(" Start process Altitud for: ", pathExecutable));
                Process process_Altitud = Process.Start(startInfo);

                //process.WaitForExit();
                while (!process_Altitud.HasExited)
                {
                    Thread.Sleep(100);
                }

                _log.WriteLog(null, "INFO", string.Concat(" End process Altitud with code: ", process_Altitud.ExitCode));

                return 1;
            }
            catch (Win32Exception ex)
            {
                _log.WriteLog(null, "ERROR", " In RunProcessAsAdmin: " + ex.Message);

                switch (ex.NativeErrorCode)
                {
                    case 1223:
                        return ex.NativeErrorCode;
                    default:
                        return -1;
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog(null, "ERROR", " In RunProcessAsAdmin: " + ex.Message);
                return -2;
            }
        }

    }
}
