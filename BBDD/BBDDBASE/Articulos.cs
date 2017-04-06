
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using TF;

namespace BBDD
{
    public class ARTICULOS : _Base
    {
        public Char GetEmptyRecord(ref ARTICULOS registro)
        {
            Char retorno;

            try
            {
                retorno = 'S';
            }

            catch (OdbcException ex)
            {
                // Error de BBDD.
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }
    }
}
