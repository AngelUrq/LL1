using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LL1
{
    class AnalizadorSintacticoLL1
    {

        List<Produccion> listaProducciones;
        List<Produccion> nuevasreglas = new List<Produccion>();

        private List<string> terminales;
        private List<string> noTerminales;
        private string simboloInicial;

        private List<Produccion> primeros;
        private List<Produccion> siguientes;

        public string[,] tabla;

        public AnalizadorSintacticoLL1()
        {
            primeros = new List<Produccion>();
            siguientes = new List<Produccion>();
            terminales = new List<string>();
            noTerminales = new List<string>();

            IniciarProducciones();
            CalcularPrimeros();
            CalcularSiguientes();

            tabla = new string[noTerminales.Count + 1, terminales.Count + 2];

            InicializarTabla();
            RellenarTabla();
            MostrarMatriz();
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

        private void EliminarRecursividadIzquierda()
        {
            string a = "";
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
            foreach (Produccion produccion in listaProducciones)
            {
                string primerElementoLadoDerecho = produccion.GetLadoDerecho()[0].ToString();

                if (Pertenece(primerElementoLadoDerecho, terminales))
                {
                    AgregarLista(primeros, primerElementoLadoDerecho, produccion.GetLadoIzquierdo());
                    BuscarEpsilonPrimero(produccion.GetLadoIzquierdo());
                }
                else if (Pertenece(primerElementoLadoDerecho, noTerminales))
                {
                    CalcularPrimeroNoTerminal(produccion.GetLadoIzquierdo(), primerElementoLadoDerecho);
                }
            }

            Console.WriteLine("Primeros...");
            foreach (Produccion primero in primeros)
            {
                Console.WriteLine(primero.ToString());
            }
        }

        private void CalcularPrimeroNoTerminal(string ladoIzquierdo, string noTerminal)
        {
            foreach (Produccion produccion in listaProducciones)
            {
                if (produccion.GetLadoIzquierdo().Equals(noTerminal))
                {
                    if (Pertenece(produccion.GetLadoDerecho()[0].ToString(), terminales))
                    {
                        AgregarLista(primeros, produccion.GetLadoDerecho()[0].ToString(), ladoIzquierdo);
                    }
                    else if (Pertenece(produccion.GetLadoDerecho()[0].ToString(), noTerminales))
                    {
                        CalcularPrimeroNoTerminal(ladoIzquierdo, produccion.GetLadoDerecho()[0].ToString());
                    }
                }
            }
        }

        private void AgregarLista(List<Produccion> lista, string simbolo, string ladoIzquierdo)
        {
            foreach (Produccion produccion in lista)
            {
                if (produccion.GetLadoIzquierdo().Equals(ladoIzquierdo))
                {
                    string nuevoLadoDerecho = produccion.GetLadoDerecho();
                    if (!produccion.GetLadoDerecho().Equals(""))
                    {
                        nuevoLadoDerecho += ",";
                    }

                    produccion.SetLadoDerecho(nuevoLadoDerecho + simbolo);
                    break;
                }
            }
        }

        private void BuscarEpsilonPrimero(string ladoIzquierdo)
        {
            foreach (Produccion produccion in listaProducciones)
            {
                if (ladoIzquierdo.Equals(produccion.GetLadoIzquierdo()) && produccion.GetLadoDerecho().Equals("€"))
                {
                    AgregarLista(primeros, "€", ladoIzquierdo);
                    break;
                }
            }
        }

        private bool Pertenece(string elemento, List<string> lista)
        {
            foreach (string elementoLista in lista)
            {
                if (elemento.Equals(elementoLista))
                {
                    return true;
                }
            }

            return false;
        }

        private void CalcularSiguientes()
        {
            AgregarLista(siguientes, "$", simboloInicial);

            foreach (Produccion produccion in listaProducciones)
            {
                if (produccion.GetLadoDerecho().Length == 3)
                {
                    AgregarSiguienteS1(produccion.GetLadoDerecho()[1].ToString(), produccion.GetLadoDerecho()[2].ToString());

                    if (DerivaEnEpsilon(produccion.GetLadoDerecho()[2].ToString()))
                    {
                        AgregarSiguienteS2(produccion.GetLadoDerecho()[1].ToString(), produccion.GetLadoIzquierdo());
                    }
                }

                if (produccion.GetLadoDerecho().Length == 2)
                {
                    AgregarSiguienteS2(produccion.GetLadoDerecho()[1].ToString(), produccion.GetLadoIzquierdo());
                }
            }

            ReemplazarSiguientes();

            Console.WriteLine("Siguientes...");
            foreach (Produccion siguiente in siguientes)
            {
                Console.WriteLine(siguiente.ToString());
            }
        }
        
        private void ReemplazarSiguientes()
        {
            bool seguirReemplazando = false;
            do
            {
                seguirReemplazando = false;

                foreach (Produccion siguiente in siguientes)
                {
                    string[] listaSiguientes = siguiente.GetLadoDerecho().Split(',');
                    
                    for (int i = 0; i < listaSiguientes.Length; i++)
                    {
                        if (Regex.IsMatch(listaSiguientes[i], "S\\([A-Z]\\)"))
                        {
                            seguirReemplazando = true;

                            Produccion siguientes = BuscarSiguientes(listaSiguientes[i][2].ToString());

                            siguiente.SetLadoDerecho(siguiente.GetLadoDerecho().Replace(listaSiguientes[i], siguientes.GetLadoDerecho()));
                        }
                    }
                }

            } while (seguirReemplazando);
        }

        private void AgregarSiguienteS1(string izquierda, string derecha)
        {
            Produccion primerosDerecha = BuscarPrimeros(derecha);

            if (primerosDerecha != null)
            {
                string[] listaPrimeros = primerosDerecha.GetLadoDerecho().Split(',');

                for (int i = 0; i < listaPrimeros.Length; i++)
                {
                    if (!listaPrimeros[i].Equals("€"))
                    {
                        AgregarLista(siguientes, listaPrimeros[i], izquierda);
                    }
                }
            }
            else if (Pertenece(derecha, terminales))
            {
                if (!ExisteEn(BuscarSiguientes(izquierda).GetLadoDerecho().Split(','),derecha))
                {
                    AgregarLista(siguientes, derecha, izquierda);
                }
            }
        }

        private bool ExisteEn(string[] lista, string elemento)
        {
            for(int i = 0; i < lista.Length; i++)
            {
                if (elemento.Equals(lista[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private void AgregarSiguienteS2(string izquierda, string derecha)
        {
            AgregarLista(siguientes, "S(" + derecha + ")", izquierda);
        }

        private bool DerivaEnEpsilon(string noTerminal)
        {
            Produccion primerosNoTerminal = BuscarPrimeros(noTerminal);

            if (primerosNoTerminal != null)
            {
                string[] listaPrimeros = primerosNoTerminal.GetLadoDerecho().Split(',');

                for (int i = 0; i < listaPrimeros.Length; i++)
                {
                    if (listaPrimeros[i].Equals("€"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Produccion BuscarPrimeros(string noTerminal)
        {
            foreach (Produccion produccion in primeros)
            {
                if (produccion.GetLadoIzquierdo().Equals(noTerminal))
                {
                    return produccion;
                }
            }

            return null;
        }

        private Produccion BuscarSiguientes(string noTerminal)
        {
            foreach (Produccion produccion in siguientes)
            {
                if (produccion.GetLadoIzquierdo().Equals(noTerminal))
                {
                    return produccion;
                }
            }

            return null;
        }

        public void InicializarTabla()
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    tabla[i, j] = " ";
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

            foreach (Produccion produccionDePrimero in primeros)
            {
                string primero = produccionDePrimero.GetLadoDerecho();
                string noTermPrimeros = produccionDePrimero.GetLadoIzquierdo();
                prim.Add(primero);
                noTerminalesPrimeros.Add(noTermPrimeros);
            }

            foreach (Produccion produccionSiguiente in siguientes)
            {
                string siguiente = produccionSiguiente.GetLadoIzquierdo();
                string noTermSiguientes = produccionSiguiente.GetLadoDerecho();
                noTerminalesSiguientes.Add(siguiente);
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
            listaProducciones.Add(new Produccion("S", "(A)"));
            listaProducciones.Add(new Produccion("S", "€"));
            listaProducciones.Add(new Produccion("A", "TE"));
            listaProducciones.Add(new Produccion("E", "&TE"));
            listaProducciones.Add(new Produccion("E", "€"));
            listaProducciones.Add(new Produccion("T", "(A)"));
            listaProducciones.Add(new Produccion("T", "a"));
            listaProducciones.Add(new Produccion("T", "b"));
            listaProducciones.Add(new Produccion("T", "c"));
            
            terminales.Add("b");
            terminales.Add("a");
            terminales.Add("c");
            terminales.Add("&");
            terminales.Add("(");
            terminales.Add(")");
            
            noTerminales.Add("S");
            noTerminales.Add("A");
            noTerminales.Add("E");
            noTerminales.Add("T");

            simboloInicial = "S";
            
            foreach (string noTerminal in noTerminales)
            {
                primeros.Add(new Produccion(noTerminal, ""));
                siguientes.Add(new Produccion(noTerminal, ""));
            }
        }

        public bool Probarcadena(String[,] matriz, List<String> cadena, int fila, int longitud)
        {
            //inicia con el simbolo inicial modificar el simbolo inicial a probar
            List<String> cadenareglas = new List<string>();
            cadenareglas.Add("$");
            //simbolo inicial
            cadenareglas.Add(simboloInicial);

            bool seguir_camino = true;

            while (seguir_camino)
            {
                int count1 = 0;
                int count2 = 0;
                if (cadenareglas[cadenareglas.Count - 1] == cadena[cadena.Count - 1])
                {
                    cadenareglas.RemoveAt(cadenareglas.Count - 1);
                    cadena.RemoveAt(cadena.Count - 1);
                }

                if (("" + cadenareglas[cadenareglas.Count - 1]) != "$")
                {
                    for (int x = 1; x < matriz.GetLength(0); x++)
                    {
                        if (("" + cadenareglas[cadenareglas.Count - 1]) == matriz[x, 0])
                        {
                            count1 = x;
                        }
                    }
                    for (int x = 1; x < matriz.GetLength(1); x++)
                    {
                        if (("" + cadena[cadena.Count - 1]) == matriz[0, x])
                        {
                            count2 = x;
                        }
                    }
                    //Console.WriteLine(count1 + " " + count2);
                    
                    if (matriz[count1, count2] != " ")
                    {


                        if (matriz[count1, count2] == "€")
                        {
                            cadenareglas.RemoveAt(cadenareglas.Count - 1);

                        }
                        else
                        {
                            bool ver = true;
                            for (int y = 0; y < matriz.GetLength(1); y++)
                            {
                                if (matriz[0, y] == matriz[count1, count2])
                                {
                                    ver = false;
                                }
                            }
                            if (!ver)
                            {
                                cadenareglas.RemoveAt(cadenareglas.Count - 1);
                                cadenareglas.Add(matriz[count1, count2]);
                            }
                            else
                            {
                                cadenareglas.RemoveAt(cadenareglas.Count - 1);

                                for (int x = matriz[count1, count2].Length - 1; x >= 0; x--)
                                {
                                    cadenareglas.Add("" + matriz[count1, count2][x]);
                                }
                            }
                            if (cadenareglas[cadenareglas.Count - 1] == cadena[cadena.Count - 1])
                            {
                                cadenareglas.RemoveAt(cadenareglas.Count - 1);
                                cadena.RemoveAt(cadena.Count - 1);

                            }
                        }
                    }
                    else
                    {
                        seguir_camino = !seguir_camino;
                    }

                }
                else
                {
                    seguir_camino = !seguir_camino;
                }
            }

            if (cadenareglas[cadenareglas.Count - 1] == "$" && cadena[cadena.Count - 1] == "$")
            {
                return true;
            }

            else
            {
                Console.WriteLine("Error en '" + cadena[cadena.Count - 1] + "' fila: " + fila + " ," + (longitud - cadena.Count));
                return false;
            }
        }

        public List<String> Configurarcadena(String[] cadena)
		{
			List<String> cadenanueva = new List<string>();
			cadenanueva.Add("$");
			for (int x = cadena.Length - 1; x >= 0; x--)
			{
				cadenanueva.Add(cadena[x]);
			}
			return cadenanueva;
		}

	}
}
