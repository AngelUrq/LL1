using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1
{
    class Produccion
    {

        private string ladoDerecho;
        private string ladoIzquierdo;

        public Produccion(string ladoIzquierdo, string ladoDerecho)
        {
            this.ladoIzquierdo = ladoIzquierdo;
            this.ladoDerecho = ladoDerecho;
        }

        public string GetLadoIzquierdo()
        {
            return ladoIzquierdo;
        }

        public void SetLadoDerecho(string ladoDerecho)
        {
            this.ladoDerecho = ladoDerecho;
        }

        public string GetLadoDerecho()
        {
            return ladoDerecho;
        }

        public override string ToString()
        {
            return ladoIzquierdo + "->" + ladoDerecho;
        }

    }
}
