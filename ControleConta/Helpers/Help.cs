using BancoSA.Models;
using ControleConta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ControleConta.Helpers
{
    public class Help
    {

        private static ContaMatriz Cm { get; set; }
        private static ContaFilial Cf { get; set; }

        public static void ContaMatrix(ContaMatriz cmz)
        {
            Cm = cmz;
        }

        public static ContaMatriz ContaMatrix()
        {
            return Cm;
        }

        public static void ContaFilial(ContaFilial cf)
        {
            Cf = cf;
        }


        public static ContaFilial ContaFilial()
        {
            return Cf;
        }

        public static bool IsValidCPF(string cpf)
        {

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            try
            {
                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)
                    return false;

                tempCpf = cpf.Substring(0, 9);

                soma = 0;

                for (int i = 0; i < 9; ++i)

                 soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;

                for (int j = 0; j < 10; j++)
                    soma += int.Parse(tempCpf[j].ToString()) * multiplicador2[j];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();
                
                return cpf.EndsWith(digito);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            return true;
        }




        //Fonte:https://social.msdn.microsoft.com/Forums

        public static bool IsValid (string texto)
        {
            if (contemLetras(texto) && contemNumeros(texto))
            {

                return true;
            }
            else if (contemLetras(texto))
            {
                return false;
            }
            else if (contemNumeros(texto))
            {
                return false;
            }

            return false;
        }

        private static bool contemLetras(string texto)
        {
            if (texto.Where(c => char.IsLetter(c)).Count() > 0)
                return true;
            else
                return false;
        }

        private static bool contemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }

    }
}
