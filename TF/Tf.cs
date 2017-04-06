
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

namespace TF
{
    /// <summary>
    /// 
    /// </summary>
    public class TFConnection : _Tf
    {

    }

    /// <summary> Clase genérica para acceso a funciones de BBDD.</summary>
    /// 
    /// <remarks>
    /// Información devuelta, según la función:
    ///     this.SqlNoOfRows, nº de filas afectadas por la sentencia SQL.
    ///     this.SqlDataRow, objeto DataTable con el registro leído.
    ///     this.SqlDataTable, objeto DataTable obtenido.
    ///     this.SqlTableReader, objeto secuencial TableReader sobre la tabla activa.
    ///     this.SqlDataReader, objeto secuencial sobre el set de datos activos.
    ///     this.SqlDataSet, objeto DataSet con el set de datos obtenido.
    ///     this.SqlDataAdapter, objeto DataAdapter de trabajo.
    ///     this.SqlSql, sentencia SQL generada.
    ///
    /// Gestión de la operación realizada:
    ///     Resultado de la llamada:
    ///         - S: Operación OK.
    ///         - N: Operación no realizada.
    ///         - C: Error BBDD / Error de programa.
    ///
    ///     Mensajes devueltos:
    ///         - this.SqlStatus, 0: OK, -1: Error.
    ///         - this.UsrErrorC, código de error (0: OK).
    ///         - this.UsrError, texto del error.
    ///         - this.UsrErrorE, texto error extendido - pila de llamadas.
    /// </remarks>
    /// 
    public class Tf : _Tf
    {
        private OdbcConnection cnTF;
        private OdbcCommand cmTF;
        private OdbcCommandBuilder cbTF;

        //------------------------------------------------------------------
        // Métodos de usuario: Funciones de acceso a datos de la BBDD.
        //------------------------------------------------------------------

