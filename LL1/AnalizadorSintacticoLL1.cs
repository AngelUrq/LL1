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

        private void EliminarRecursividadIzquierda()
        {
            //Parte de Rubén
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
