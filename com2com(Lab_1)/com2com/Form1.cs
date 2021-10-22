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
        private void SendButton_Click(object sender, EventArgs e) {
            if (portName != "Null") {
                try {
                    string writeLine = Convert.ToString(InputBox.Text);
                    comPort.WriteLine(writeLine);
                    InputBox.Text = "";
                    Debug.Text = "Send message";
                }
                catch (InvalidOperationException) { Debug.Text = portName + " is busy. Select another port"; }
                catch (TimeoutException) { Debug.Text = "Time for send a message is out";  }
            }
            else {
                Debug.Text = "Select Com port";
            }
        }
        private void InputBox_TextChanged(object sender, EventArgs e) {                 //Send message if click enter 
            if (InputBox.Text.Length > 0 && portName != "Null") {
                if (InputBox.Text[InputBox.Text.Length - 1] == '\n' && InputBox.Text.TrimEnd('\r', '\n') != "") {
                    try {
                        string message = Convert.ToString(InputBox.Text.TrimEnd('\r', '\n'));
                        comPort.WriteLine(message);
                        InputBox.Text = "";
                        Debug.Text = "Send message";
                    }
                    catch (InvalidOperationException) { Debug.Text = portName + " is busy. Select another port"; }
                    catch (TimeoutException) { Debug.Text = "Time for send a message is out"; }
                }
            }
            else if(portName == "Null") {
                Debug.Text = "Select Com port";
            }
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
                    catch (TimeoutException e)        { Debug.Invoke((MethodInvoker)delegate { Debug.Text = "TimeoutException - " + e.Message; }); }
                    catch (ObjectDisposedException e) { Debug.Invoke((MethodInvoker)delegate { Debug.Text = "ObjectDisposedException - " + e.Message; }); }
                    finally { canRead = false; }
                }
            }
        }
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
                        InputBox.Enabled = true;
                    }
                    catch (UnauthorizedAccessException) {
                        comPort.Close();
                        comPort = new SerialPort(tmp_name, 9600, Parity.None, 8, StopBits.One);
                        comPort.Encoding = Encoding.Unicode;
                        if (tmp_name != "Null") { comPort.Open(); }
                        Debug.Text = portName + " is busy. \nConnect to " + tmp_name;
                        GUIflag--;
                        ComboBox.SelectedItem = tmp_name;
                        if (tmp_name == "Null") { InputBox.Enabled = false; }
                        else { InputBox.Enabled = true; }
                    }
                    catch (IOException) { Debug.Text = "The port response time has expired. \nConnect to " + tmp_name; GUIflag--; ComboBox.SelectedItem = tmp_name; }     //fix
                    finally { mutex.ReleaseMutex(); comPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); }
                }
            }
            if(portName == "Null" && tmp_name == "Null") { GUIflag = 0; return; } 
            if(portName == "Null" && tmp_name != null) { comPort.Close(); GUIflag--; InputBox.Enabled = false; }
        }
        public void Com2com_FormClosing(object sender, FormClosingEventArgs e){
            readThread.Abort();
            if (comPort != null) { comPort.Close(); }
            System.Environment.Exit(0);
        }

        private void clrOutputButton_Click(object sender, EventArgs e){
            OutputBox.Items.Clear();
            Debug.Text = "The Output was cleared";
        }

        private void updatePortsButton_Click(object sender, EventArgs e){
            ComboBox.Items.Clear();
            ports = SerialPort.GetPortNames();
            ComboBox.Items.Add("Null");
            ComboBox.Items.AddRange(ports);
            GUIflag = 0;
            ComboBox.SelectedItem = "Null";
            Debug.Text = "Port list was updated";
        }
    }
}