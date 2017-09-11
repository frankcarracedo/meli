using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Meli.Web.Controllers
{
    public class MeliController : ApiController
    {
        protected const char ZERO = '0';
        protected const string GUION = "-";
        protected const char GUION_BAJO = '_';
        protected const string PUNTO = ".";

        [HttpPost]
        public string DecodeBits2Morse(string message)
        {
            // Declaro variables.
            List<string> zeroList = new List<string>();
            List<string> oneList = new List<string>();
            int aux = 0;

            // Recorro el mensaje.
            for (int i = 0; i < message.Length; i++)
            {
                if (i != 0)
                {
                    if (message[i] != message[i - 1])
                    {
                        if (message[i - 1] == '0')
                        {
                            // Agrego porcion de 0 a la lista
                            zeroList.Add(message.Substring(aux, (i - aux)));
                        }
                        else
                        {
                            // Agrego porcion de 1 a la lista
                            oneList.Add(message.Substring(aux, (i - aux)));
                        }

                        aux = i;
                    }
                }
            }

            // Ordeno las lists de menor a mayor por caracteres.
            zeroList = zeroList.OrderBy(x => x.Length).ToList();
            oneList = oneList.OrderBy(x => x.Length).ToList();

            return Decode(zeroList, oneList, message);
        }

        private string Decode(List<string> zeroList, List<string> oneList, string message)
        {
            // Declaro variables.
            List<int> countOne = new List<int>();
            List<int> countZero = new List<int>();
            int aux = 0;
            int suma = 0;
            int sumaOne = 0;
            int promedio = 0;
            int promedioOne = 0;
            string messageSubstring = string.Empty;

            // Recorro lista de 0s y obtengo todos los largos.
            for (int i = 0; i < oneList.Count; i++)
                countOne.Add(oneList[i].Length);

            // Recorro lista de 0s y obtengo todos los largos.
            for (int i = 0; i < zeroList.Count; i++)
                countZero.Add(zeroList[i].Length);

            // Recorro los largos para sumarlos
            for (int i = 0; i < countOne.Count; i++)
                sumaOne += countOne[i];

            // Recorro los largos para sumarlos
            for (int i = 0; i < countZero.Count; i++)
                suma += countZero[i];

            // Obtengo el promedio del largo de los 0s.
            promedio = (suma / countZero.Count);
            promedioOne = (sumaOne / countOne.Count);

            // Recorro el mensaje.
            for (int i = 0; i < message.Length; i++)
            {
                if (i != 0)
                {
                    if (message[i] != message[i - 1])
                    {
                        if (message[i - 1] != ZERO)
                        {
                            messageSubstring = message.Substring(aux, (i - aux));

                            var aStringBuilder = new StringBuilder(message);
                            aStringBuilder.Remove(aux, (i - aux));

                            if (messageSubstring.Length > promedioOne)
                                aStringBuilder.Insert(aux, GUION.PadRight(messageSubstring.Length, GUION_BAJO));
                            else
                                aStringBuilder.Insert(aux, PUNTO.PadRight(messageSubstring.Length, GUION_BAJO));

                            message = aStringBuilder.ToString();
                        }
                        else
                        {
                            messageSubstring = message.Substring(aux, (i - aux));

                            var aStringBuilder = new StringBuilder(message);
                            aStringBuilder.Remove(aux, (i - aux));

                            if (messageSubstring.Length > promedio)
                            {
                                aStringBuilder.Insert(aux, " ".PadRight(messageSubstring.Length, GUION_BAJO));
                            }
                            else
                            {
                                aStringBuilder.Insert(aux, string.Empty.PadRight(messageSubstring.Length, GUION_BAJO));
                            }

                            message = aStringBuilder.ToString();
                        }
                        aux = i;
                    }
                }
            }

            message = message.Replace("_", string.Empty).Trim(ZERO);

            return message;
        }

        [HttpPost]
        public string Translate2Human(string message)
        {
            // Declaro y asigno variables.
            string[] messageSplit = message.Split(' ');
            Dictionary<string, string> dictionaryMorse = DictionaryMorse();
            string messageFinal = string.Empty;

            for (int i = 0; i < messageSplit.Length; i++)
                messageFinal = messageFinal + dictionaryMorse[messageSplit[i]];

            return messageFinal;
        }

        private Dictionary<string, string> DictionaryMorse()
        {
            Dictionary<string, string> dictionaryMorse = new Dictionary<string, string>();

            dictionaryMorse.Add(".-", "A");
            dictionaryMorse.Add("-...", "B");
            dictionaryMorse.Add("-.-.", "C");
            dictionaryMorse.Add("-..", "D");
            dictionaryMorse.Add(".", "E");
            dictionaryMorse.Add("..-.", "F");
            dictionaryMorse.Add("--.", "G");
            dictionaryMorse.Add("....", "H");
            dictionaryMorse.Add("..", "I");
            dictionaryMorse.Add(".---", "J");
            dictionaryMorse.Add("-.-", "K");
            dictionaryMorse.Add(".-..", "L");
            dictionaryMorse.Add("--", "M");
            dictionaryMorse.Add("-.", "N");
            dictionaryMorse.Add("---", "O");
            dictionaryMorse.Add(".--.", "P");
            dictionaryMorse.Add("--.-", "Q");
            dictionaryMorse.Add(".-.", "R");
            dictionaryMorse.Add("...", "S");
            dictionaryMorse.Add("-", "T");
            dictionaryMorse.Add("..-", "U");
            dictionaryMorse.Add("...-", "V");
            dictionaryMorse.Add(".--", "W");
            dictionaryMorse.Add("-..-", "X");
            dictionaryMorse.Add("-.--", "Y");
            dictionaryMorse.Add("--..", "Z");

            dictionaryMorse.Add("-----", "0");
            dictionaryMorse.Add(".----", "1");
            dictionaryMorse.Add("..---", "2");
            dictionaryMorse.Add("...--", "3");
            dictionaryMorse.Add("....-", "4");
            dictionaryMorse.Add(".....", "5");
            dictionaryMorse.Add("-....", "6");
            dictionaryMorse.Add("--...", "7");
            dictionaryMorse.Add("---..", "8");
            dictionaryMorse.Add("----.", "9");

            dictionaryMorse.Add(".-.-.-", "FullStop");

            dictionaryMorse.Add("", " ");

            return dictionaryMorse;
        }
       
    }
}