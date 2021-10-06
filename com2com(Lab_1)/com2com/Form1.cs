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
using static System.Threading.Monitor;

namespace com2com
{
    public partial class com2com : Form
    {
        SerialPort comPort;
        Thread readThread;
        String portName;
        int GUIflag = 0;
        static bool QWE = false;
        static Mutex mutex = new Mutex();
        //static int[] readyFlags = new int[2] { 0, 0 };
        //static int turn = 1;
        static string[] ports = SerialPort.GetPortNames();
        public com2com()
        {
            InitializeComponent();
            this.FormClosing += Com2com_FormClosing;

            Debug.Text = "";
            for (int i = 0; i < ports.Length; i++){
                Debug.Text += "i = " + i + "  " + ports[i] + "\n";
                ComboBox.Items.Add(ports[i]);
            }

            for (int i = 0; i < ports.Length; i++){
                ComboBox.SelectedIndex = i;
                //readyFlags[0] = 0;
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
            comPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            readThread = new Thread(read);
            readThread.Start();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            QWE = true;
        }

        //private void read()
        //{
        //    while (true)
        //    {
        //        if (comPort.DataReceived)
        //        {
        //            serialPort_DataReceived
        //        }
        //    }
        //}

        private void read()
        {                                        // Thread for check new message in port
            //Thread.Sleep(3000);
            while (true)
            {
                //string readLine = "";
                //readyFlags[1] = 1;
                //if (readyFlags[0] == 0) { turn = 1; }
                //if (readyFlags[1] == 1 && turn == 1)
                //{
                if (QWE)
                {
                    Console.WriteLine("read start");
                    try
                    {
                        Console.WriteLine("read");
                        mutex.WaitOne();
                        OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Add(comPort.ReadLine()); });
                        mutex.ReleaseMutex();
                        //mutex.WaitOne();
                        //OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Add(comPort.ReadLine()); });

                        //readLine = comPort.ReadLine();
                        //readyFlags[1] = 0;
                        //OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Clear(); });
                        //OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Add(i.ToString()); });
                    }
                    catch (TimeoutException e) { Console.WriteLine("TimeoutException -... " + e.Message); }
                    catch (ObjectDisposedException) { Console.WriteLine("ObjectDisposedException \n"); }
                    finally
                    {
                        //mutex.ReleaseMutex();
                        //readyFlags[1] = 0;
                        QWE = false;
                        Console.WriteLine("read end");
                    }
                }
                //}
                else { Console.WriteLine("Wait changes"); Thread.Sleep(1000); }
                //turn = 0;
            }
        }

        //portName - хранит текущий порт, tmp_port - старый порт 
        //PortName сохраняет имя нового порта, отключаемся от старого порта
        //и пытемся подключится к новому. если не получается, то подключаемся к старому
        private void ComboBox_SelectedIndexChanged_1(object sender, EventArgs e){
            //readyFlags[0] = 1;
            GUIflag++;
            string tmp_name = portName;
            portName = ComboBox.SelectedItem.ToString();
            Thread.Sleep(300);
            if (comPort != null && GUIflag == 1)
            {
                GUIflag--;
                //while (true) {
                //    Console.WriteLine("{" + readyFlags[0] + ", " + readyFlags[1] + "}");
                //    if (turn == 0) {
                //        break;
                //    }
                //}
                try
                {
                    Console.WriteLine("change start");
                    mutex.WaitOne();
                    comPort.Close();
                    comPort = new SerialPort(portName);
                    comPort.Open();
                }
                catch (UnauthorizedAccessException)
                {
                    comPort.Close();
                    comPort = new SerialPort(tmp_name);
                    comPort.Open();
                    Debug.Text = "Com Port is Closed. \nConnect to " + tmp_name;
                    GUIflag--;
                    ComboBox.SelectedItem = tmp_name;
                }
                catch (IOException) { Debug.Text = "The port response time has expired. \nConnect to " + tmp_name; GUIflag--; ComboBox.SelectedItem = tmp_name;  }
                finally { mutex.ReleaseMutex(); Console.WriteLine("change end"); Thread.Sleep(300);/* readyFlags[0] = 0; turn = 1;*/ }
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
            //    try {
            //        while (true) {
            //            //if (portSem.WaitOne()){
            //            Lock = false;
            //            try
            //            {
            //                Monitor.TryEnter(locker, ref Lock);
            //                Console.WriteLine("change: sem Open");
            //                comPort.Close();
            //                comPort = new SerialPort(portName);
            //                comPort.Open();
            //                //flg = portSem.Release();
            //                //Console.WriteLine("flg = " + flg);
            //                Console.WriteLine("change: sem Close");
            //                break;
            //            }
            //            finally { if (Lock) Monitor.Exit(locker); }
            //        }
            //    } catch (UnauthorizedAccessException) {
            //        while (true) {
            //            try
            //            {
            //                comPort.Close();
            //                comPort = new SerialPort(tmp_name);
            //                comPort.Open();
            //                //flg = portSem.Release();
            //                //Console.WriteLine("flg = " + flg);
            //                Console.WriteLine("re!change: sem Close");
            //                Debug.Text = "Com Port is Closed. \nConnect to " + tmp_name;
            //                GUIflag--;
            //                ComboBox.SelectedItem = tmp_name;
            //                //portSem.Release();
            //            }
            //            finally { if (Lock) Monitor.Exit(locker); }
            //        break;
            //        }
            //    //catch (System.Threading.ThreadAbortException exception) {
            //    //      Console.WriteLine("Abort exception - " + exception.Message);}
            //    }
            //}