using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LL1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("\n\tIniciando analizador sintáctico LL1...\n");
            
            AnalizadorSintacticoLL1 analizador = new AnalizadorSintacticoLL1();

            string[] cadenas = LeerArchivo("../../entrada.txt").Split('\n');

            bool aceptado = true;
            for(int i = 0; i < cadenas.Length - 1; i++)
            {
                aceptado = aceptado && analizador.Probarcadena(analizador.tabla, analizador.Configurarcadena(cadenas[i].Split(' ')),i + 1, cadenas[i].Length);  
            }

            if (aceptado)
            {
                Console.WriteLine("Cadena aceptada");
            }

            Console.ReadKey();
        }

        public static string LeerArchivo(string direccionArchivo)
        {
            string cadena = "";

            FileStream fileStream = new FileStream(direccionArchivo, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream,Encoding.Default,true))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    cadena+=line + "\n";
                }
            }

            return cadena;
        }

    }
}
