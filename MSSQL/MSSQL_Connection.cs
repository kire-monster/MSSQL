/*!
 * MSSQL - C sharp
 * https://github.com/kire-monster/MSSQL/
 * 
 * Released under the GNU General Public License v3.0
 * @author: Erik Carrillo
 * @email: hell-dxd@hotmail.com, devs@kllmp.org
 * @version: 2.0.0
 * 
 * @Date: 2020-09-09
 * 
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace MSSQL
{
    namespace Module
    {
        public class MSSQL_Connection
        {
            public SqlDataReader Record { get => _Record; }
            public bool StatusError { get => _StatusError; }
            public string MessageError { get => _MessageError; }
            public string ConnectionString { set => _ConnectionString = value; }


            private SqlConnection Con = null;
            private bool _StatusError = false;
            private SqlDataReader _Record = null;
            private string _MessageError = String.Empty;
            private string _ConnectionString = String.Empty;
            


            public void Open()
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

            public void Close()
            {
                try
                {
                    Con.Close();
                    if (Record != null) 
                        Record.Close();
                    _StatusError = false;
                }
                catch (Exception err)
                {
                    _StatusError = true;
                    _MessageError = err.Message;
                }
            }

            public void Exec(string query)
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

            public void Exec(string storedProcedure, List<SqlParameter> parametros)
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

            public bool Fetch()
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
}
