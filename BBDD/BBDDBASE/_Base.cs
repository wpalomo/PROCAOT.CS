
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF;

namespace BBDD
{
    /// <summary>
    /// Propiedades y métodos de uso interno de la clase de acceso a ficheros de la BBDD.
    /// </summary>
    public class _Base
    {
        /// <summary> Acceso a BBDD, uso interno.</summary>
        /// 
        public Tf __TFDb;

        /// <summary> Acceso a fichero XML de configuración (RF).</summary>
        /// 
        public Xml __TFXmlDb;

        //------------------------------------------------------------------
        // Propiedades públicas de control de SQL.
        //------------------------------------------------------------------

        /// <summary> Código de error. </summary>
        public int UsrErrorC
        { get; set; }

        /// <summary> Mensajes de error. </summary>
        public String UsrError
        { get; set; }

        /// <summary> Mensaje de error extendido. </summary>
        public String UsrErrorE
        { get; set; }

        /// <summary> Inicializar las propiedades de la clase </summary>
        internal void _inicializar()
        {
            UsrError = "";
            UsrErrorC = 0;
            UsrErrorE = "";

            return;
        }

    }
}
