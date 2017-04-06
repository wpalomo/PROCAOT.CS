
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
    /// <summary> Clase genérica para acceso a ficheros XML</summary>

    public class Xml : _Xml
    {

        //------------------------------------------------------------------
        // Métodos de usuario.
        //------------------------------------------------------------------

        /// <summary>
        /// Cargar el contenido de un archivo en formato XML
        /// </summary>
        /// <param name="pathFichero">Nombre del fichero XML a cargar</param>
        /// <returns>Resultado (S/N)</returns>

        public Char LoadXmlFile(string pathFichero)
        {
            Char retorno = 'S';

            _inicializar();

            try
            {
                if (System.IO.File.Exists(pathFichero) == false)
                {
                    xmlstatus = -2;

                    UsrErrorC = -2;
                    UsrError = "No existe";
                    UsrErrorE = String.Format("El fichero {0} XML no existe", pathFichero);
                    retorno = 'N';

                    return retorno;
                }

                XMLDocument = new XmlDocument();
                XMLDocument.Load(pathFichero);
            }

            catch (Exception ex)
            {
                xmlstatus = -1;

                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary> Obtener el valor de una clave - Devuelve estado</summary>
        /// <remarks>
        /// Lee un fichero XML cargado previamente
        /// Devuelve resultado operación
        /// Devuelve el valor de la clave como parámetro de salida
        /// </remarks>
        /// <param name="xDoc"> Nombre del fichero XML</param>
        /// <param name="keyName"> Nombre de la clave (Nodo) a leer</param>
        /// <param name="keyValue"> Valor de la clave (Nodo) a leer</param>
        /// <returns>Resultado (S/N)</returns>

        public char XmlSeekSingleKeyValue(XmlDocument xDoc, string keyName, out string keyValue)
        {
            char retorno = 'S';
            keyValue = "";

            try
            {
                XmlNode nodNodo = xDoc.SelectSingleNode(keyName);
                if(nodNodo == null)
                {
                    // No existe la clave.
                    xmlstatus = -3;

                    UsrErrorC = -3;
                    UsrError = "Clave no existe";
                    UsrErrorE = String.Format("La clave {0} XML no existe", keyName);
                    retorno = 'N';

                    return retorno;
                }

                keyValue = nodNodo.InnerText;
            }

            catch (Exception ex)
            {
                xmlstatus = -1;

                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        /// <summary> Obtener el valor de una clave - Devuelve valor</summary>
        /// <remarks>
        /// Lee un fichero XML cargado previamente.
        /// Devuelve directamente el valor de la clave como parámetro de salida.
        /// </remarks>
        /// <param name="keyName"> Nombre de la clave (Nodo) a leer</param>
        /// <param name="xDoc"> Nombre del fichero XML. Def: XDocumento por defecto</param>
        /// <returns>Valor de la clave (Nodo) solicitada</returns>

        public string XmlSeekSingleKeyValue(string keyName, XmlDocument xDoc = null)
        {
            string keyValue = null;

            try
            {
                XmlSeekSingleKeyValue(xDoc==null ? this.XMLDocument : xDoc, keyName, out keyValue);
            }

            catch (Exception ex)
            {
                xmlstatus = -1;

                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;
            }

            return keyValue;
        }

        /// <summary> Obtener el valor de una clave - Devuelve valor</summary>
        /// <remarks>
        /// Lee un fichero XML cargado previamente
        /// Devuelve directamente el valor de la clave como parámetro de salida
        /// </remarks>
        /// <param name="keyName"> Nombre de la clave (Nodo) a leer</param>
        /// <param name="docName"> Nombre del fichero XML a cargar</param>
        /// <returns>Valor de la clave (Nodo) solicitada</returns>

        public string XmlSeekSingleKeyValue(string keyName, string docName)
        {
            string keyValue = "";

            return keyValue;
        }

        /// <summary> Obtener el atributo de una clave - Devuelve resultado</summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Resultado (S/N)</returns>


        // Obtener un atributo de una clave.
        public char XmlSeekSingleKeyAttribute()
        {
            char retorno = 'S';

            try
            {

            }

            catch (Exception ex)
            {
                xmlstatus = -1;

                UsrErrorC = ex.HResult;
                UsrError = ex.Message;
                UsrErrorE = ex.StackTrace;

                retorno = 'C';
            }

            return retorno;
        }

        //------------------------------------------------------------------
        // Constructores de la clase.
        //------------------------------------------------------------------

        /// <summary> Constructor por defecto de la clase</summary>

        public Xml()
        {
        }

        /// <summary> Constructor de la clase</summary>
        /// <param name="pathFichero"> Path fichero en formato XML a cargar</param>

        public Xml(string pathFichero)
        {
            LoadXmlFile(pathFichero);
        }
    }
}
