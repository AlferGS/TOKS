﻿using System;
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
        private const char plusEsc = '#';
        private const char notEsc = '&';

        public string ByteStuffing(string message, RichTextBox Debug)
        {
            string tempString = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == Esc)              // Find esc symbols
                {
                    Debug.SelectionStart = Debug.Text.Length;
                    tempString += "" + Esc + notEsc;
                    Debug.SelectionColor = Color.Green;
                    Debug.AppendText("" + Esc + notEsc);
                    Debug.SelectionColor = Color.Black;
                }
                else if (message[i] == flag)        // Find flag symbols
                {
                    Debug.SelectionStart = Debug.Text.Length;
                    tempString += "" + Esc + plusEsc;
                    Debug.SelectionColor = Color.Red;
                    Debug.AppendText("" + Esc + plusEsc);
                    Debug.SelectionColor = Color.Black;
                }
                else
                {
                    tempString += message[i];
                    Debug.AppendText(message[i].ToString());
                }
            }
            return tempString;
        }
        public string UnByteStuffing(string message)
        {
            string tempString = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == Esc && message[i + 1] == plusEsc)    // Back flag
                {
                    i++;
                    tempString += flag;
                }
                else if (message[i] == Esc && message[i + 1] == notEsc)   //Find notEsc
                {
                    i++;
                    tempString += Esc;
                }
                else
                {
                    tempString += message[i];
                }
            }
            return tempString;
        }
    }
}