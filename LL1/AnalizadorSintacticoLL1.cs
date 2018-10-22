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

		public AnalizadorSintacticoLL1()
        {
            IniciarProducciones();
        }

		

		public List<Produccion> Factorizar_izquierda(List<Produccion> reglas)
		{
			List<List<String>> produccionestotal = new List<List<String>>();
			List<String> producciones = new List<String>();

			bool ver = true;
			producciones.Add("♣");
			for (int x = 0; x < reglas.Count; x++)
			{
				List<String> datos = new List<string>();
				for (int y = 0; y < producciones.Count; y++)
				{
					if (reglas[x].GetLadoIzquierdo() == producciones[y])
					{
						ver = !ver;
					}
				}
				if (ver)
				{
					producciones.Add(reglas[x].GetLadoIzquierdo());
					for (int j = 0; j < reglas.Count; j++)
					{
						if (producciones[producciones.Count - 1] == reglas[j].GetLadoIzquierdo())
						{
							datos.Add(reglas[j].GetLadoDerecho());
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

		private void Cadena()
        {
            string cadena1 = "";
            for (int i = 0; i < listaProducciones.Count; i++)
            {
                cadena1 = cadena1 + listaProducciones[i].GetLadoIzquierdo().ToString() + "->" + listaProducciones[i].GetLadoDerecho().ToString() + "\n";
            }
            EliminarRecursividadIzquierda(cadena1);
        }
        string a = "";
        private void EliminarRecursividadIzquierda(string cadena1)
        {
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
            //Parte de Rodri
        }

        private void CalcularSiguientes()
        {
            //Parte de Diego
        }

        private void RellenarTabla()
        {
            //Parte de Adri y Ángel
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
        }

    }
}
