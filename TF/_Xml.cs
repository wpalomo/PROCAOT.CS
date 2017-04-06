
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TF
{
    /// <summary>
    /// Propiedades y métodos de uso interno de la clase genérica para acceso a ficheros en formato XML.
    /// </summary>

    public class _Xml
    {
        //------------------------------------------------------------------
        // Propiedades públicas de control de XML.
        //------------------------------------------------------------------

        // Código de error XML.
        internal int xmlstatus;
        /// <summary></summary>
        public int XmlStatus
        {
            get { return xmlstatus; }
            private set { xmlstatus = value; }
        }

        // Nº de elementos afectados.
        internal int xmlnoofelements;
        /// <summary></summary>
        public int XmlNoOfElements
        {
            get { return xmlnoofelements; }
            private set { xmlnoofelements = value; }
        }

        // Código de error de programa.
        private int usrerrorc;
        /// <summary></summary>
        public int UsrErrorC
        {
            get { return usrerrorc; }
            set { usrerrorc = value; }
        }

        // Mensajes de error.
        private string usrerror;
        /// <summary></summary>
        public String UsrError
        {
            get { return usrerror; }
            set { usrerror = value; }
        }

        // Mensaje de error extendido.
        private string usrerrore;
        /// <summary></summary>
        public String UsrErrorE
        {
            get { return usrerrore; }
            set { usrerrore = value; }
        }

        //------------------------------------------------------------------
        // Propiedades públicas de XML.
        //------------------------------------------------------------------

        // XmlDocumento de retorno.
        internal XmlDocument xmldocument;
        /// <summary></summary>
        public XmlDocument XMLDocument
        {
            set { xmldocument = value; }
            get { return xmldocument; }
        }

        // Key value de retorno.
        internal string xmlkeyvalue;
        /// <summary></summary>
        public string XMLKeyValue
        {
            set { xmlkeyvalue = value; }
            get { return xmlkeyvalue; }
        }

        // Attribute value de retorno.
        internal string xmlattrvalue;
        /// <summary></summary>
        public string XMLAttrValue
        {
            set { xmlattrvalue = value; }
            get { return xmlattrvalue; }
        }

        // Inicializar las propiedades.
        internal void _inicializar()
        {
            xmlnoofelements = -1;
            xmlstatus = 0;
            usrerror = "";
            usrerrorc = 0;
            usrerrore = "";

            return;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        /// <summary> Constructor por defecto de la clase</summary>

        public _Xml()
        {
        }

        //------------------------------------------------------------------
        // Destructor de la clase.
        //------------------------------------------------------------------

        //~_Xml()
        //{
        //}
    }
}
