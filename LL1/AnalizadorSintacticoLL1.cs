using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1
{
    class AnalizadorSintacticoLL1
    {

        List<Produccion> listaProducciones;
        List<Produccion> nuevasreglas = new List<Produccion>();

        private List<string> terminales;
        private List<string> noTerminales;

        public static string[] Resultado;
        public static char[] noTerminalesChar;

        private List<string> primeros;
        private List<string> siguientes;

        int posicion = 0;
        int posicionfinal = 0;

        string HayTerminal = "false";
        string HayEpsilon = "false";

        private string[,] tabla;

        public AnalizadorSintacticoLL1()
        {
            primeros = new List<string>();
            siguientes = new List<string>();
            terminales = new List<string>();
            noTerminales = new List<string>();

            IniciarProducciones();
            CalcularPrimeros();

            /*tabla = new string[noTerminales.Count + 1, terminales.Count + 2];

            InicializarTabla();
            RellenarTabla();
            MostrarMatriz();*/
        }

        public List<Produccion> Factorizar_izquierda()
        {
            List<List<String>> produccionestotal = new List<List<String>>();
            List<String> producciones = new List<String>();

            bool ver = true;
            producciones.Add("♣");
            for (int x = 0; x < listaProducciones.Count; x++)
            {
                List<String> datos = new List<string>();
                for (int y = 0; y < producciones.Count; y++)
                {
                    if (listaProducciones[x].GetLadoIzquierdo() == producciones[y])
                    {
                        ver = !ver;
                    }
                }
                if (ver)
                {
                    producciones.Add(listaProducciones[x].GetLadoIzquierdo());
                    for (int j = 0; j < listaProducciones.Count; j++)
                    {
                        if (producciones[producciones.Count - 1] == listaProducciones[j].GetLadoIzquierdo())
                        {
                            datos.Add(listaProducciones[j].GetLadoDerecho());
                        }
                    }
                    produccionestotal.Add(datos);
                }
                ver = true;

            }


            return Reescribirreglas(producciones, produccionestotal);
        }

        public List<Produccion> Reescribirreglas(List<String> ld, List<List<String>> producciones)
        {
            List<Produccion> reglasrestantes = new List<Produccion>();

            for (int x = 0; x < producciones.Count; x++)
            {
                int posicion = 0;
                Boolean ver = true;

                for (int y = 0; y < producciones[x].Count; y++)
                {
                    for (int j = 0; j < producciones[x].Count; j++)
                    {
                        if (producciones[x][y][0] == producciones[x][j][0] && j != y)
                        {
                            ver = false;
                            reglasrestantes.Add(new Produccion(ld[x + 1], producciones[x][y]));
                        }
                    }
                    if (ver)
                    {
                        nuevasreglas.Add(new Produccion(ld[x + 1], producciones[x][y]));
                        Console.WriteLine("regla a añadir " + ld[x + 1] + " " + producciones[x][y]);
                    }
                    ver = true;
                }
            }
            List<Produccion> aux = new List<Produccion>();
            String nuevaregla = "";
            List<int> numeros = new List<int>();
            for (int j = 0; j < reglasrestantes.Count; j++)
            {
                nuevaregla = "";
                int countt = 0;
                numeros.Add(123);
                List<int> verificar = new List<int>();
                if (Ver_repetidonum(numeros, j))
                {
                    numeros.Add(j);
                    verificar.Add(j);
                    for (int y = 0; y < reglasrestantes.Count; y++)
                    {
                        if (reglasrestantes[j].GetLadoDerecho()[0] == reglasrestantes[y].GetLadoDerecho()[0] &&
                        reglasrestantes[j].GetLadoIzquierdo() == reglasrestantes[y].GetLadoIzquierdo() && j != y)
                        {
                            numeros.Add(y);
                            verificar.Add(y);
                            countt++;
                        }
                    }

                    bool camino = true;
                    int pos = 0;
                    int n1 = 0;
                    while (camino)
                    {
                        n1 = 0;
                        for (int x = 0; x < verificar.Count; x++)
                        {
                            if (pos < reglasrestantes[j].GetLadoDerecho().Length && pos < reglasrestantes[verificar[x]].GetLadoDerecho().Length)
                            {
                                if (reglasrestantes[j].GetLadoDerecho()[pos] == reglasrestantes[verificar[x]].GetLadoDerecho()[pos])
                                {
                                    n1++;
                                }
                            }

                        }

                        if (n1 == (countt + 1))
                        {
                            nuevaregla += reglasrestantes[j].GetLadoDerecho()[pos];
                            pos++;
                        }
                        else
                        {
                            camino = false;
                        }

                    }

                }
                if (nuevaregla != "")
                {
                    Ver_repetido(reglasrestantes[j].GetLadoIzquierdo(), nuevaregla + reglasrestantes[j].GetLadoIzquierdo());
                    for (int h = 0; h < verificar.Count; h++)
                    {
                        if (reglasrestantes[verificar[h]].GetLadoDerecho().Length <= nuevaregla.Length || reglasrestantes[verificar[h]].GetLadoDerecho().Substring(nuevaregla.Length) == "")
                        {
                            Ver_repetido(reglasrestantes[j].GetLadoIzquierdo(), "£");
                        }
                        else
                        {
                            Ver_repetido(reglasrestantes[j].GetLadoIzquierdo(), reglasrestantes[verificar[h]].GetLadoDerecho().Substring(nuevaregla.Length));
                        }
                    }
                }
            }
            return nuevasreglas;
        }
        public void Ver_repetido(String iz, String ld)
        {
            bool ver = true;
            for (int x = 0; x < nuevasreglas.Count; x++)
            {
                if (nuevasreglas[x].GetLadoIzquierdo() == iz &&
                           nuevasreglas[x].GetLadoDerecho() == ld)
                {
                    ver = !ver;
                }
            }
            if (ver)
            {
                nuevasreglas.Add(new Produccion(iz, ld));
            }
        }
        public Boolean Ver_repetidonum(List<int> lista, int num)
        {
            bool ver = true;
            for (int x = 1; x < lista.Count; x++)
            {
                if (lista[x] == num)
                {
                    ver = !ver;
                }
            }
            return ver;
        }

        string a = "";
        private void EliminarRecursividadIzquierda()
        {
            string cadena1 = "";
            for (int i = 0; i < listaProducciones.Count; i++)
            {
                cadena1 = cadena1 + listaProducciones[i].GetLadoIzquierdo().ToString() + "->" + listaProducciones[i].GetLadoDerecho().ToString() + "\n";
            }

            Clase_Separador cs = new Clase_Separador();
            cs.Separar(cadena1);
            for (int i = 0; i < cs.beta.Count; i++)
            {
                a = a + cs.alfa[i] + "->" + cs.beta[i] + "\n";
            }
            for (int i = 0; i < cs.nombreRegla.Count; i++)
            {
                a = a + cs.nombreRegla[i] + "->" + cs.resultado[i] + "\n";
            }
        }


        private void CalcularPrimeros()
        {
            posicion = 0;
            posicionfinal = noTerminales.Count;
            noTerminalesChar = string.Join(string.Empty, noTerminales).ToCharArray();

            for (int i = 0; i < terminales.Count; i++)
            {
                char[] characters = terminales[i].ToCharArray();
                char[] charaux;
                char[] charaux2;

                if (posicion == 0)
                {
                    for (int j = 0; j < characters.Length; j++)
                    {
                        Console.WriteLine(characters[j]);
                        Console.WriteLine("P ( " + noTerminales[i] + " ) = {");
                        if (Char.IsUpper(characters[j])) //Verifica si el primer caracter es No Terminal   
                        {
                            for (int s = 0; s < noTerminales.Count; s++) //Recorre los No terminales
                            {
                                if (characters[0].Equals(noTerminalesChar[s]))//Recorre para encontrar el noterminal con el que se encontro
                                {
                                    charaux = terminales[s].ToCharArray();
                                    for (int ca = 0; ca < charaux.Length; ca++)
                                    {
                                        if (Char.IsUpper(charaux[ca]))
                                        {
                                            for (int s1 = 0; s1 < noTerminales.Count; s1++) //Recorre los No terminales
                                            {
                                                if (charaux[ca].Equals(noTerminalesChar[s1]))
                                                {
                                                    charaux2 = terminales[s1].ToCharArray();
                                                    for (int caa = 0; caa < charaux2.Length; caa++)
                                                    {

                                                        if (Char.IsLower(charaux2[caa]) && charaux2[caa] != '€')
                                                        {
                                                            Console.WriteLine(charaux2[caa]);

                                                        }
                                                        else if (charaux2[caa].Equals('€'))
                                                        {
                                                            HayEpsilon = "true";
                                                        }


                                                    }
                                                }

                                            }
                                        }
                                        else if (Char.IsUpper(charaux[ca]) && HayEpsilon.Equals("true"))
                                        {
                                            Console.WriteLine(characters[j]);
                                            HayEpsilon = "false";
                                        }
                                        else if (Char.IsLower(charaux[ca]))
                                        {
                                            Console.WriteLine(charaux[ca]);
                                            HayEpsilon = "false";
                                        }
                                        else if (charaux[ca].Equals('€'))
                                        {
                                            HayEpsilon = "true";
                                        }
                                    }
                                }
                            }
                        }
                        //En el caso que la siguiente sea terminal y se haya encontrado un epsilon
                        else if (Char.IsUpper(characters[j]) && HayEpsilon.Equals("true"))
                        {
                            Console.WriteLine(characters[j]);
                            HayEpsilon = "false";
                        }
                        else if (Char.IsLower(characters[j]) || characters[j].Equals('e')) //e se clasifica como epsilon
                        {
                            Console.WriteLine(characters[j]);
                            HayEpsilon = "false";
                        }
                        else if (Char.IsLower(characters[j]))
                        {
                            Console.WriteLine(characters[j]);
                        }

                        Console.WriteLine("}");
                    }
                }


            }
        }

        private void CalcularSiguientes()
        {
            //Parte de Diego
        }

        public void InicializarTabla()
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    tabla[i, j] = "";
                }
            }

            int contador = 0;
            int noTerm = 0;

            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    if (i == 0 && j >= 1)
                    {
                        if (contador == terminales.Count)
                        {
                            tabla[i, j] = "$";
                        }
                        else
                        {
                            if (contador < terminales.Count)
                            {
                                tabla[i, j] = terminales[contador].ToString();
                                contador++;
                            }
                        }
                    }
                    else if (i > 0 && j == 0)
                    {
                        tabla[i, j] = noTerminales[noTerm];
                        noTerm++;
                    }
                }
            }
        }

        public void MostrarMatriz()
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    Console.Write(tabla[i, j] + " | ");
                }
                Console.WriteLine();
            }
        }

        public void RellenarTabla()
        {
            List<string> prim = new List<string>();
            List<string> noTerminalesPrimeros = new List<string>();
            List<string> noTerminalesSiguientes = new List<string>();
            List<string> listaDeLosSiguientes = new List<string>();

            int numeroColumna = 0;
            int numeroFila = 0;

            foreach (string primero in primeros)
            {
                string p = primero.Substring(3, primero.Length - 3);
                string noTermPrimeros = primero.Substring(0, 1);
                prim.Add(p);
                noTerminalesPrimeros.Add(noTermPrimeros);
            }

            foreach (string siguiente in siguientes)
            {
                string s = siguiente.Substring(0, 1);
                string noTermSiguientes = siguiente.Substring(3, siguiente.Length - 3);
                noTerminalesSiguientes.Add(s);
                listaDeLosSiguientes.Add(noTermSiguientes);
            }


            foreach (Produccion regla in listaProducciones)
            {
                if (regla.GetLadoDerecho()[0].Equals('€'))
                {
                    for (int k = 0; k < noTerminalesSiguientes.Count; k++)
                    {
                        if (regla.GetLadoIzquierdo().ToString().Equals(noTerminalesSiguientes[k]))
                        {
                            string[] elementosSiguientes = listaDeLosSiguientes[k].Split(',');

                            for (int l = 0; l < elementosSiguientes.Length; l++)
                            {
                                for (int m = 1; m < tabla.GetLength(1); m++)
                                {
                                    if (elementosSiguientes[l].Equals(tabla[0, m]))
                                    {
                                        int i = 0;

                                        for (int n = 0; n < tabla.GetLength(0); n++)
                                        {
                                            if (regla.GetLadoIzquierdo().ToString().Equals(tabla[n, 0]))
                                            {
                                                i = n;
                                                break;
                                            }
                                        }

                                        tabla[i, m] = "€";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Char.IsLetter(regla.GetLadoDerecho()[0]) && regla.GetLadoDerecho()[0].ToString().Equals(regla.GetLadoDerecho()[0].ToString().ToUpper()))
                {
                    for (int i = 0; i < noTerminalesPrimeros.Count; i++)
                    {
                        if (noTerminalesPrimeros[i].Equals(regla.GetLadoDerecho()[0].ToString()))
                        {
                            string[] elementosPrimeros = prim[i].Split(',');

                            for (int j = 0; j < elementosPrimeros.Length; j++)
                            {
                                if (elementosPrimeros[j].Equals("€"))
                                {
                                    for (int k = 0; k < noTerminalesSiguientes.Count; k++)
                                    {
                                        if (regla.GetLadoDerecho()[0].ToString().Equals(noTerminalesSiguientes[k]))
                                        {
                                            string[] elementosSiguientes = listaDeLosSiguientes[k].Split(',');

                                            for (int l = 0; l < elementosSiguientes.Length; l++)
                                            {
                                                for (int m = 1; m < tabla.GetLength(1); m++)
                                                {
                                                    if (elementosSiguientes[l].Equals(tabla[0, m]))
                                                    {
                                                        bool reglaEpsilon = false;

                                                        for (int n = 0; n < listaProducciones.Count; n++)
                                                        {
                                                            if (listaProducciones[n].GetLadoIzquierdo().Equals(regla.GetLadoIzquierdo()) && listaProducciones[n].GetLadoDerecho().Equals("€"))
                                                            {
                                                                tabla[k, m] = "€";
                                                                reglaEpsilon = true;
                                                                break;
                                                            }
                                                        }

                                                        if (!reglaEpsilon)
                                                        {
                                                            tabla[k, m] = regla.GetLadoDerecho();
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < tabla.GetLength(1); k++)
                                    {
                                        if (tabla[0, k].Equals(elementosPrimeros[j]))
                                        {
                                            for (int l = 0; l < tabla.GetLength(0); l++)
                                            {
                                                if (tabla[l, 0].Equals(regla.GetLadoIzquierdo()))
                                                {
                                                    tabla[l, k] = regla.GetLadoDerecho();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Al encontrar un terminal en la produccion busca la posicion  en la tabla que le corresponde 
                    for (int i = 0; i < tabla.GetLength(0); i++)
                    {
                        if (i == 0)
                        {
                            for (int j = 1; j < tabla.GetLength(1); j++)
                            {
                                if (regla.GetLadoDerecho()[0].ToString().Equals(tabla[0, j]))
                                {
                                    numeroColumna = j;
                                }
                            }
                        }
                        else
                        {
                            if (regla.GetLadoIzquierdo().ToString().Equals(tabla[i, 0]))
                            {
                                numeroFila = i;
                            }
                        }
                    }

                    tabla[numeroFila, numeroColumna] = regla.GetLadoDerecho();
                }
            }
        }


        private void IniciarProducciones()
        {
            listaProducciones = new List<Produccion>();
            listaProducciones.Add(new Produccion("S", "Aa"));
            listaProducciones.Add(new Produccion("A", "BD"));
            listaProducciones.Add(new Produccion("B", "b"));
            listaProducciones.Add(new Produccion("B", "€"));
            listaProducciones.Add(new Produccion("B", "b"));
            listaProducciones.Add(new Produccion("B", "€"));

            terminales.Add("a");
            terminales.Add("b");
            terminales.Add("d");

            noTerminales.Add("S");
            noTerminales.Add("A");
            noTerminales.Add("B");
            noTerminales.Add("D");
        }

    }
}
