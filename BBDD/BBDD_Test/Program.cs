
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBDD;

namespace BBDD_Test
{
    class TestART
    {
        internal void DoSomething()
        {
            ARTICULOS oArt = new();
            BBDD.BBDD.OCUPACIONES oOcu = new BBDD.BBDD.OCUPACIONES();
            <BBDD.BBDD.OCUPACIONES> oBD = new BBDD.BBDD();

            oArt.GetEmptyRecord(ref oArt);
            oOcu.GetEmptyRecord(ref oOcu);
            oBD
            
            return;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TestART oART = new TestART();
            oART.DoSomething();

            return;
        }
    }
}