        /// <summary> Comvertir un DataSet en un fichero Xml. </summary>
        /// 
        /// <remarks> Default DataSet: SqlDataSet </remarks>
        /// 
        /// <param name="outputFile"> Fichero de salida. </param>
        /// <param name="ds"> DataSet a convertir en XML. </param>
        ///
        /// <returns> Resultado (S-OK/N-No OK/C-Error) </returns>
        /// 
        public Char DataSetToXml(string outputFile, DataSet ds = null)
        {
            Char retorno = 'S';

            try
            {
                (ds==null ? SqlDataSet: ds).WriteXml(outputFile, XmlWriteMode.IgnoreSchema);
            }

            catch (OdbcException ex)
            {
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary> Convierte un DataSet en una cadena XML. </summary>
        ///
        /// <param name="ds"> DataSet a convertir en XML. </param>
        /// <returns> Cadena XML. Null si error. </returns>
        /// 
        public string DataSetToXml(DataSet ds = null)
        {
            string retorno = null;

            try
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();
                (ds==null ? SqlDataSet : ds).WriteXml(sw, XmlWriteMode.IgnoreSchema);
                retorno = sw.ToString();

                // Alternativamente.
                //retorno = (ds == null ? SqlDataSet : ds).GetXml();
            }

            catch (OdbcException ex)
            {
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            catch (SqlException ex)
            {
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            return retorno;
        }

        /// <summary> Leer registros de un fichero de la BBDD. </summary>
        /// 
        /// <remarks> Si resultado multi-row, devuelve el primer registro. </remarks>
        /// 
        /// <param name="SeekFichero"> Nombre del fichero </param>
        /// <param name="SeekFields"> Columnas a devolver. Def: Todas </param>
        /// <param name="SeekWhere"> Claúsula de selección de registros. Def: Todos </param>
        /// <param name="SeekOrder"> Orden de las columnas. Def: Sin ordenar </param>
        /// <param name="SeekGroup"> Agrupación de registros. Def: Sin agrupación </param>
        /// <param name="SeekJoin"> Cláusula JOIN </param>
        /// 
        /// <returns>
        /// Resultado (S-Hay filas/N-No hay filas/C-Error)
        /// this.SqlNoOfRows, nº de filas devueltas.
        /// </returns>
        /// 
        /// <see cref="OdbcSeekRow(string, string, string, string, string, string)" />
        /// <seealso cref="SqlSeekRow(string, string, string, string, string, string)"/>
        ///
        public Char SeekRow(string SeekFichero,
                            string SeekFields = "*",
                            string SeekWhere = "1=1",
                            string SeekOrder = "",
                            string SeekGroup = "",
                            string SeekJoin = "")
        {
            Char retorno = 'S';
            
            try
            {
                if (_DBProvider == DBProviders.SQL)
                    retorno = SqlSeekRow(SeekFichero,
                                         SeekFields,
                                         SeekWhere,
                                         SeekOrder,
                                         SeekGroup,
                                         SeekJoin);
                else
                    retorno = OdbcSeekRow(SeekFichero,
                                         SeekFields,
                                         SeekWhere,
                                         SeekOrder,
                                         SeekGroup,
                                         SeekJoin);
            }

            catch (Exception ex)
            {
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary> Leer registros de un fichero de la BBDD. Utiliza ODBC. </summary>
        /// 
        /// <remarks> Si resultado multi-row, devuelve el primer registro. </remarks>
        /// 
        /// <param name="SeekFichero"> Nombre del fichero </param>
        /// <param name="SeekFields"> Columnas a devolver. Def: Todas </param>
        /// <param name="SeekWhere"> Claúsula de selección de registros. Def: Todos </param>
        /// <param name="SeekOrder"> Orden de las columnas. Def: Sin ordenar </param>
        /// <param name="SeekGroup"> Agrupación de registros. Def: Sin agrupación </param>
        /// <param name="SeekJoin"> Cláusula JOIN </param>
        /// 
        /// <returns>
        /// Resultado (S-Hay filas/N-No hay filas/C-Error)
        /// this.SqlNoOfRows, nº de filas devueltas.
        ///</returns>
        ///
        private Char OdbcSeekRow(string SeekFichero,
                                 string SeekFields = "*",
                                 string SeekWhere = "1=1",
                                 string SeekOrder = "",
                                 string SeekGroup = "",
                                 string SeekJoin = "")
        {
            Char retorno = 'S';

            _inicializar();

            try
            {
                // Montar la sentencia SQL.
                SqlSQL = "Select " + (SeekFields == "" ? " * " : SeekFields);
                SqlSQL += " From " + SeekFichero;
                SqlSQL += (SeekJoin == "" ? "" : " On " + SeekJoin);
                SqlSQL += " Where " + (SeekWhere == "" ? "1=1" : SeekWhere);
                SqlSQL += (SeekOrder == "" ? "" : " Order By " + SeekOrder);
                SqlSQL += (SeekGroup == "" ? "" : " Group By " + SeekGroup);

                sqlcommand = new OdbcCommand(SqlSQL, (OdbcConnection)sqlconnection);

                sqldatatable = new DataTable(SeekFichero);
                sqldataset = new DataSet();
                sqldataset.Clear();
                sqldataadapter = new OdbcDataAdapter((OdbcCommand)sqlcommand);

                SqlNoOfRows = ((OdbcDataAdapter)(sqldataadapter)).Fill(sqldataset, SeekFichero);
                sqldatatable = sqldataset.Tables[SeekFichero];
                sqltablereader = sqldatatable.CreateDataReader();

                retorno = (SqlNoOfRows != 0) ? 'S' : 'N';
                if (retorno == 'S')
                {
                    // DataRow a devolver.
                    sqldatarow = sqldatatable.NewRow();
                    sqldatarow = sqldataset.Tables[SeekFichero].Rows[0];
                }
            }

            catch (OdbcException ex)
            {
                // Error BBDD.
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            catch (Exception ex)
            {
                // Error de programa.
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary> Leer registros de un fichero de la BBDD. Utiliza SQL. </summary>
        /// 
        /// <remarks> Si resultado multi-row, devuelve el primer registro. </remarks>
        /// 
        /// <param name="SeekFichero">Nombre del fichero </param>
        /// <param name="SeekFields">Columnas a devolver. Def: Todas </param>
        /// <param name="SeekWhere">Claúsula de selección de registros. Def: Todos </param>
        /// <param name="SeekOrder">Orden de las columnas. Def: Sin ordenar </param>
        /// <param name="SeekGroup">Agrupación de registros. Def: Sin agrupación </param>
        /// <param name="SeekJoin">Cláusula JOIN </param>
        /// 
        /// <returns>
        /// Resultado (S-Hay filas/N-No hay filas/C-Error)
        /// this.SqlNoOfRows, nº de filas devueltas.
        ///</returns>
        ///
        private Char SqlSeekRow(string SeekFichero,
                                string SeekFields = "*",
                                string SeekWhere = "1=1",
                                string SeekOrder = "",
                                string SeekGroup = "",
                                string SeekJoin = "")
        {
            Char retorno = 'S';

            _inicializar();

            try
            {
                // Montar la sentencia SQL.
                SqlSQL = "Select " + (SeekFields == "" ? " * " : SeekFields);
                SqlSQL += " From " + SeekFichero;
                SqlSQL += (SeekJoin == "" ? "" : " On " + SeekJoin);
                SqlSQL += " Where " + (SeekWhere == "" ? "1=1" : SeekWhere);
                SqlSQL += (SeekOrder == "" ? "" : " Order By " + SeekOrder);
                SqlSQL += (SeekGroup == "" ? "" : " Group By " + SeekGroup);

                sqlcommand = new SqlCommand(SqlSQL, (SqlConnection)sqlconnection);

                sqldatatable = new DataTable(SeekFichero);
                sqldataset = new DataSet();
                sqldataset.Clear();
                sqldataadapter = new SqlDataAdapter((SqlCommand)sqlcommand);
                SqlNoOfRows = ((SqlDataAdapter)(sqldataadapter)).Fill(sqldataset, SeekFichero);
                sqldatatable = sqldataset.Tables[SeekFichero];
                sqltablereader = sqldatatable.CreateDataReader();

                retorno = (SqlNoOfRows != 0) ? 'S' : 'N';
                if (retorno == 'S')
                {
                    // DataRow a devolver.
                    sqldatarow = sqldatatable.NewRow();
                    sqldatarow = sqldataset.Tables[SeekFichero].Rows[0];
                }
            }

            catch (SqlException ex)
            {
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            catch (Exception ex)
            {
                SqlStatus = -1;
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary>
        /// Ejecución directa de una sentencia SQL que no devuelve datos (NonQuery).
        /// </summary>
        /// 
        /// <param name="strSQL">Sentencia SQL a ejecutar. </param>
        /// 
        /// <remarks>
        /// Ejecuta sentencias SQL que no devuelven datos - Select / Insert / Update / Delete.
        /// Si se ejecuta una sentencia Select, es solo a efectos de saber cuántas filas hay.
        /// </remarks>
        /// 
        /// <returns>
        /// - Nº de filas afectadas.
        /// - this.SqlNoOfRows, nº de filas afectadas.
        /// </returns>
        /// 
        public object ExecDirectSQL(string strSQL)
        {
            object retorno = null;

            try
            {
                cmTF.CommandType = CommandType.Text;
                cmTF.CommandText = strSQL;
                cmTF.Connection = (OdbcConnection) sqlconnection;

                retorno = ExecDirectSQL(cmTF);
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = -1;
            }

            return retorno;
        }

        /// <summary>
        /// Ejecución directa de una sentencia SQL que no devuelve datos (NonQuery).
        /// </summary>
        /// 
        /// <param name="cm"> Objeto OdbcCommand a ejecutar. </param>
        /// <param name="strSQL"> Sentencia SQL a ejecutar. </param>
        /// 
        /// <remarks>
        /// Ejecuta sentencias SQL que no devuelven datos - Select / Insert / Update / Delete.
        /// Si se ejecuta una sentencia Select, es solo a efectos de saber cuántas filas hay.
        /// </remarks>
        /// 
        /// <returns>
        /// - Nº de filas afectadas.
        /// - this.SqlNoOfRows, nº de filas afectadas.
        /// </returns>
        /// 
        public object ExecDirectSQL(OdbcCommand cm, String strSQL = null)
        {
            object retorno = null;

            try
            {
                SqlNoOfRows = cm.ExecuteNonQuery();

                retorno = SqlNoOfRows;
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = -1;
            }

            return retorno;
        }

        /// <summary>
        /// Ejecución directa de una sentencia SQL que no devuelve datos (NonQuery).
        /// </summary>
        /// 
        /// <param name="cm"> Objeto SqlCommand a ejecutar. </param>
        /// <param name="strSQL"> Sentencia SQL a ejecutar. </param>
        /// 
        /// <remarks>
        /// Ejecuta sentencias SQL que no devuelven datos - Select / Insert / Update / Delete.
        /// Si se ejecuta una sentencia Select, es solo a efectos de saber cuántas filas hay.
        /// </remarks>
        /// 
        /// <returns>
        /// - Nº de filas afectadas.
        /// - this.SqlNoOfRows, nº de filas afectadas.
        /// </returns>
        /// 
        public object ExecDirectSQL(SqlCommand cm, String strSQL = null)
        {
            object retorno = null;

            try
            {
                SqlNoOfRows = cm.ExecuteNonQuery();
                retorno = SqlNoOfRows;
            }

            catch (OdbcException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = -1;
            }

            return retorno;
        }

        /// <summary>
        /// Ejecución directa de una sentencia SQL que devuelve un único dato (Scalar).
        /// </summary>
        /// 
        /// <remarks>
        /// Ejecuta una sentencia SQL que devuelve una única columna de un único registro
        /// ó bien un único resultado (Count, Sum).
        /// </remarks>
        /// 
        /// <param name="strSQL"> Sentencia SQL a ejecutar. </param>
        /// 
        /// <returns>
        /// - Valor obtenido por la sentencia SQL.
        /// </returns>
        /// 
        public object ExecScalarSQL(string strSQL)
        {
            object retorno;

            try
            {

                if (_DBProvider == DBProviders.SQL)
                {
                    SqlCommand cm = new SqlCommand();

                    cm.CommandType = CommandType.Text;
                    cm.CommandText = strSQL;
                    cm.Connection = (SqlConnection)sqlconnection;

                    retorno = ExecScalarSQL(cm);
                }
                //if (_DBProvider == DBProviders.ODBC)
                else
                {
                    OdbcCommand cm = new OdbcCommand();

                    cm.CommandType = CommandType.Text;
                    cm.CommandText = strSQL;
                    cm.Connection = (OdbcConnection)sqlconnection;

                    retorno = ExecScalarSQL(cm);
                }
            }

            catch (SqlException ex)
            {
                // Error de BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            catch (OdbcException ex)
            {
                // Error de BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            catch (Exception ex)
            {
                // Error de programa.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            return retorno;
        }

        /// <summary>
        /// Ejecución directa de una sentencia SQL que devuelve un único dato (Scalar).
        /// </summary>
        /// 
        /// <remarks>
        /// Ejecuta una sentencia SQL que devuelve una única columna de un único registro
        /// ó bien un único resultado (Count, Sum).
        /// </remarks>
        /// 
        /// <param name="cm">  Objeto OdbcCommand a ejecutar. </param>
        /// <param name="strSQL"> Sentencia SQL a ejecutar. </param>
        /// 
        /// <returns>
        /// - Valor obtenido por la sentencia SQL.
        /// </returns>
        /// 
        public object ExecScalarSQL(OdbcCommand cm, String strSQL = null)
        {
            object retorno = null;

            try
            {
                sqlscalar = cm.ExecuteScalar();
                retorno = sqlscalar;
            }

            catch (OdbcException ex)
            {
                // Error de BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            catch (Exception ex)
            {
                // Error de programa.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            return retorno;
        }

        /// <summary>
        /// Ejecución directa de una sentencia SQL que devuelve un único dato (Scalar).
        /// </summary>
        /// 
        /// <remarks>
        /// Ejecuta una sentencia SQL que devuelve una única columna de un único registro
        /// ó bien un único resultado (Count, Sum).
        /// </remarks>
        /// 
        /// <param name="cm">  Objeto SqlCommand a ejecutar. </param>
        /// <param name="strSQL"> Sentencia SQL a ejecutar. </param>
        /// 
        /// <returns>
        /// - Valor obtenido por la sentencia SQL.
        /// </returns>
        /// 
        public object ExecScalarSQL(SqlCommand cm, String strSQL = null)
        {
            object retorno = null;

            try
            {
                sqlscalar = cm.ExecuteScalar();
                retorno = sqlscalar;
            }

            catch (OdbcException ex)
            {
                // Error de BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            catch (Exception ex)
            {
                // Error de programa.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = null;
            }

            return retorno;
        }

        //-----------------------------------------------------------------------
        // Métodos de usuario: Funciones de actualización de datos de la BBDD.
        //-----------------------------------------------------------------------

        /// <summary>
        /// Actualizar los cambios del DataRow activo, en el DataAdpater activo, obtenido previamente con SeekOdbcRow.
        /// Actualiza a partir de la PK de la tabla a actualizar.
        /// </summary>
        /// 
        /// <param name="da"> OdbcDataAdapater para actualizar. </param>
        /// <param name="ds"> DataSet a actualizar. </param>
        /// <param name="dtIndex"> Índice de la tabla en el DataRow. </param>
        /// <param name="drIndex"> Fila de la tabla. </param>
        /// <param name="UpdColsValues"> Columnas a actualizar, en formato Col1, value1, Col2, Value2, ..., ColN, ValueN </param>
        /// 
        ///<remarks>
        /// Recibe un array con las columnas a actualizar con sus valores,
        /// en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN.
        /// Las tablas a actualizar DEBEN tener PK.
        /// </remarks>
        /// 
        /// <seealso cref="OdbcSeekRow(string, string, string, string, string, string)"/>
        /// 
        /// <returns>Resultado (S/N/C)</returns>
        /// 
        public Char UpdateCurrentRow(OdbcDataAdapter da, DataSet ds, int dtIndex, int drIndex, params object[] UpdColsValues)
        {
            Char retorno = 'S';
            string dt = ds.Tables[dtIndex].TableName;

            try
            {
                _inicializar();

                // Calcular la sentencia update a partir de la PK.
                cbTF = new OdbcCommandBuilder(da);
                da.UpdateCommand = cbTF.GetUpdateCommand();
                da.InsertCommand = cbTF.GetInsertCommand();
                da.DeleteCommand = cbTF.GetDeleteCommand();

                for (int i = 0; i < UpdColsValues.Length; i += 2)
                {
                    ds.Tables[dtIndex].Rows[drIndex][UpdColsValues[i].ToString()] = UpdColsValues[i + 1];
                }

                SqlNoOfRows = da.Update(ds, dt);
            }

            catch(OdbcException ex)
            {
                // Error de BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary>
        /// Actualizar los cambios del DataRow activo, en el DataAdpater activo, obtenido previamente con SeekOdbcRow.
        /// Actualiza a partir de la PK de la tabla a actualizar.
        /// </summary>
        /// 
        /// <param name="da"> SqlDataAdapater para actualizar. </param>
        /// <param name="ds"> DataSet a actualizar </param>
        /// <param name="dtIndex"> Índice de la tabla en el DataRow. </param>
        /// <param name="drIndex"> Fila de la tabla. </param>
        /// <param name="UpdColsValues"> Columnas a actualizar, en formato Col1, value1, Col2, Value2, ..., ColN, ValueN </param>
        /// 
        ///<remarks>
        /// Recibe un array con las columnas a actualizar con sus valores,
        /// en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN.
        /// Las tablas a actualizar DEBEN tener PK.
        /// </remarks>
        /// 
        /// <seealso cref="OdbcSeekRow(string, string, string, string, string, string)"/>
        /// 
        /// <returns>Resultado (S/N/C)</returns>
        /// 
        public Char UpdateCurrentRow(SqlDataAdapter da, DataSet ds, int dtIndex, int drIndex, params object[] UpdColsValues)
        {
            Char retorno = 'S';
            string dt = ds.Tables[dtIndex].TableName;

            _inicializar();

            // Calcular la sentencia update a partir de la PK.
            SqlCommandBuilder cbTF = new SqlCommandBuilder(da);
            da.UpdateCommand = cbTF.GetUpdateCommand();
            da.InsertCommand = cbTF.GetInsertCommand();
            da.DeleteCommand = cbTF.GetDeleteCommand();

            for (int i = 0; i < UpdColsValues.Length; i += 2)
            {
                ds.Tables[dtIndex].Rows[drIndex][UpdColsValues[i].ToString()] = UpdColsValues[i + 1];
            }

            SqlNoOfRows = da.Update(ds, dt);

            return retorno;
        }

        /// <summary>
        /// Actualizar un DataRow.
        /// </summary>
        /// 
        /// <param name="da"> OdbcDataAdapater para actualizar. </param>
        /// <param name="ds"> DataSet a actualizar </param>
        /// <param name="UpdColsValues"> Columnas a actualizar, en formato Col1, value1, Col2, Value2, ..., ColN, ValueN </param>
        /// 
        /// <remarks>
        /// Recibe un array con las columnas a actualizar con sus valores
        /// en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN.
        /// </remarks>
        /// 
        /// <returns>Resultado (S/N/C)</returns>
        /// 
        public Char UpdateCurrentRow(OdbcDataAdapter da, DataSet ds, params object[] UpdColsValues)
        {
            return UpdateCurrentRow(da, ds, 0, 0, UpdColsValues);
        }

        /// <summary>
        /// Actualizar un DataRow.
        /// </summary>
        /// 
        /// <param name="da"> SqlDataAdapater para actualizar. </param>
        /// <param name="ds"> DataSet a actualizar. </param>
        /// <param name="UpdColsValues"> Columnas a actualizar, en formato Col1, value1, Col2, Value2, ..., ColN, ValueN </param>
        /// 
        /// <remarks>
        /// Recibe un array con las columnas a actualizar con sus valores
        /// en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN.
        /// </remarks>
        /// 
        /// <returns> Resultado (S/N/C)</returns>
        /// 
        public Char UpdateCurrentRow(SqlDataAdapter da, DataSet ds, params object[] UpdColsValues)
        {
            return UpdateCurrentRow(da, ds, 0, 0, UpdColsValues);
        }

        /// <summary>
        /// Actualizar un DataRow.
        /// </summary>
        /// 
        /// <param name="UpdColsValues"> Columnas a actualizar, en formato Col1, value1, Col2, Value2, ..., ColN, ValueN </param>
        /// 
        /// <remarks> Recibe un array con las columnas a actualizar con sus valores, </remarks>
        /// <remarks> en formato Columna1, Valor1, Columna2, Valor2, ..., columnaN, ValorN. </remarks>
        /// <returns>Resultado (S/N/C)</returns>
        /// 
        public Char UpdateCurrentRow(params object[] UpdColsValues)
        {
            if (this._DBProvider==DBProviders.SQL)
                return UpdateCurrentRow((SqlDataAdapter)SqlDataAdapter, sqldataset, 0, 0, UpdColsValues);
            else
                return UpdateCurrentRow((OdbcDataAdapter)SqlDataAdapter, sqldataset, 0, 0, UpdColsValues);
        }

        //------------------------------------------------------------------
        // Métodos públicos de conversión de tipos según tipo BBDD.
        //------------------------------------------------------------------

        /// <summary> Obtener el formato para procesar una columna NULL. </summary>
        /// 
        /// <param name="dbEnvironment"></param>
        /// <param name="dbVersion"></param>
        /// <param name="dbColumna"></param>
        /// <param name="sustValor"></param>
        /// 
        /// <returns> Cadena con formato. </returns>
        /// 
        public string GetCvtNull(DBEnvironments dbEnvironment, string dbVersion, string dbColumna, object sustValor)
        {
            string strNullString = "";

            switch (dbEnvironment)
            {
                case DBEnvironments.SQL:
                    strNullString = "ISNULL(" + dbColumna + ", " + sustValor + ")";
                    break;

                case DBEnvironments.DB2:
                    strNullString = sustValor.ToString();
                    break;

                case DBEnvironments.ORACLE:
                    strNullString = "NVL(" + dbColumna + ", " + sustValor + ")";
                    break;

                case DBEnvironments.MSACCESS:
                    strNullString = "ISNULL(" + dbColumna + ", " + sustValor + ")";
                    break;

                case DBEnvironments.VFP:
                    strNullString = sustValor.ToString();
                    break;

                default:
                    strNullString = "IIF(ISNULL(" + dbColumna + "), " + sustValor + ")";
                    break;
            }

            return strNullString;
        }

        /// <summary> Obtener formato para leer una subcadena de una columna de un fichero - Método</summary>
        /// 
        /// <param name="dbEnvironment">Entorno activo de a BBDD</param>
        /// <param name="dbVersion">Versión de la BBDD</param>
        /// <param name="dbColumna">Nombre de la columna de la BBDD</param>
        /// <param name="posicionInicial">Posición inicial de la subcadena</param>
        /// <param name="lenPosicionFinal">Longitud / posición final de la subcadena</param>
        /// 
        /// <returns>Formato de acceso a la subcadena de la columna</returns>
        /// 
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

        /// <summary> Obtener formato para concatenar columnas de un fichero de la BBDD - Método</summary>
        /// 
        /// <param name="dbEnvironment">Entorno activo de a BBDD</param>
        /// <param name="dbVersion">Versión de la BBDD</param>
        /// 
        /// <returns>Formato para concatenar columnas</returns>
        /// 
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

        /// <summary> Obtener formato de la función LIKE en sentencias SQL - Método</summary>
        /// 
        /// <param name="dbEnvironment">Entorno activo de a BBDD</param>
        /// <param name="dbVersion">Versión de la BBDD</param>
        /// <returns>Formato para la función LIKE</returns>
        /// 
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

        /// <summary> Eliminar espacios al final de una columna de tipo string. </summary>
        /// 
        /// <param name="dbEnvironment"></param>
        /// <param name="dbVersion"></param>
        /// <param name="dbColumna"></param>
        /// 
        /// <returns> Columna sin espacios. </returns>
        /// 
        public string GetCvtRTrim(DBEnvironments dbEnvironment, string dbVersion, string dbColumna)
        {
            string strRTrimStrim = "";

            switch (dbEnvironment)
            {
                case DBEnvironments.SQL:
                    strRTrimStrim = "RTRIM(" + dbColumna + ")";
                    break;

                case DBEnvironments.DB2:
                    strRTrimStrim = "RTRIM(" + dbColumna + ")";
                    break;

                case DBEnvironments.ORACLE:
                    strRTrimStrim = "RTRIM(" + dbColumna + ")";
                    break;

                case DBEnvironments.MSACCESS:
                    strRTrimStrim = "RTRIM(" + dbColumna + ")";
                    break;

                case DBEnvironments.VFP:
                    strRTrimStrim = "RTRIM(" + dbColumna + ")";
                    break;

                default:
                    strRTrimStrim = "RTRIM(" + dbColumna + ")";
                    break;
            }

            return strRTrimStrim;
        }

        //------------------------------------------------------------------
        // Macros públicas de conversión de tipos según tipo BBDD.
        //------------------------------------------------------------------

        /// <summary> Obtener el formato para procesar una columna NULL - Macro </summary>
        /// 
        /// <param name="dbColumna"></param>
        /// <param name="sustValue"></param>
        /// 
        /// <returns> Cadena con formato. </returns>
        /// 
        public string _GCN(string dbColumna, object sustValue)
        {
            return GetCvtNull(_DBEntorno, _DBVersion, dbColumna, sustValue);
        }

        /// <summary> Obtener formato para leer una subcadena de una columna de un fichero - Macro</summary>
        /// 
        /// <remarks> Accede al método GetCvtSubStr</remarks>
        /// <remarks> Asume Entorno y Versión DDBB por defecto al inicializar la clase</remarks>
        /// 
        /// <param name="dbColumna">Nombre de la columna de la BBDD</param>
        /// <param name="posInicial">Posición inicial de la subcadena</param>
        /// <param name="lenPosFinal">Longitud / posición final de la subcadena</param>
        /// 
        /// <seealso cref="GetCvtSubStr(_Tf.DBEnvironments, string, string, int, int)"/>
        /// 
        /// <returns>Formato de acceso a la subcadena de la columna</returns>
        /// 
        public string _GCSS(string dbColumna, int posInicial, int lenPosFinal)
        {
            return GetCvtSubStr(_DBEntorno, _DBVersion, dbColumna, posInicial, lenPosFinal);
        }

        /// <summary> Obtener formato para concatenar columnas de un fichero de la BBDD - Macro</summary>
        /// 
        /// <seealso cref="GetCvtCat(_Tf.DBEnvironments, string)"/>
        /// 
        /// <returns>Columnas concatenadas</returns>
        /// 
        public string _GCC()
        {
            return GetCvtCat(_DBEntorno, _DBVersion);
        }

        /// <summary> Obtener formato para concatenar columnas de un fichero de la BBDD - Macro</summary>
        /// 
        /// <param name="colsToConcat">Columnas a concaternar, en formato Col1, Col2, ---, ColN</param>
        /// 
        /// <seealso cref="GetCvtCat(_Tf.DBEnvironments, string)"/>
        /// 
        /// <returns>Columnas concatenadas</returns>
        /// 
        public string _GCC(params string [] colsToConcat)
        {
            string colsConcatenadas = colsToConcat.Length == 0 ? "" : colsToConcat[0];

            for(int i=1; i<colsToConcat.Length; i++)
                colsConcatenadas += GetCvtCat(_DBEntorno, _DBVersion) + colsToConcat;

            return colsConcatenadas;
        }

        /// <summary> Obtener formato de la función LIKE en sentencias SQL - Método</summary>
        /// 
        /// <seealso cref="GetCvtLike(_Tf.DBEnvironments, string)"/>
        /// 
        /// <returns>Formato para la función LIKE</returns>
        /// 
        public string _GCL()
        {
            return GetCvtLike(_DBEntorno, _DBVersion);
        }

        /// <summary> Obtener formato de la función LIKE en sentencias SQL - Macro</summary>
        /// 
        /// <param name="valueToLike"> Columna a formatear con LIKE</param>
        /// <param name="likeLeft"> Aplicar LIKE a la izquierda. Def: false</param>
        /// <param name="likeRight"> Aplicar LIKE a la derecha. Def: true</param>
        /// <param name="trimLeft"> Eliminar espacios a la izquierda de la columna. Def: false</param>
        /// <param name="trimRight"> Eliminar espacios a la derecha de la columna. Def: false</param>
        /// 
        /// <seealso cref="GetCvtLike(_Tf.DBEnvironments, string)"/>
        /// 
        /// <returns>Columna formateada para la función LIKE</returns>
        /// 
        public string _GCL(string valueToLike, bool likeLeft = false, bool likeRight = true, bool trimLeft = false, bool trimRight = true)
        {
            string strRetorno = valueToLike;
            string strLike = GetCvtLike(_DBEntorno, _DBVersion);

            strRetorno = trimLeft == true ? strRetorno.TrimStart() : strRetorno;
            strRetorno = trimRight == true ? strRetorno.TrimEnd() : strRetorno;

            strRetorno = (likeLeft == true ? strLike : "") + strRetorno;
            strRetorno += (likeRight == true ? strLike : "");

            return strRetorno;
        }

        /// <summary> Eliminar espacios al final de una columna de tipo string - Macro. </summary>
        /// 
        /// <param name="dbColumna"></param>
        /// 
        /// <returns> Columna sin espacios. </returns>
        /// 
        public string _GCRT(string dbColumna)
        {
            return GetCvtRTrim(_DBEntorno, _DBVersion, dbColumna);
        }

        /// <summary>
        /// Obtener una clave de la lista de numeradores.
        /// </summary>
        /// 
        /// <param name="dbEntorno"> Entorno activo de la BBDD</param>
        /// <param name="dbVersion"> Versión de la BBDD</param>
        /// <param name="Numerador"> Código del numerador</param>
        /// <param name="nLongitud"> Nº de caracteres a devolver</param>
        /// <param name="nCuantos"> Incremento numerador (Def: 1) </param>
        /// 
        /// <returns> Cadena con el numerador (null si error) </returns>
        /// 
        public string NewNum(string Numerador, DBEnvironments dbEntorno = DBEnvironments.NULL, string dbVersion = null, int nLongitud = 10, int nCuantos = 1)
        {
            string strRetorno = null;
            string strSQL = "";

            try
            {
                dbEntorno = dbEntorno == DBEnvironments.NULL ? _DBEntorno : dbEntorno;
                dbVersion = dbVersion == null ? _DBVersion : dbVersion;

                switch (dbEntorno)
                {
                    case DBEnvironments.SQL:
                        strSQL = "Select NEXT VALUE FOR '" + Numerador + "'";
                        strRetorno = System.Convert.ToString(ExecDirectSQL(strSQL)).PadLeft(nLongitud, '0');
                        break;

                    case DBEnvironments.DB2: 
                        break;

                    case DBEnvironments.ORACLE:
                        break;

                    case DBEnvironments.MSACCESS:
                        break;

                    case DBEnvironments.VFP:
                        break;

                    default:
                        break;
                }
            }

            catch (OdbcException ex)
            {
                UsrError = ex.Message;
            }

            catch (Exception ex)
            {
                UsrError = ex.Message;
            }

            return strRetorno;
        }

        /// <summary>
        /// Obtener una clave de la lista de numeradores.
        /// </summary>
        /// 
        /// <param name="Numerador"> Código del numerador</param>
        /// <param name="nLongitud"> Nº de caracteres a devolver</param>
        /// <param name="nCuantos"> Incremento numerador (Def: 1) </param>
        /// 
        /// <returns>Cadena con el numerador (null si error)</returns>
        /// 
        public string NewNum(string Numerador, int nLongitud = 10, int nCuantos = 1)
        {
            return NewNum(Numerador, _DBEntorno, _DBVersion, nLongitud, nCuantos);
        }

        //------------------------------------------------------------------
        // Métodos públicos de la clase - General.
        //------------------------------------------------------------------

        /// <summary> Inicializar el estado de la clase</summary>
        /// 
        /// <returns> true / false</returns>
        /// 
        public bool Inicializar(string DBCadenaDeConexion = null,
                                TF._Tf.DBEnvironments DBEntorno = TF._Tf.DBEnvironments.SQL,
                                string DBVersion = "1.0",
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
                        sqlconnection = cnTF;
                    }

                _DBEntorno = DBEntorno;
                _DBVersion = DBVersion;
                _DBProvider = DBProvider;
                _DBLike = _GCL();
                _DBConcat = _GCC();
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

        /// <summary> Finalizar el estado de la clase</summary>
        /// 
        /// <returns> true / false</returns>
        /// 
        public bool Finalizar()
        {
            return true;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        /// <summary> Constructor por defecto de la clase</summary>
        /// 
        public Tf()
        {
            cnTF = new OdbcConnection();

            UsrErrorC = 0;
            UsrError = "";
            UsrErrorE = "";
        }

        /// <summary> Constructor de la clase </summary>
        /// 
        /// <param name="cmConn"> Objeto Command de acceso a la BBDD </param>
        /// 
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

            catch (SqlException ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }
        }

        /// <summary> Constructor de la clase</summary>
        /// 
        /// <param name="strCadenaDeConexion"> Cadena de conexión con la BBDD </param>
        /// 
        public Tf(string strCadenaDeConexion) : this()
        {
            try
            {
                cnTF.ConnectionString = strCadenaDeConexion;
                cnTF.Open();
                sqlconnection = cnTF;
            }

            catch (OdbcException ex)
            {
                // Error BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }

            catch (SqlException ex)
            {
                // Error BBDD.
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

        /// <summary> Destructor de la clase</summary>
        /// 
        /// <remarks> Elimina la conexión con la DDBB</remarks>
        /// 
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

        /// <summary> Sobrecarga de la función general ToString</summary>
        /// 
        /// <remarks> Devuelve una cadena con la descripción de la clase</remarks>
        /// 
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

        /// <summary> Sobrecarga de la función general Equals</summary>
        /// 
        /// <remarks> Compara dos objetos de la clase TF</remarks>
        /// 
        /// <returns> Devuelve true / false</returns>
        /// 
        public override bool Equals(object obj)
        {
            return ((TF.Tf)obj).cnTF.ConnectionString == this.cnTF.ConnectionString;
            // return base.Equals(obj);
        }

        /// <summary> Sobrecarga de la función general GetHashCode</summary>
        /// 
        /// <returns>HashCode</returns>
        /// 
        public override int GetHashCode()
        {
            // If we override Equals, we've to do the same with GetHashCode.
            return base.GetHashCode();
        }
    }
}
