using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1
{
    class Clase_Separador
    {
        public ArrayList nombreRegla = new ArrayList();
        public ArrayList resultado = new ArrayList();

        ArrayList nom = new ArrayList();
        public ArrayList alfa = new ArrayList();
        public ArrayList beta = new ArrayList();

        public void Separar(string cadena)
        {
            string a1 = cadena.Replace("->", " ").Replace("\n", " ");
            char[] delimitador = { ' ' };
            string[] arr = a1.Split(delimitador, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < arr.Length; i++)
            {
                nombreRegla.Add(arr.GetValue(i));
                resultado.Add(arr.GetValue(i + 1));
                Array_sep(arr.GetValue(i).ToString(), arr.GetValue(i + 1).ToString());
                i++;
            }

        }

        private void Array_sep(string a, string aa)
        {
            if (nom.Contains(a))
            {
                Arraymult(a, aa);
            }
            else
            {
                nom.Add(a);
                Arraymult(a, aa);
                //MessageBox.Show(a);
            }
        }

        private void Arraymult(string a, string aa)
        {
            if (a.Length <= aa.Length)
            {
                if (a.Equals(aa.Substring(0, a.Length).ToString()) && !alfa.Contains(a + "\'"))
                {
                    alfa.Add(a + "\'");
                    beta.Add(aa = aa.Substring(a.Length) + a + "\'");
                    alfa.Add(a + "\'");
                    beta.Add((char)4);
                }
                else
                if (a.Equals(aa.Substring(0, a.Length).ToString()))
                {
                    alfa.Add(a + "\'");
                    beta.Add(aa = aa.Substring(a.Length) + a + "\'");
                }
                else
                if (alfa.Contains(a + "\'"))
                {
                    alfa.Add(a);
                    beta.Add(aa + a + "\'");
                }
            }
            else
                if (alfa.Contains(a + "\'"))
            {
                alfa.Add(a);
                beta.Add(aa + a + "\'");
            }
        }
    }
}
