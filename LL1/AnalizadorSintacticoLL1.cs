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

        public AnalizadorSintacticoLL1()
        {
            IniciarProducciones();
        }

        private void FactorizarIzquierda()
        {
            //Parte de Jhon
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
