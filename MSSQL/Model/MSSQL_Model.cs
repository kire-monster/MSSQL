/*!
 * MSSQL.Model - C sharp
 * https://github.com/kire-monster/MSSQL/
 * 
 * Released under the GNU General Public License v3.0
 * @author: Erik Carrillo
 * @email: hell-dxd@hotmail.com, devs@kllmp.org
 * @version: 2.0.1
 * 
 * @Date: 2020-09-09
 * 
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MSSQL.Model
{
    public class MSSQL_Model
    {
        public SqlDataReader MSSQL_Record { get => _Record; }
        public bool MSSSQL_StatusError { get => _StatusError; }
        public string MSSQL_MessageError { get => _MessageError; }
        public string MSSQL_ConnectionString { set => _ConnectionString = value; }
            

        private SqlConnection Con = null;
        private bool _StatusError = false;
        private SqlDataReader _Record = null;
        private string _MessageError = String.Empty;
        private string _ConnectionString = String.Empty;


        public void MSSQL_Open()
        {
            try
            {
                Con = new SqlConnection(_ConnectionString);
                _StatusError = false;
                Con.Open();
            }
            catch (Exception error)
            {
                _MessageError = error.Message;
                _StatusError = true;
            }
        }

        public void MSSQL_Close()
        {
            try
            {
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

        public void MSSQL_Exec(string query)
        {
            try
            {
                if (_StatusError || Con == null) { return; }
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.CommandType = CommandType.Text;
                _Record = cmd.ExecuteReader();
                _StatusError = false;
            }
            catch (Exception error)
            {
                _StatusError = true;
                _MessageError = error.Message;
            }
        }

        public void MSSQL_Exec(string storedProcedure, List<SqlParameter> parametros)
        {
            try
            {
                if (_StatusError || Con == null) { return; }

                SqlCommand cmd = new SqlCommand(storedProcedure, Con);
                cmd.CommandType = CommandType.StoredProcedure;

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
            catch (Exception error)
            {
                _StatusError = true;
                _MessageError = error.Message;
            }
        }

        public bool MSSQL_Fetch()
        {
            bool status = false;
            try
            {
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
    }
}

