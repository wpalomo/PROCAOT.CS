
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace TF
{
    public class Tf : _Tf
    {
        private OdbcConnection cnTF;
        private OdbcCommand cmTF;
        private OdbcCommandBuilder cbTF;

        //------------------------------------------------------------------
        // Métodos de usuario.
        //------------------------------------------------------------------

        // Leer DataRow.
        public Char OdbcSeekRow(string SeekFichero,
                                string SeekFields = "*",
                                string SeekWhere = "1=1",
                                string SeekOrder = "",
                                string SeekGroup = "")
        {
            Char retorno = 'S';

            _inicializar();

            try
            {
                // Montar la sentencia SQL.
                sqlsql = "Select " + (SeekFields=="" ? " * " : SeekFields);
                sqlsql += " From " + SeekFichero;
                sqlsql += (SeekWhere=="" ? "1=1" : " Where " + SeekWhere);
                sqlsql += (SeekOrder=="" ? "" : " Order By " + SeekOrder);
                sqlsql += (SeekGroup=="" ? "" : " Group By " + SeekGroup);

                // cmTF.Connection = cnTF;          // This way or CreateCommand().
                cmTF = cnTF.CreateCommand();        // This way or Set connection parameter.
                cmTF.CommandType = CommandType.Text;
                cmTF.CommandText = sqlsql;

                sqldatatable = new DataTable(SeekFichero);
                sqldataadapter = new OdbcDataAdapter(cmTF);
                sqldataset = new DataSet();

                sqlnoofrows = sqldataadapter.Fill(sqldataset, SeekFichero);
                sqldatatable = sqldataset.Tables[SeekFichero];
                sqltablereader = SqlDataSet.CreateDataReader();

                retorno = (SqlNoOfRows > 0) ? 'S' : 'N';
                if (retorno=='S')
                {
                    // DataRow a devolver.
                    sqldatarow = sqldatatable.NewRow();
                    sqldatarow = sqldataset.Tables[SeekFichero].Rows[0];
                }
            }

            catch (OdbcException ex)
            {
                sqlstatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        // Actualizar un DataRow.
        // Recibe un array con las columnas a actualizar con sus valores,
        // en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtIndex"></param>
        /// <param name="drIndex"></param>
        /// <param name="UpdColsValues"></param>
        /// <returns></returns>
        public Char OdbcUpdateCurrentRow(int dtIndex, int drIndex, params object[] UpdColsValues)
        {
            Char retorno = 'S';

            _inicializar();

            // Calcular la sentencia update a partir de la PK.
            cbTF = new OdbcCommandBuilder(sqldataadapter);
            sqldataadapter.UpdateCommand = cbTF.GetUpdateCommand();

            for (int i = 0; i < UpdColsValues.Length; i+=2)
            {
                sqldataset.Tables[dtIndex].Rows[drIndex][UpdColsValues[i].ToString()] = UpdColsValues[i + 1];
            }

            // sqldatatable.ImportRow(UpdRow);
            sqlnoofrows = sqldataadapter.Update(sqldatatable);

            return retorno;
        }

        // Actualizar un DataRow.
        // Recibe un array con las columnas a actualizar con sus valores,
        // en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN.

 
        /// <summary>
        /// Texto
        /// </summary>
        /// <param name="UpdColsValues">Parámetro</param>
        /// <returns>S/N</returns>
        public Char OdbcUpdateCurrentRow(params object[] UpdColsValues)
        {
            return OdbcUpdateCurrentRow(0, 0, UpdColsValues);
        }

        // Ejecución directa de una sentencia SQL.
        public Char OdbcDirectSQL(string strSQL)
        {
            Char retorno = 'S';

            try
            {
                cmTF.CommandType = CommandType.Text;
                cmTF.CommandText = strSQL;
                cmTF.Connection = cnTF;

                cmTF.ExecuteScalar();
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'N';
            }

            return retorno;
        }

        //------------------------------------------------------------------
        // Métodos públicos de conversión de tipos según tipo BBDD.
        //------------------------------------------------------------------

        // Obtener substring - Método.
        public string GetCvtSubStr(DBEnvironments dbEnvironment, string dbVersion, string dbColumna, int posicionInicial, int lenPosicionFinal)
        {
            string strSubstring = "";

            switch (dbEnvironment)
            {
                case DBEnvironments.SQL:
                    strSubstring = "SUBSTRING(" + dbColumna + ", " + posicionInicial.ToString("####") + ", " + lenPosicionFinal.ToString("####") + ")";
                    break;

                case DBEnvironments.DB2:
                    strSubstring = "SUBSTR(" + dbColumna + ", " + posicionInicial.ToString("####") + ", " + lenPosicionFinal.ToString("####") + ")";
                    break;

                case DBEnvironments.ORACLE:
                    strSubstring = "SUBSTR(" + dbColumna + ", " + posicionInicial.ToString("####") + ", " + lenPosicionFinal.ToString("####") + ")";
                    break;

                case DBEnvironments.MSACCESS:
                    strSubstring = "MID(" + dbColumna + ", " + posicionInicial.ToString("####") + ", " + lenPosicionFinal.ToString("####") + ")";
                    break;

                case DBEnvironments.VFP:
                    strSubstring = "SUBSTR(" + dbColumna + ", " + posicionInicial.ToString("####") + ", " + lenPosicionFinal.ToString("####") + ")";
                    break;

                default:
                    strSubstring = "SUBSTRING(" + dbColumna + ", " + posicionInicial.ToString("####") + ", " + lenPosicionFinal.ToString("####") + ")";
                    break;
            }

            return strSubstring;
        }

        // Obtener la cadena de concatenación - Método.
        public string GetCvtCat(DBEnvironments dbEnvironment, string dbVersion)
        {
            string strConcat = "";

            switch (dbEnvironment)
            {
                case DBEnvironments.SQL:
                    strConcat = "+";
                    break;

                case DBEnvironments.DB2:
                    strConcat = "A";
                    break;

                case DBEnvironments.ORACLE:
                    strConcat = "||";
                    break;

                case DBEnvironments.MSACCESS:
                    strConcat = "+";
                    break;

                case DBEnvironments.VFP:
                    strConcat = "+";
                    break;

                default:
                    strConcat = "+";
                    break;
            }

            return strConcat;
        }

        // Obtener la cadena Like - Método.
        public string GetCvtLike(DBEnvironments dbEnvironment, string dbVersion)
        {
            string strLike = "";

            switch (dbEnvironment)
            {
                case DBEnvironments.SQL:
                    strLike = "%";
                    break;

                case DBEnvironments.DB2:
                    strLike = "%";
                    break;

                case DBEnvironments.ORACLE:
                    strLike = "%";
                    break;

                case DBEnvironments.MSACCESS:
                    strLike = "*";
                    break;

                case DBEnvironments.VFP:
                    strLike = "%";
                    break;

                default:
                    strLike = "%";
                    break;
            }

            return strLike;
        }

        //------------------------------------------------------------------
        // Macros públicas de conversión de tipos según tipo BBDD.
        //------------------------------------------------------------------

        // Obtener substring - Macro.
        public string _GCSS(string dbColumna, int posInicial, int lenPosFinal)
        {
            return GetCvtSubStr(_DBEntorno, _DBVersion, dbColumna, posInicial, lenPosFinal);
        }

        // Obtener la cadena de concatenación - Macro.
        public string _GCC()
        {
            return GetCvtCat(_DBEntorno, _DBVersion);
        }

        // Obtener la cadena de concatenación - Macro.
        public string _GCC(params string [] colsToConcat)
        {
            string colsConcatenadas = colsToConcat.Length == 0 ? "" : colsToConcat[0];

            for(int i=1; i<colsToConcat.Length; i++)
                colsConcatenadas += GetCvtCat(_DBEntorno, _DBVersion) + colsToConcat;

            return colsConcatenadas;
        }

        // Obtener la cadena Like - Macro.
        public string _GCL()
        {
            return GetCvtLike(_DBEntorno, _DBVersion);
        }

        //------------------------------------------------------------------
        // Métodos públicos de la clase - General.
        //------------------------------------------------------------------

        // Inicializar la clase.
        public bool Inicializar(string DBCadenaDeConexion = null,
                                TF._Tf.DBEnvironments DBENtorno = TF._Tf.DBEnvironments.SQL,
                                string DBVersion = "0.0",
                                TF._Tf.DBProviders DBProvider = TF._Tf.DBProviders.ODBC)
        {
            bool retorno = true;

            try
            {
                if(cnTF.State!=ConnectionState.Open)
                    if(DBCadenaDeConexion!=null)
                    {
                        cnTF.ConnectionString = DBCadenaDeConexion;
                        cnTF.Open();
                    }

                _DBEntorno = DBENtorno;
                _DBVersion = DBVersion;
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = false;
            }

            return retorno;
        }

        // Cerrar la clase.
        public bool Finalizar()
        {
            return true;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        public Tf()
        {
            cnTF = new OdbcConnection();

            UsrErrorC = 0;
            UsrError = "";
            UsrErrorE = "";
        }

        public Tf(OdbcConnection cmConn = null) : base()
        {
            try
            {
                if (cmConn != null) cnTF = cmConn;
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }
        }

        public Tf(string strCadenaDeConexion) : this()
        {
            try
            {
                cnTF.ConnectionString = strCadenaDeConexion;
                cnTF.Open();
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }

            finally
            {
            }
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        //------------------------------------------------------------------
        // Destructor de la clase.
        //------------------------------------------------------------------

        ~Tf()
        {
            try
            {
                if (cnTF.State == ConnectionState.Open) cnTF.Close();
                cmTF = null;
            }

            catch (Exception ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        public override string ToString()
        {
            string retorno = null;

            try
            {
                if (cnTF.State == ConnectionState.Open)
                    return cnTF.ConnectionString;
                else
                    return null;
            }
            catch (Exception ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }

            return retorno;
            // return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return ((TF.Tf)obj).cnTF.ConnectionString == this.cnTF.ConnectionString;
            // return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            // If we override Equals, we've to do the same with GetHashCode.
            return base.GetHashCode();
        }
    }
}
