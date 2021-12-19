using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;

namespace com2com
{
    public partial class CSMA: Form {
        public List<char> messageCharList = new List<char>();
        private int counterOfCollisions;

        public void StringToCharArray(string _message) {
            messageCharList.Clear();
            messageCharList.AddRange(_message);
        }

        private bool ChannelIsBusy() {
            System.Random rnd = new System.Random();
            if (rnd.Next(0, 100) < 30)
                return true;
            else
                return false;
        }

        private bool IsCollision()
        {
            System.Random rnd = new System.Random();
            if (rnd.Next(0, 100) < 30)
                return true;
            else
                return false;
        }

        private void CollisionWindow() {
            System.Threading.Thread.Sleep(10); 
        }

        private void RandomDelay() {
            System.Random rnd = new System.Random();
            System.Threading.Thread.Sleep(rnd.Next(30,200));
        }

        public void SendMessage(SerialPort comPort, RichTextBox Debug) {
            counterOfCollisions = 0;
            while (messageCharList.Count > 0) {
                if (!ChannelIsBusy()) {
                    comPort.Write(messageCharList[0].ToString());
                    CollisionWindow();
                    if (!IsCollision()) {
                        if (messageCharList[0] != '\r' && messageCharList[0] != '\n') {
                            Debug.Text += "\n" + messageCharList[0] + ": " + new string('#', counterOfCollisions);
                        }
                        messageCharList.RemoveAt(0);
                        counterOfCollisions = 0;
                        continue;
                    } else {
                        counterOfCollisions++;
                        if(counterOfCollisions >= 10) {
                            messageCharList.RemoveAt(0);
                            counterOfCollisions = 0;
                        } else {
                            RandomDelay();
                            continue;
                        }
                    }
                } else {
                    continue;
                }
            }
        }
    }
}
