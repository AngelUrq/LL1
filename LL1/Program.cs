using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Iniciando analizador sintáctico LL1...");

            AnalizadorSintacticoLL1 analizador = new AnalizadorSintacticoLL1();

            Console.ReadKey();
        }
        
    }
}
