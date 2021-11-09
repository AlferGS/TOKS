using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace com2com
{
    public partial class Stuffing : Form
    {
        private const char flag = 'b';
        private const char Esc = '@';
        private const char rightEsc = '#';
        private const char notEsc = '&';

        public string ByteStuffing(string message, RichTextBox Debug)
        {
            string tmpString = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == Esc)              // Find esc symbols
                {
                    Debug.SelectionStart = Debug.Text.Length;
                    tmpString += "" + Esc + notEsc;
                    Debug.SelectionColor = Color.Green;
                    Debug.AppendText("" + Esc + notEsc);
                    Debug.SelectionColor = Color.Black;
                }
                else if (message[i] == flag)        // Find flag symbols
                {
                    Debug.SelectionStart = Debug.Text.Length;
                    tmpString += "" + Esc + rightEsc;
                    Debug.SelectionColor = Color.Red;
                    Debug.AppendText("" + Esc + rightEsc);
                    Debug.SelectionColor = Color.Black;
                }
                else
                {
                    tmpString += message[i];
                    Debug.AppendText(message[i].ToString());
                }
            }
            return tmpString;
        }
        public string DeByteStuffing(string message)
        {
            string tmpString = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == Esc && message[i + 1] == rightEsc)    // Back flag
                {
                    i++;
                    tmpString += flag;
                }
                else if (message[i] == Esc && message[i + 1] == notEsc)   //Find notEsc
                {
                    i++;
                    tmpString += Esc;
                }
                else
                {
                    tmpString += message[i];
                }
            }
            return tmpString;
        }
    }
}