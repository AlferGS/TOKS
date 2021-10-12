using System;
using System.Text;
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
        int GUIflag = 0;
        static bool canRead = false;
        static Mutex mutex = new Mutex();
        static string[] ports = SerialPort.GetPortNames();
        public com2com() {
            InitializeComponent();
            this.FormClosing += Com2com_FormClosing;

            Debug.Text = "";                                            //Add ports in ComboBox
            ComboBox.Items.Add("Null");
            ComboBox.Items.AddRange(ports);
            ComboBox.SelectedItem = "Null";
            GUIflag--;
            readThread = new Thread(read);
            readThread.Start();
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {        //if received messasge in port, signal to read it
            canRead = true;
            Debug.Text = "Got message";
        }

        private void read() {                                        // Thread for check new message in port
            while (true) {
                if (canRead) {
                    try {
                        mutex.WaitOne();
                        OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Items.Add(comPort.ReadLine()); });
                        mutex.ReleaseMutex();
                    }
                    catch (TimeoutException e)      { Console.WriteLine("TimeoutException -... " + e.Message); }
                    catch (ObjectDisposedException) { Console.WriteLine("ObjectDisposedException \n"); }
                    finally { canRead = false; }
                }
                else { Thread.Sleep(1000); }
            }
        }

        //portName - хранит текущий порт, tmp_port - старый порт 
        //PortName сохраняет имя нового порта, отключаемся от старого порта
        //и пытемся подключится к новому. если не получается, то подключаемся к старому
        private void ComboBox_SelectedIndexChanged_1(object sender, EventArgs e) {                      
            GUIflag++;
            string tmp_name = portName;
            portName = ComboBox.SelectedItem.ToString();
            if (portName != "Null") {
                if (GUIflag == 1) {
                    GUIflag--;
                    try {
                        mutex.WaitOne();
                        if (comPort != null) { comPort.Close(); }
                        comPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                        comPort.Encoding = Encoding.Unicode;
                        comPort.Open();
                        Debug.Text = "Select " + portName;
                    }
                    catch (UnauthorizedAccessException) {
                        comPort.Close();
                        comPort = new SerialPort(tmp_name, 9600, Parity.None, 8, StopBits.One);
                        comPort.Encoding = Encoding.Unicode;
                        comPort.Open();
                        Debug.Text = "The Com Port is Closed. \nConnect to " + tmp_name;
                        GUIflag--;
                        ComboBox.SelectedItem = tmp_name;
                    }
                    catch (IOException) { Debug.Text = "The port response time has expired. \nConnect to " + tmp_name; GUIflag--; ComboBox.SelectedItem = tmp_name; }
                    finally { mutex.ReleaseMutex(); comPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); }
                }
            }
            if(portName == "Null" && tmp_name != null) { comPort.Close(); }
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if (portName != "Null") {
                try {
                    string writeLine = Convert.ToString(InputBox.Text);
                    comPort.WriteLine(writeLine);
                    InputBox.Text = "";
                    Debug.Text = "Send message";
                }
                catch (TimeoutException) { Debug.Text = "Time for send is out";  }
                catch (InvalidOperationException) { Debug.Text = "Port is Closed. Select another port"; }
            }
            else {
                Debug.Text = "Select Com port";
            }
        }

        public void Com2com_FormClosing(object sender, FormClosingEventArgs e){
            readThread.Abort();
            if (comPort != null) { comPort.Close(); }
            System.Environment.Exit(0);
            System.Environment.FailFast("Exit Error!");
        }

        private void InputBox_TextChanged(object sender, EventArgs e) {                 //Send message if click enter 
            if (InputBox.Text.Length > 0 && portName != "Null") {
                if (InputBox.Text[InputBox.Text.Length - 1] == '\n' && InputBox.Text.TrimEnd('\r', '\n') != "") {
                    try {
                        string message = Convert.ToString(InputBox.Text.TrimEnd('\r', '\n'));
                        comPort.WriteLine(message);
                        InputBox.Text = null;
                        Debug.Text = "Send message";
                    }
                    catch (TimeoutException) { Debug.Text = "Time for send is out"; }
                    catch (InvalidOperationException) { Debug.Text = "Port is Closed. Select another port"; }
                }
            }
            else if(portName == "Null") {
                Debug.Text = "Select Com port";
            }
        }
    }
}