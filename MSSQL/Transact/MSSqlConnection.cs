/*!
 * MSSQL.Transact - C sharp
 * https://github.com/kire-monster/MSSQL/
 * 
 * Released under the GNU General Public License v3.0
 * @author: Erik Carrillo
 * @email: hell-dxd@hotmail.com, devs@kllmp.org
 * @version: 1.0.0
 * 
 * @Date: 2021-01-16
 * 
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MSSQL.Transact
{
    /// <summary>
    /// Clase que gestiona la conexion a base de datos
    /// </summary>
    public class MSSqlConnection
    {
        public bool StatusError { get => _StatusError; }
        public string MessageError { get => _MessageError; }
        public string ConnectionString { set => _ConnectionString = value; }
        public int SetCommandTimeout { set => _CommandTimeout = value; }


        private bool _StatusError = false;
        private string _MessageError = String.Empty;
        private string _ConnectionString = String.Empty;
        private int _CommandTimeout = 30;


        /// <summary>
        /// Ejecuta la Consulta obteniendo los registros de uno a uno.
        /// </summary>
        /// <param name="query" type="string">Consulta que sera ejecutada en el servidor</param>
        /// <returns type="class">MSSqlRegisters</returns>
        public MSSqlRegisters Exec(string query)
        {
            try
            {
                return new MSSqlRegisters(new SqlConnection(_ConnectionString), _CommandTimeout, query);
            }
            catch (SqlException err)
            {
                _MessageError = err.Message;
                _StatusError = true;
            }
            catch (Exception error)
            {
                _StatusError = true;
                _MessageError = error.Message;
            }
            return new MSSqlRegisters(null, _CommandTimeout, query);
        }


        /// <summary>
        /// Ejecuta el Procedimiento almancenado obteniendo los registros de uno a uno.
        /// </summary>
        /// <param name="storedProcedure" type="string"></param>
        /// <param name="parameters" type="List<SqlParameter>"></param>
        /// <returns type="class">MSSqlRegisters</returns>
        public MSSqlRegisters Exec(string storedProcedure, List<SqlParameter> parameters)
        {
            try
            {
                return new MSSqlRegisters(new SqlConnection(_ConnectionString), _CommandTimeout, storedProcedure, parameters);
            }
            catch (SqlException err)
            {
                _MessageError = string.Format("State {0}, Line {1}, Procedure {2}. {3}", err.State, err.LineNumber, err.Procedure, err.Message);
                _StatusError = true;
            }
            catch (Exception error)
            {
                _StatusError = true;
                _MessageError = error.Message;
            }
            return new MSSqlRegisters(null, _CommandTimeout, storedProcedure, parameters);
        }

        /// <summary>
        /// Ejecuta una consulta en DB y devuelve una clase, esta Puede tener DataSet o DataTable o Ambos
        /// </summary>
        /// <param name="query" type="string">Consulta que se ejecutara en DB</param>
        /// <param name="type" type="Data.Type">Tipo de dato que retornara (DataSet o DataTable)</param>
        /// <returns>Data.DataGrid</returns>
        public Data.DataGrid Exec(string query, Data.Type type)
        {
            Data.DataGrid data = new Data.DataGrid();
            try
            {
                SqlConnection Con = new SqlConnection(_ConnectionString);
                Con.Open();

                SqlCommand cmd = new SqlCommand(query, Con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = _CommandTimeout
                };

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                switch (type)
                {
                    case Data.Type.DataSet:
                        da.Fill(data.DataSet);
                    break;
                    case Data.Type.DataTable:
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        data.DataTable = (ds.Tables.Count > 0) ? ds.Tables[0] : null;
                    break;
                    case Data.Type.Both:
                        da.Fill(data.DataSet);
                        data.DataTable = (data.DataSet.Tables.Count > 0) ? data.DataSet.Tables[0] : null;
                    break;
                }


                Con.Close();
                _StatusError = false;
            }
            catch (SqlException err)
            {
                _MessageError = err.Message;
                _StatusError = true;
            }
            catch (Exception error)
            {
                _StatusError = true;
                _MessageError = error.Message;
            }

            return data;
        }


        /// <summary>
        /// Ejecuta un Stored Procedures en DB y devuelve una clase, esta Puede tener DataSet o DataTable o Ambos
        /// </summary>
        /// <param name="storedProcedure" type="string">Stored Procedures que se ejecutara en DB</param>
        /// <param name="parameters" type="List<SqlParameter>">Parametros de Stored Procedured</param>
        /// <param name="type" type="Data.Type">Tipo de dato que retornara (DataSet o DataTable)</param>
        /// <returns>Data.DataGrid</returns>
        public Data.DataGrid Exec(string storedProcedure, List<SqlParameter> parameters, Data.Type type)
        {
            Data.DataGrid data = new Data.DataGrid();
            try
            {
                SqlConnection Con = new SqlConnection(_ConnectionString);
                Con.Open();

                SqlCommand cmd = new SqlCommand(storedProcedure, Con)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = _CommandTimeout
                };

                if (parameters != null)
                {
                    foreach (var parametreo in parameters)
                    {
                        cmd.Parameters.Add(parametreo);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                switch (type)
                {
                    case Data.Type.DataSet:
                        da.Fill(data.DataSet);
                    break;
                    case Data.Type.DataTable:
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        data.DataTable = (ds.Tables.Count > 0) ? ds.Tables[0] : null;
                    break;
                    case Data.Type.Both:
                        da.Fill(data.DataSet);
                        data.DataTable = (data.DataSet.Tables.Count > 0) ? data.DataSet.Tables[0] : null;
                    break;
                }

                Con.Close();
                _StatusError = false;
            }
            catch (SqlException err)
            {
                _MessageError = string.Format("State {0}, Line {1}, Procedure {2}. {3}", err.State, err.LineNumber, err.Procedure, err.Message);
                _StatusError = true;
            }
            catch (Exception error)
            {
                _StatusError = true;
                _MessageError = error.Message;
            }

            return data;
        }
    }
}