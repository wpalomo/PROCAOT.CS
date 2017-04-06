using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlTest
{
    class Test
    {
        // Lectura de XML: Nodos - Versión 1.
        public void xmlTest1(string strFichero)
        {
            XmlDocument xDoc = new XmlDocument();

            Console.Clear();

            if (System.IO.File.Exists(strFichero) == false)
            {
                Console.WriteLine("No existe el fichero {0}", strFichero);
                Console.ReadKey();
                return;
            }

            xDoc.Load(strFichero);
            XmlNodeList lstCommon = xDoc.GetElementsByTagName("CROSS-DOCKING");
            XmlNodeList eleLista = ((XmlElement)lstCommon[0]).GetElementsByTagName("MUELLESCD");
            XmlNodeList eleDet = ((XmlElement)lstCommon[0]).GetElementsByTagName("OPERARIOSCD");
            XmlNodeList eleDet2 = ((XmlElement)lstCommon[0]).GetElementsByTagName("OPERARIOCD");

            Console.Clear();

            if (lstCommon.Count==0)
            {
                Console.WriteLine("No hay elementos en {0}", lstCommon.ToString());
                Console.ReadKey();
            }
            if (eleLista.Count == 0)
            {
                Console.WriteLine("No hay elementos en {0}", eleLista.ToString());
                Console.ReadKey();
            }
            if (eleDet.Count == 0)
            {
                Console.WriteLine("No hay elementos en {0}", eleDet.ToString());
                Console.ReadKey();
            }
            if (eleDet2.Count == 0)
            {
                Console.WriteLine("No hay elementos en {0}", eleDet2.ToString());
                Console.ReadKey();
            }

            Console.WriteLine("Nombre:{0} Concepto:{1} Descripción:{2}", lstCommon[0].Name, lstCommon[0].Attributes[0].Name, lstCommon[0].Attributes[0].Value);

            // Modo A: leer a partir del contador de elementos.
            for (int i = 0; i < eleDet2.Count; i++)
            {
                XmlElement nodNodo = (XmlElement)eleDet2[i];
                Console.WriteLine("Código: {0} Nombre:{1}", nodNodo["CODIGO"].InnerText, nodNodo["NOMBRE"].InnerText);
            }

            Console.ReadKey();

            return;
        }

        // Lectura de XML: Nodos - Versión 2.
        public void xmlTest11(string strFichero)
        {
            XmlDocument xDoc = new XmlDocument();

            Console.Clear();

            if (System.IO.File.Exists(strFichero) == false)
            {
                Console.WriteLine("No existe el fichero {0}", strFichero);
                Console.ReadKey();
                return;
            }

            xDoc.Load(strFichero);
            XmlNodeList lstCommon = xDoc.SelectNodes("/PRORAD/COMMON/PATHS/PATHPROCAOT");

            Console.Clear();

            if (lstCommon.Count==0)
            {
                Console.WriteLine("No hay elementos en {0}", lstCommon.ToString());
                Console.ReadKey();
                return;
            }

            // Modo A: leer a partir del contador de elementos.
            for (int i = 0; i < lstCommon.Count; i++)
            {
                XmlNode nodNodo = lstCommon.Item(i);
                Console.WriteLine("(Modo b) ID:{0} Valor:{1} Concepto:{2}", nodNodo["ID"].InnerText, nodNodo["valor"].InnerText , nodNodo["concepto"].InnerText);
            }

            Console.ReadKey();

            // Modo B: Leer a partir de los objetos.
            foreach (XmlNode nodNodo in lstCommon)
            {
                Console.WriteLine("(Modo A) ID:{0} Valor:{1} Concepto:{2}", nodNodo["ID"].InnerText, nodNodo["valor"].InnerText, nodNodo["concepto"].InnerText);
            }

            Console.ReadKey();

            return;
        }

        // Lectura de XML: Atributos - Versión 1.
        public void xmlTest2(string strFichero)
        {
            XmlDocument xDoc = new XmlDocument();

            Console.Clear();

            if (System.IO.File.Exists(strFichero) == false)
            {
                Console.WriteLine("No existe el fichero {0}", strFichero);
                Console.ReadKey();
                return;
            }

            xDoc.Load(strFichero);
            XmlNodeList lstCommon = xDoc.SelectNodes("/PRORAD/COMMON/VFP");             // Lista de nodos.
            XmlNode nodCommon = xDoc.SelectSingleNode("/PRORAD/COMMON/VFP/CENTURY");    // Nodo simple.

            Console.Clear();

            if (lstCommon.Count==0)
            {
                Console.WriteLine("No hay elementos en lstCommon {0}", lstCommon.ToString());
                Console.ReadKey();
            }

            if (nodCommon==null)
            {
                Console.WriteLine("No hay elementos en {0}", "NodCommon");
                Console.ReadKey();
            }

            // Modo A: leer a partir del contador de elementos.
            for (int i = 0; i < lstCommon.Count; i++)
            {
                XmlNode nodNodo = lstCommon.Item(i);
                Console.WriteLine("NodeList (Modo A) Descripción:{0} Valor:{1}", nodNodo["DEBUGSQL"].Attributes[0].Value, nodNodo["DEBUGSQL"].InnerText);
            }

            Console.ReadKey();

            // Modo B: Leer a partir de los objetos.
            foreach (XmlNode nodNodo in lstCommon)
            {
                Console.WriteLine("NodeList (Modo B) Descripción:{0} Valor:{1}", nodNodo["DEBUGSQL"].Attributes[0].Value, nodNodo["DEBUGSQL"].InnerText);
            }

            Console.WriteLine("Nodo Simple Nombre:{0} Descripción:{1} Valor:{2}", nodCommon.Name, nodCommon.Attributes[0].Value, nodCommon.InnerText);

            Console.ReadKey();

            return;
        }
        
        // Lectura de XML: Atributos - Versión 2.
        public void xmlTest22(string strFichero)
        {
            XmlDocument xDoc = new XmlDocument();

            Console.Clear();

            if (System.IO.File.Exists(strFichero) == false)
            {
                Console.WriteLine("No existe el fichero {0}", strFichero);
                Console.ReadKey();
                return;
            }

            xDoc.Load(strFichero);

            XmlNodeList lstLista = xDoc.SelectNodes("/PRORAD/COMMON/FICHEROS");
            XmlNode nodLista = xDoc.SelectSingleNode("/PRORAD/COMMON/PATHS/PATHPROCAOT");

            // Test operarios Cross-Docking.
            XmlNodeList nodOperLista = xDoc.SelectNodes("/PRORAD/COMMON/CROSS-DOCKING/OPERARIOSCD");
            XmlNode nodOper = nodOperLista[0];

            // Test parámetros Default.
            XmlNodeList nodDefault = xDoc.SelectNodes("/PRORAD/COMMON/DEFAULT");
            XmlNode nodDigControl = nodDefault[0];
            XmlNode nodAgrList = nodDefault[1];

            Console.WriteLine("Concepto {0} Valor {1}", nodLista.Attributes[0].Value, nodLista.InnerText);
            Console.ReadKey();

            // Las dos formas de obtener el valor de los atributos.
            Console.WriteLine("Concepto {0} Activo {1} Valor {2}", nodOper["OPERARIOCD"].Attributes[0].Value, nodOper["OPERARIOCD"].Attributes[1].Value, nodOper["OPERARIOCD"].InnerText);
            Console.WriteLine("Concepto {0}", nodOperLista[1]["OPERARIOCD"].Attributes[0].Value);
            Console.ReadKey();

            // Las dos formas de visualizar parámetros.
            Console.WriteLine("Dig. Control {0} Agr. Lista {1}", nodDigControl["DIGCONTROL"].Attributes[0].Value, nodDigControl["AGRLISTA"].Attributes[0].Value);
            Console.WriteLine("Dig. Control {0} Agr. Lista {1}", nodDefault[0]["DIGCONTROL"].Attributes[0].Value, nodDefault[0]["AGRLISTA"].Attributes[0].Value);
            Console.ReadKey();

            Console.Clear();

            if (lstLista.Count == 0)
            {
                Console.WriteLine("No hay elementos en {0}", lstLista.ToString());
                Console.ReadKey();
                return;
            }

            foreach (XmlElement nodNodo in lstLista) // lstLista.Item(0))
            {
                Console.WriteLine("ID:{0} Valor:{1}", nodNodo.InnerText, nodNodo.GetAttribute("concepto"));
            }

            Console.ReadKey();

            if(nodOperLista.Count==0)
            {
                Console.WriteLine("No hay elementos en {0}", nodOperLista.ToString());
                Console.ReadKey();
                return;
            }

            // Lectura de nodos operarios CD.



            return;
        }

        // Modo C: Leer nodos con XmlReader.
        public void xmlTest3(string strFichero)
        {
            if (System.IO.File.Exists(strFichero) == false)
            {
                Console.WriteLine("No existe el fichero {0}", strFichero);
                Console.ReadKey();
                return;
            }

            Console.Clear();

            XmlReader xRead = XmlReader.Create(strFichero);

            xRead.MoveToContent();

            while (xRead.Read())
            {
                if (xRead.IsStartElement())
                {
                    switch (xRead.Name)
                    {
                        case "ID":
                        case "valor":
                        case "concepto":
                            string strClave = xRead.Name;
                            if (xRead.Read())
                            {
                                Console.WriteLine("{0} {1}", strClave, xRead.Value);
                                Console.ReadKey();
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            Console.ReadKey();

            return;
        }
    }

    class main
    {
        static void Main(string[] args)
        {
            Test oTS = new Test();

            //oTS.xmlTest1("D:/PROYECTOS/TORSESA/SGA.NET/PRORAD.CS/PRORAD/bin/Debug/RADIO.XML");
            //oTS.xmlTest11("D:/PROYECTOS/TORSESA/SGA.NET/PRORAD.CS/PRORAD/bin/Debug/RADIO.XML");
            //oTS.xmlTest2("D:\\PROYECTOS\\TORSESA\\SGA.NET\\PRORAD.CS\\PRORAD\\bin\\Debug\\RADIO.XML");
            oTS.xmlTest22("D:/PROYECTOS/TORSESA/SGA.NET/PRORAD.CS/PRORAD/bin/Debug/RADIO.XML");
            //oTS.xmlTest3("D:/PROYECTOS/TORSESA/SGA.NET/PRORAD.CS/PRORAD/bin/Debug/RADIO.XML");

            return;
        }
    }
}
