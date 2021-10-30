using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace com2com
{
    public partial class Stuffing: Form
    {
        private const string flag = "b";
        private const string esc  = "#";
        private const string changedEsc = "&";

        public string ByteStuffing(string message, RichTextBox Debug)
        {
            message = message.Replace(esc, changedEsc);
            message = message.Replace(flag, "@" + esc);
            for (int index = 0; index < message.Length; index++)
            {
                string output = "";
                if ((message[index] == '@') && (message[index + 1] == esc[0]))
                {
                    output = "@"+ esc;
                    Debug.SelectionColor = Color.Red;
                    Debug.AppendText(output);
                    Debug.SelectionColor = Color.Black;
                    output = "";
                    index++;
                }
                else
                {
                    if ((message[index] == changedEsc[0]))
                    {
                        output = changedEsc;
                        Debug.SelectionColor = Color.Green;
                        Debug.AppendText(output);
                        Debug.SelectionColor = Color.Black;
                        output = "";
                        index++;
                    }
                    else
                    {
                        output = "";
                        output += message[index];
                        Debug.AppendText(output);
                    }
                }
            }
            return message;
        }
        public string UnByteStuffing(string message)
        {
            message = message.Replace("@" + esc, flag);
            message = message.Replace(changedEsc, esc);
            return message;
        }
    }
}
