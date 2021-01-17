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
    /// Clase Para manejar los Registros 
    /// </summary>
    public class MSSqlRegisters
    {
        private SqlConnection Con = null;
        private SqlDataReader _Record = null;

        public SqlDataReader Record { get => _Record; }
        public bool StatusError { get => _StatusError; }
        public string MessageError { get => _MessageError; }


        private bool _StatusError = false;
        private string _MessageError = String.Empty;
        private int _CommandTimeout = 30;


        /// <summary>
        /// Constructor - Overload 1
        /// </summary>
        /// <param name="connection" type="SqlConnection">Conexion de SqlClient</param>
        /// <param name="commandTimeout" type="int">Parametro de tiempo de ejecucion de la consulta</param>
        /// <param name="query" type="string">Consulta que se ejecutara en la base de datos</param>
        public MSSqlRegisters(SqlConnection connection, int commandTimeout, string query)
        {
            _CommandTimeout = commandTimeout;
            Open(connection);
            Exec(query);
        }


        /// <summary>
        /// Constructor - Overload 1
        /// </summary>
        /// <param name="connection" type="SqlConnection">Conexion de SqlClient</param>
        /// <param name="commandTimeout" type="int">Parametro de tiempo de ejecucion de la consulta</param>
        /// <param name="query" type="string">Consulta que se ejecutara en la base de datos</param>
        /// <param name="param" type="List<SqlParameter>">Parametros para el procedimiento almacenado tambien acepta NULL</param>
        public MSSqlRegisters(SqlConnection connection, int commandTimeout, string query, List<SqlParameter> param)
        {
            _CommandTimeout = commandTimeout;
            Open(connection);
            Exec(query, param);
        }


        /// <summary>
        /// Description: Cierra la conexion
        /// </summary>
        public void Close()
        {
            try
            {
                if(Con != null)
                    Con.Close();
                if (_Record != null)
                    _Record.Close();
                _StatusError = false;
            }
            catch (Exception err)
            {
                _StatusError = true;
                _MessageError = err.Message;
            }
        }


        /// <summary>
        /// Description: Abre la Conexion
        /// </summary>
        /// <param name="connection" type="SqlConnection">Objeto de Conexion</param>
        private void Open(SqlConnection connection)
        {
            try
            {
                if(connection!=null)
                {
                    Con = connection;
                    Con.Open();
                    _StatusError = false;
                }
            }
            catch(SqlException err)
            {
                _MessageError = err.Message;
                _StatusError = true;
            }
            catch(Exception err)
            {
                _MessageError = err.Message;
                _StatusError = true;
            }
        }


        /// <summary>
        /// Description: Verifica si hay registros del Query ejecutado o Procedimiento Almacenado
        /// </summary>
        /// <returns>bool</returns>
        public bool Fetch()
        {
            bool status = false;
            try
            {
                if(Con == null || _Record == null)
                {
                    _MessageError = "The Connection is Null";
                    return false;
                }

                if (!_StatusError && _Record.HasRows)
                {
                    if (_Record.Read())
                    {
                        status = true;
                    }
                }
            }
            catch (Exception err)
            {
                _StatusError = true;
                _MessageError = err.Message;
            }

            return status;
        }


        /// <summary>
        /// Description: Ejecuta la consulta en el servidor
        /// </summary>
        /// <param name="query"></param>
        private void Exec(string query)
        {
            try
            {
                if (_StatusError || Con == null) { return; }
                SqlCommand cmd = new SqlCommand(query, Con)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = _CommandTimeout
                };
                _Record = cmd.ExecuteReader();
                _StatusError = false;
            }
            catch (SqlException err)
            {
                _MessageError = err.Message;
                _StatusError = true;
            }
            catch (Exception err)
            {
                _MessageError = err.Message;
                _StatusError = true;
            }
        }


        /// <summary>
        /// Description: Ejecuta el procedimiento almacenado en el servidor
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="parametros"></param>
        private void Exec(string storedProcedure, List<SqlParameter> parametros)
        {
            try
            {
                if (_StatusError || Con == null) { return; }

                SqlCommand cmd = new SqlCommand(storedProcedure, Con)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = _CommandTimeout
                };
                    

                if (parametros != null)
                {
                    foreach (var parametreo in parametros)
                    {
                        cmd.Parameters.Add(parametreo);
                    }
                }

                _Record = cmd.ExecuteReader();
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
        }

    }
}
