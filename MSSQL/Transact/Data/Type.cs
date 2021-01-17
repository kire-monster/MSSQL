namespace MSSQL.Transact.Data
{
    /// <summary>
    /// Tipos que podra obtener en Las consultas 
    /// </summary>
    public enum Type
    {
        DataSet   = 0b0000_0000,
        DataTable = 0b0000_0001,
        Both      = 0b0000_0010,
    }
}
