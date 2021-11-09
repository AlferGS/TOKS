using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com2com
{
    public partial class CyclicCode: Form
    {
        private string message { get; set; }
        private char[] charByte { get; set; }
        private byte oneChar { get; set; }

        // перевод из string в масив char // из char перевод в dec // из dec перевод в binary
        public string StringToBin(string _message)
        {
            message = _message;
            string newMessage = "";
            for (int i = 0; i < message.Length; i++)
            {
                if(message[i] == ' ' || '0' <= message[i]) 
                {
                    if (message[i] <= '9')
                    {
                        newMessage += "0" + Convert.ToString(message[i], 2);
                        continue;
                    }
                }
                if(message[i] == '\r' || message[i] == '\n') { newMessage += "000" + Convert.ToString(message[i], 2); continue; }
                newMessage += Convert.ToString(message[i], 2);
            }
            return newMessage;
        }

        public string BinToString(string _message)
        {
            string newMessage = "";                         //строка для вывода
            int numOfBytes = _message.Length / 7;           //количество байтов (char)
            byte[] bytes = new byte[numOfBytes];            //массив битов
            for (int i = 0; i < numOfBytes; i++)
            {
                string oneBinaryByte = _message.Substring(7 * i, 7);    //извлекаем подстроку(один char)
                bytes[i] = Convert.ToByte(oneBinaryByte, 2);            //преобразуем bin в dec
                newMessage += Convert.ToChar(bytes[i]);                 //преобразуем dec в ASCII символ
            }
            return newMessage;
        }
    }
}
