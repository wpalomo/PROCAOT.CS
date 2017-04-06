
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using TF;

namespace TF
{
    /// <summary>
    /// Propiedades y métodos de uso interno de la clase genérica para acceso a funciones de BBDD.
    /// </summary>

    public class _Tf
    {
        // Proveedores de BBDD.
        /// <summary> Proveedores de acceso a BBDD </summary>
        public enum DBProviders
        {
            /// <summary> ODBC </summary>
            ODBC = 'O',
            /// <summary> ADODB </summary>
            ADO = 'A',
            /// <summary> OLEDB </summary>
            OLEDB = 'L',
            /// <summary> SQL </summary>
            SQL = 'S'
        }

        // Entorno de BBDD.
        /// <summary> Entornos de trabajo de BBDD</summary>
        public enum DBEnvironments
        {
            /// <summary></summary>
            SQL = 'S',
            /// <summary></summary>
            DB2 = 'D',
            /// <summary></summary>
            ORACLE = 'O',
            /// <summary></summary>
            MSACCESS = 'M',
            /// <summary></summary>
            VFP = 'V',
            /// <summary></summary>
            NULL = 0
        }

        /// <summary> </summary>
        /// <returns></returns>
        public struct RowID
        {
            private string rowid;
            private long currentValue;
            private char fillchar;

            /// <summary> </summary>
            /// 
            /// <param name="capacity"> </param>
            /// <param name="fillChar"> </param>
            public RowID(int capacity = 10, char fillChar = '0')
            {
                rowid = fillChar.ToString().PadRight(capacity, fillChar);
                currentValue = 0;
                fillchar = fillChar;

                return;
            }

            /// <summary> </summary>
            /// <param name="rID"></param>
            public static implicit operator String(RowID rID)
            {
                return rID.rowid;
            }

            /// <summary> </summary>
            /// <param name="rID"></param>
            /// <returns></returns>
            public static RowID operator ++(RowID rID)
            {
                return rID + 1;
                //rID.currentValue++;
                //rID.rowid = System.Convert.ToString(rID.currentValue).PadLeft(rID.rowid.Length, rID.fillchar);

                //return rID;
            }

            /// <summary> </summary>
            /// <param name="rID"></param>
            /// <param name="incremento"></param>
            /// <returns></returns>
            public static RowID operator+(RowID rID, int incremento)
            {
                rID.currentValue += incremento;
                rID.rowid = System.Convert.ToString(rID.currentValue).PadLeft(rID.rowid.Length, rID.fillchar);
                return rID;
            }
        }

        //------------------------------------------------------------------
        // Propiedades públicas de control de entorno.
        //------------------------------------------------------------------

        private DBEnvironments _dbentorno = DBEnvironments.SQL;
        /// <summary> Entorno activo de BBDD</summary>
        public DBEnvironments _DBEntorno
        {
            get { return _dbentorno; }
            set { _dbentorno = value; }
        }

        private string _dbversion = "1.0";
        /// <summary> Versión activa de la BBDD</summary>
        public string _DBVersion
        {
            get { return _dbversion; }
            set { _dbversion = value; }
        }

        private DBProviders _dbprovider = DBProviders.ODBC;
        /// <summary> Proveedor activo de BBDD</summary>
        public DBProviders _DBProvider
        {
            get { return _dbprovider; }
            set { _dbprovider = value; }
        }

        // Cadena de concatenación.
        private string _dbconcat;
        /// <summary></summary>
        public string _DBConcat
        {
            get { return _dbconcat; }
            set { _dbconcat = value; }
        }

        // Cadena Like.
        private string _dblike;
        /// <summary></summary>
        public string _DBLike
        {
            get { return _dblike; }
            set { _dblike = value; }
        }

        //------------------------------------------------------------------
        // Propiedades públicas de control de SQL.
        //------------------------------------------------------------------

        // Código de error SQL.
        /// <summary></summary>
        public int SqlStatus
        { get; set; }

        // Sentencia SQL.
        /// <summary></summary>
        public String SqlSQL
        { get; set; }

        // Nº de filas afectadas.
        /// <summary></summary>
        public int SqlNoOfRows
        { get; set; }

        // Código de error de programa.
        /// <summary></summary>
        public int UsrErrorC
        { get; set; }

        // Mensajes de error.
        /// <summary></summary>
        public String UsrError
        { get; set; }

        // Mensaje de error extendido.
        /// <summary></summary>
        public String UsrErrorE
        { get; set; }

        //------------------------------------------------------------------
        // Propiedades públicas de la clase.
        //------------------------------------------------------------------

        internal Object sqlconnection;
        /// <summary> Connection de retorno </summary>
        public Object SqlConnection
        {
            get { return sqlconnection; }
            private set { sqlconnection = value; }
        }

        internal Object sqldataadapter;
        /// <summary> DataAdapter de retorno </summary>
        public Object SqlDataAdapter
        {
            get { return sqldataadapter; }
            private set { sqldataadapter = value; }
        }

        internal DataSet sqldataset;
        /// <summary> DataSet de retorno</summary>
        public DataSet SqlDataSet
        {
            get { return sqldataset; }
            private set { sqldataset = value; }
        }

        internal DataTableReader sqltablereader;
        /// <summary> DataTableReader de retorno</summary>
        public DataTableReader SqlTableReader
        {
            get { return sqltablereader; }
            private set { sqltablereader = value; }
        }

        internal object sqldatareader;
        /// <summary> DataReader de retorno</summary>
        public Object SqlDataReader
        {
            get { return sqldatareader; }
            private set { sqldatareader = value; }
        }

        internal DataTable sqldatatable;
        /// <summary> DataTable de retorno</summary>
        public DataTable SqlDataTable
        {
            get { return sqldatatable; }
            private set { sqldatatable = value; }
        }

        internal DataRow sqldatarow;
        /// <summary> DataRow de retorno </summary>
        public DataRow SqlDataRow
        {
            get { return sqldatarow; }
            set { sqldatarow = value; }
        }

        internal object sqlscalar;
        /// <summary> Objeto devuelto por ExecuteScalar </summary>
        public Object SqlScalar
        {
            get { return sqlscalar;  }
            private set { sqlscalar = value;  }
        }

        internal object sqlcommand;
        /// <summary> Objeto Comando </summary>
        public Object SqlCommand
        {
            get { return sqlcommand; }
            private set { sqlcommand = value; }
        }

        /// <summary> Inicializar las propiedades de uso interno de la clase </summary>
        internal void _inicializar()
        {
            SqlNoOfRows = -1;
            SqlSQL = "";
            SqlStatus = 0;
            UsrError = "";
            UsrErrorC = 0;
            UsrErrorE = "";

            return;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        /// <summary> Constructor por defecto de la clase. </summary>
        public _Tf()
        {
        }

        //------------------------------------------------------------------
        // Destructor de la clase.
        //------------------------------------------------------------------

        //~_Tf()
        //{
        //}
    }
}
