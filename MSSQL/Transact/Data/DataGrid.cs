using System.Data;

namespace MSSQL.Transact.Data
{
    /// <summary>
    /// Clase Que contendra las grillas de datos
    /// </summary>
    public class DataGrid
    {
        public DataTable DataTable { get; set; }
        public DataSet DataSet { set; get; }

        public DataGrid()
        {
            DataTable = new DataTable();
            DataSet = new DataSet();
        }
    }
}
