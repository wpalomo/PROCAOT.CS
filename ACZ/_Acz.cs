
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF;

namespace ACZ
{
    /// <summary>
    /// Propiedades y métodos de uso interno de la clase genérica para acceso a funciones de BBDD.
    /// </summary>

    public class _Acz
    {
        //------------------------------------------------------------------
        // Propiedades públicas de control de entorno.
        //------------------------------------------------------------------

        /// <summary> Acceso a BBDD, uso interno.</summary>
        /// 
        public Tf __TFSga;

        /// <summary> Acceso a ficheros XML de configuración (SGA)</summary>
        /// 
        public Xml __TFXmlSga;

        //------------------------------------------------------------------
        // Propiedades públicas de configuración de la clase.
        //------------------------------------------------------------------

        private Char flgoc = 'S';
        /// <summary> Flag actualización de Ocupaciones (S / N). Def: S </summary>
        public Char FlgOC
        {
            get { return flgoc;  }
            set { flgoc = value; }
        }

        private Char flgub = 'S';
        /// <summary> Flag actualización de Ubicaciones (S / N). Def: S </summary>
        public Char FlgUB
        {
            get { return flgub; }
            set { flgub = value; }
        }

        private Char flghm = 'S';
        /// <summary> Flag actualización de Histórico de Movimientos (S / N). Def: S </summary>
        public Char FlgHM
        {
            get { return flghm; }
            set { flghm = value; }
        }

        //------------------------------------------------------------------
        // Propiedades públicas de usuario de la clase.
        //------------------------------------------------------------------

        private String codprodef = null;
        /// <summary> Código de propietario por defecto</summary>
        public String CodProD
        {
            get
            { return codprodef; }

            set
            {
                // Validar que exista el propietario
                char UsrConect = __TFSga.OdbcSeekRow("F01P001", SeekWhere: "F01pCodigo='" + value + "'");

                switch (UsrConect)
                {
                    case 'S':
                        codprodef = value;
                        break;

                    default:
                        break;
                }
            }
        }

        private String sitstkedef = null;
        /// <summary> SSTK stock entrada ocupación por defecto</summary>
        public String SitStkED
        {
            get
            { return sitstkedef; }

            set
            {
                // Validar que exista la SSTK.
                char UsrConect = __TFSga.OdbcSeekRow("F00C001", SeekWhere: "F00cCodStk='" + value + "'");

                switch (UsrConect)
                {
                    case 'S':
                        sitstkedef = value;
                        break;

                    default:
                        break;
                }
            }
        }

        private String tipmovedef = null;
        /// <summary> TMOV para entradas de material por defecto</summary>
        public String TipMovED
        {
            get
            { return tipmovedef; }

            set
            {
                // Validar que exista el TMOV.
                char UsrConect = __TFSga.OdbcSeekRow("F00B001", SeekWhere: "F00bCodMov='" + value + "'");

                switch (UsrConect)
                {
                    case 'S':
                        tipmovedef = value;
                        break;

                    default:
                        break;
                }
            }
        }

        private String tipmovsdef = null;
        /// <summary> TMOV para salidas de material por defecto</summary>
        public String TipMovSD
        {
            get
            { return tipmovsdef; }

            set
            {
                // Validar que exista el TMOV.
                char UsrConect = __TFSga.OdbcSeekRow("F00B001", SeekWhere: "F00bCodMov='" + value + "'");

                switch (UsrConect)
                {
                    case 'S':
                        tipmovsdef = value;
                        break;

                    default:
                        break;
                }
            }
        }

        //------------------------------------------------------------------
        // Propiedades públicas de control de Procesos.
        //------------------------------------------------------------------

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
        // Propiedades públicas de la Clase.
        //------------------------------------------------------------------

        /// <summary> Inicializar las propiedades. </summary>
        /// 
        internal void _inicializar()
        {
            usrerror = "";
            usrerrorc = "";
            usrerrore = "";

            return;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        /// <summary> Constructor por defecto de la clase</summary>
        /// 
        public _Acz()
        {
        }

        //------------------------------------------------------------------
        // Destructor de la clase.
        //------------------------------------------------------------------

        /// <summary> Destructor por defecto de la clase</summary>
        /// 
        ~_Acz()
        {
        }
    }
}
