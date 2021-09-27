using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Threading;

namespace com2com
{
    public partial class com2com : Form
    {

        SerialPort comPort;
        Thread readThread;
        String portName;
        bool flg = true;
        public com2com()
        {
            InitializeComponent();
            this.FormClosing += Com2com_FormClosing;

            string[] ports = SerialPort.GetPortNames();
            Debug.Text = "";
            for (int i = 0; i < ports.Length; i++)                   // Add Ports names in ComboBox
            {
                Debug.Text += "i = " + i + "  " + ports[i] + "\n";
                ComboBox.Items.Add(ports[i]);
            }
            for (int i = 0; i < ports.Length; i++)                  //Choose first port 
            {
                ComboBox.SelectedIndex = i;
                comPort = new SerialPort(ports[i]);
                if (comPort.IsOpen == false)
                {
                    try
                    {
                        portName = ComboBox.SelectedItem.ToString();
                        comPort.Open();
                    }catch (UnauthorizedAccessException) { continue; }
                    i = ports.Length + 1;
                    break;
                }
            }
            comPort.ReadTimeout = 1000;
            readThread = new Thread(read);
            readThread.Start();
        }

        private void read(){                                        // Thread for check new message in port
            while (readThread.ThreadState.ToString() != "WaitSleepJoin" && readThread.ThreadState.ToString() != "Aborted")
            {
                Console.WriteLine("..."+readThread.ThreadState.ToString()+ "...");
                try{
                    if (comPort.IsOpen){
                        Console.WriteLine("comport is open\n");
                        string portname = comPort.PortName;
                        string readLine = comPort.ReadLine();
                        OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Add(readLine); });
                    }
                }
                catch (TimeoutException e) { Console.WriteLine("TimeoutException -... " + e.Message); }
                catch (ObjectDisposedException ) {
                    //System.Threading.Timer t = new System.Threading.Timer(null, null, 1000, 1);
                    Console.WriteLine("ObjectDisposedException \n");
                    Thread.Sleep(1000);
                    flg = false;
                }
                //catch (InvalidOperationException IOE) { Console.WriteLine("InvalidOperationException (Port closed) - " + IOE.Message); }
                
                while(flg == false){
                    Console.WriteLine("flg == false\n");
                    Thread.Sleep(100000);
                    flg = true;
                }
            }
        }
        private void SendButton_Click(object sender, EventArgs e){
            string writeLine = Convert.ToString(InputBox.Text);
            comPort.WriteLine(writeLine);
            InputBox.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e){
        }

        private void OutputBox_SelectedIndexChanged(object sender, EventArgs e){
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e){
        }

        public void Com2com_FormClosing(object sender, FormClosingEventArgs e){
            readThread.Abort();
            comPort.Close();
            //Application.Exit();
            System.Environment.Exit(0);
            System.Environment.FailFast("Exit Error!");
        }

        //portName - хранит текущий порт, tmp_port - старый порт 
        //PortName сохраняет имя нового порта, отключаемся от старого порта
        //и пытемся подключится к новому. если не получается, то подключаемся к старому
        private void ComboBox_SelectedIndexChanged_1(object sender, EventArgs e){
            string tmp_name = portName;
            portName = ComboBox.SelectedItem.ToString();
            if (comPort != null){
                try
                {
                    Console.WriteLine(readThread.ThreadState.ToString());
                    readThread.Abort();
                    Console.WriteLine(readThread.ThreadState.ToString());
                    //flg = false;
                    Thread.Sleep(10000);

                    readThread.Interrupt();
                    Console.WriteLine(readThread.IsAlive.ToString());

                    comPort.Close();
                    comPort = new SerialPort(portName);
                    comPort.Open();
                    //readThread.Abort();
                    readThread = new Thread(read);
                    Console.WriteLine(readThread.ThreadState.ToString());
                    readThread.Start();
                    Console.WriteLine(readThread.ThreadState.ToString());
                    //flg = true;

                }
                catch (System.Threading.ThreadAbortException exception) {
                    Console.WriteLine("Abort exception - " + exception.Message);
                }
                catch (UnauthorizedAccessException)
                {
                    comPort.Close();
                    comPort = new SerialPort(tmp_name);
                    comPort.Open();
                    //readThread = new Thread(read);
                    readThread.Start();
                    ComboBox.SelectedItem = tmp_name;
                    Debug.Text = "Com Port is Closed. \nConnect to " + tmp_name;
                }
            }
        }
    }
}