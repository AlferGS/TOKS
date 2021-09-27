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
        Semaphore portSem = new Semaphore(1,2);
        int GUIflag = 0;
        public com2com()
        {
            InitializeComponent();
            this.FormClosing += Com2com_FormClosing;

            string[] ports = SerialPort.GetPortNames();
            Debug.Text = "";
            for (int i = 0; i < ports.Length; i++){
                Debug.Text += "i = " + i + "  " + ports[i] + "\n";
                ComboBox.Items.Add(ports[i]);
            }

            for (int i = 0; i < ports.Length; i++){
                ComboBox.SelectedIndex = i;
                GUIflag--;
                comPort = new SerialPort(ports[i]);
                if (comPort.IsOpen == false) {
                    try{
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
            while (true){
                        portSem.WaitOne();
                Console.WriteLine("..."+readThread.ThreadState.ToString()+ "...");
                try{
                    if (comPort.IsOpen){
                        string readLine = comPort.ReadLine();
                        OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Add(readLine); });
                    }
                }
                catch (TimeoutException e)       { Console.WriteLine("TimeoutException -... " + e.Message); }
                catch (ObjectDisposedException ) { Console.WriteLine("ObjectDisposedException \n"); }
                
                        portSem.Release();
                Thread.Sleep(1000);
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
            System.Environment.Exit(0);
            System.Environment.FailFast("Exit Error!");
        }

        //portName - хранит текущий порт, tmp_port - старый порт 
        //PortName сохраняет имя нового порта, отключаемся от старого порта
        //и пытемся подключится к новому. если не получается, то подключаемся к старому
        private void ComboBox_SelectedIndexChanged_1(object sender, EventArgs e){
            GUIflag++;
            string tmp_name = portName;
            portName = ComboBox.SelectedItem.ToString();
            if (comPort != null && GUIflag == 1){
                GUIflag--;
                try{
                    while (true){
                        if (portSem.WaitOne(1)){
                            comPort.Close();
                            comPort = new SerialPort(portName);
                            comPort.Open();
                            portSem.Release();
                            break;
                        }
                    }
                }catch (UnauthorizedAccessException){
                    while (true){
                        comPort.Close();
                        comPort = new SerialPort(tmp_name);
                        comPort.Open();
                        portSem.Release();
                        Debug.Text = "Com Port is Closed. \nConnect to " + tmp_name;
                        GUIflag--;
                        ComboBox.SelectedItem = tmp_name;
                        //portSem.Release();
                        break;
                    }
                //catch (System.Threading.ThreadAbortException exception) {
                //      Console.WriteLine("Abort exception - " + exception.Message);}
                }
            }
        }
    }
}