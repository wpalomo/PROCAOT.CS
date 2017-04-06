
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF;

namespace ACZ
{
    /// <summary> Clase genérica para funciones de actualización.</summary>
    /// 
    /// <remarks>
    /// Información devuelta, según la función:
    ///     this.
    ///
    /// Gestión de la operación realizada:
    ///     Resultado de la llamada:
    ///         - S: Operación OK.
    ///         - N: Operación no realizada.
    ///         - C: Error BBDD / Error de programa.
    ///
    ///     Mensajes devueltos:
    ///         - this.UsrErrorC, código de error (0: OK).
    ///         - this.UsrError, texto del error.
    ///         - this.UsrErrorE, texto error extendido - pila de llamadas.
    /// </remarks>

    public class Acz : _Acz
    {

        //------------------------------------------------------------------
        // Métodos de usuario:
        //------------------------------------------------------------------

        /// <summary> Leer una ocupación a partir de si ID Row.</summary>
        /// 
        /// <param name="RowID"></param>
        /// 
        /// <returns>
        /// Resultado (S-Existe la ocupación/N-No existe la ocupación/C-Error)
        ///</returns>

        public Char ACZGetOcupacion(string RowID)
        {
            Char retorno = 'S';

            _inicializar();

            try
            {
            }

            catch (Exception ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary>
        /// Función básica para crear una ocupacion.
        /// Recibe los parámetros básicos de la ocupación.
        /// Actualiza datos en ficheros:
        ///     - F16C: Ocupaciones.
        ///     - F10C: Estado ubicación.
        ///     - F20C: Histórico de movimientos.
        /// </summary>
        /// 
        /// <param name="Articulo"> Código de artículo</param>
        /// <param name="Ubicacion"> Código de ubicación</param>
        /// <param name="Cantidad"> Cantidad a ubicar</param>
        /// <param name="Palet"></param>
        /// <param name="Lote"></param>
        /// <param name="Caducidad"></param>
        /// 
        /// <remarks> Parámetros opcionales. Toma por defecto valores cargados al inicializar</remarks>
        /// 
        /// <param name="Propietario"> Código de propietario</param>
        /// <param name="SSTK"> Situación de stock de la ocupación</param>
        /// <param name="TMOV"> Tipo de movimiento para asiento en histórico</param>
        /// <param name="RowID"> RowId de la ocupación, si es para update</param>
        /// 
        /// <returns>
        /// Resultado. S: Ocupación creada OK. N: No se ha podido crear ocupación. C: Error.
        /// </returns>

        public Char ACZCreateOrUpdateOcupacion(string Articulo,
                                               string Ubicacion,
                                               decimal Cantidad,
                                               string Palet,
                                               string Lote,
                                               DateTime Caducidad,
                                               string Propietario = null,
                                               string SSTK = null,
                                               string TMOV = null,
                                               string RowID = null)
        {
            Char retorno = 'S';

            _inicializar();

            try
            {
            }

            catch (Exception ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary>
        /// Actualizar registro en histórico de movimientos.
        /// </summary>
        /// 
        /// <returns>
        /// Resultado (S-Ok/N-No OK/C-Error)
        /// </returns>

        public Char ACZActHM()
        {
            char UsrConect = 'S';

            return UsrConect;
        }

        //------------------------------------------------------------------
        // Métodos públicos de la clase - General.
        //------------------------------------------------------------------

        /// <summary> 
        /// Inicializar el estado de la clase.
        /// </summary>
        /// 
        /// <returns> true / false</returns>

        public char Inicializar()
        {
            char retorno = 'S';

            try
            {
            }

            catch (Exception ex)
            {
                UsrErrorC = "ACZ-0002";
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
                retorno = 'C';
            }

            return retorno;
        }

        /// <summary> Finalizar el estado de la clase</summary>
        /// <returns> true / false</returns>

        public bool Finalizar()
        {
            return true;
        }

        /// <summary>
        /// Inicializar las constantes por defecto de uso de la clase.
        /// </summary>
        /// 
        /// <returns>
        /// true / false
        /// </returns>

        private bool _xmlinicializar()
        {
            bool retorno = false;

            // Valores por defecto.
            CodProD = __TFXmlSga.XmlSeekSingleKeyValue("/SGA/COMMON/DEFAULT/CODPRODEF");            // Propietario por defecto.
            retorno = !String.IsNullOrEmpty(CodProD);

            SitStkED = __TFXmlSga.XmlSeekSingleKeyValue("/SGA/COMMON/DEFAULT/SITSTKEDEF");          // SSTK por defecto de la ocupación.
            retorno = retorno && !String.IsNullOrEmpty(SitStkED);

            TipMovED = __TFXmlSga.XmlSeekSingleKeyValue("/SGA/COMMON/DEFAULT/TIPMOVEDEF");          // TMOV de entrada por defecto.
            retorno = retorno && !String.IsNullOrEmpty(TipMovED);

            TipMovSD = __TFXmlSga.XmlSeekSingleKeyValue("/SGA/COMMON/DEFAULT/TIPMOVSDEF");          // TMOV de salida por defecto.
            retorno = retorno && !String.IsNullOrEmpty(TipMovSD);

            return retorno;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        /// <summary> Constructor por defecto de la clase.</summary>
        /// 
        /// <see cref="Acz.Acz(string, string)"/>
        /// <see cref="Acz.Acz(Tf, Xml)"/>

        public Acz()
        {
        }

        /// <summary> Constructor de la clase</summary>
        /// 
        /// <param name="oTF"> Objeto TF ya inicializado de acceso a la BBDD</param>
        /// <param name="oXML"> Objeto XML ya inicializado de acceso a ficheros XML</param>
        /// 
        /// <seealso cref="Acz.Acz()"/>
        /// <seealso cref="Acz.Acz(string, string)"/>
        /// 
        public Acz(TF.Tf oTF, TF.Xml oXML)
        {
            try
            {
                __TFSga = oTF;
                __TFXmlSga = oXML;
            }

            catch (Exception ex)
            {
                UsrErrorC = "ACZ-0003";
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }
        }

        /// <summary> Constructor de la clase</summary>
        /// 
        /// <param name="strCadenaDeConexion"> Cadena de conexión con la BBDD</param>
        /// <param name="strPathFicheroXML"> PAth fichero XML de configuración de la clase</param>
        /// 
        /// <see cref="Acz.Acz()"/>
        /// <see cref="Acz.Acz(Tf, Xml)"/>
        /// 
        public Acz(string strCadenaDeConexion, string strPathFicheroXML)
        {
            try
            {
                __TFSga = new TF.Tf(strCadenaDeConexion);
                __TFXmlSga = new TF.Xml(strPathFicheroXML);
            }

            catch (Exception ex)
            {
                UsrErrorC = "ACZ-0004";
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

        /// <summary> Destructor de la clase.</summary>

        ~Acz()
        {
            try
            {
            }

            catch (Exception ex)
            {
                UsrErrorC = "ACZ-0005";
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        /// <summary> Sobrecarga de la función general ToString</summary>
        /// <remarks> Devuelve una cadena con la descripción de la clase</remarks>

        public override string ToString()
        {
            string retorno = null;

            try
            {
                retorno = this.ToString();
            }

            catch (Exception ex)
            {
                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }

            return retorno;
        }

        /// <summary> Sobrecarga de la función general Equals</summary>
        /// <remarks> Compara dos objetos de la clase TF</remarks>
        /// <returns> Devuelve true / false</returns>

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary> Sobrecarga de la función general GetHashCode</summary>

        public override int GetHashCode()
        {
            // If we override Equals, we've to do the same with GetHashCode.
            return base.GetHashCode();
        }
    }
}
