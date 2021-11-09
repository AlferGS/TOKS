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
        private CyclicCode cyclicCode;
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
            cyclicCode = new CyclicCode();
        }
        private void SendButton_Click(object sender, EventArgs e) {
            if (portName != "Null") {
                if (InputBox.Text != "")
                {
                    try {
                        string writeLine = Convert.ToString(InputBox.Text);
                        writeLine += "\r\n";
                        comPort.Write(cyclicCode.StringToBin(writeLine));
                        InputBox.Text = "";
                        Debug.Text = "Message send";
                    }
                    catch (InvalidOperationException) { Debug.Text = portName + " is busy. Select another port"; }
                    catch (TimeoutException) { Debug.Text = "Send time exceeded"; }
                }
            }
            else {
                Debug.Text = "Select Com port";
            }
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (InputBox.Text.Length > 0 && portName != "Null")
            {
                if (e.KeyData == Keys.Enter && e.KeyData != Keys.Shift && InputBox.Text.TrimEnd('\r', '\n') != "")
                {
                    try
                    {
                        string message = Convert.ToString(InputBox.Text.TrimEnd('\r', '\n'));
                        message += "\r\n";
                        comPort.Write(cyclicCode.StringToBin(message));
                        Debug.Text = "Message send";
                        InputBox.Clear();
                    }
                    catch (InvalidOperationException) { Debug.Text = portName + " is busy. Select another port"; }
                    catch (TimeoutException) { Debug.Text = "Send time exceeded"; }
                    e.SuppressKeyPress = true;
                }
                if (InputBox.Text == "\r\n") { e.SuppressKeyPress = true; InputBox.Clear(); }
            }
            if (InputBox.Text == "" && e.KeyData == Keys.Enter) { e.SuppressKeyPress = true; }
            if (e.KeyData == (Keys.Shift | Keys.Enter))
            {
                if (InputBox.Text == "") { e.SuppressKeyPress = true; }
            }
            else if (portName == "Null")
            {
                Debug.Text = "Select Com port";
            }
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {        //if received messasge in port, signal to read it
            canRead = true;
            Debug.Invoke((MethodInvoker)delegate { Debug.Text = "Message received"; });
        }

        private void read() {                                        // Thread for check new message in port
            while (true) {
                if (canRead) {
                    try {
                        mutex.WaitOne();
                        OutputBox.Invoke((MethodInvoker)delegate { OutputBox.Text += cyclicCode.BinToString(comPort.ReadExisting()) + "\n"; });
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
                        Debug.Text = portName + " selected";
                        InputBox.Enabled = true;
                        CheckDataInPort();
                    }
                    catch (UnauthorizedAccessException) {
                        comPort.Close();
                        comPort = new SerialPort(tmp_name, 9600, Parity.None, 8, StopBits.One);
                        comPort.Encoding = Encoding.Unicode;
                        Debug.Text = portName + " is busy.";
                        if (tmp_name != "Null") { comPort.Open(); 
                        Debug.Text += "\nConnect to " + tmp_name; }
                        GUIflag--;
                        ComboBox.SelectedItem = tmp_name;
                        InputBox.Enabled = (tmp_name == "Null") ? false : true;
                    }
                    catch (IOException) { Debug.Text = "Port response time expired. \nConnect to " + tmp_name; GUIflag--; ComboBox.SelectedItem = tmp_name; }     
                    finally { mutex.ReleaseMutex(); comPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); }
                }
            }
            if(portName == "Null" && tmp_name == "Null") { GUIflag = 0; return; } 
            if(portName == "Null" && tmp_name != null) { comPort.Close(); GUIflag--; InputBox.Enabled = false; }
        }
        public void CheckDataInPort()
        {
            try
            {
                if (comPort.BytesToRead != 0) { comPort.ReadExisting(); }
            }
            catch (TimeoutException e) { Debug.Invoke((MethodInvoker)delegate { Debug.Text = "TimeoutException - " + e.Message; }); }
            catch (ObjectDisposedException e) { Debug.Invoke((MethodInvoker)delegate { Debug.Text = "ObjectDisposedException - " + e.Message; }); }
        }
        public void Com2com_FormClosing(object sender, FormClosingEventArgs e){
            readThread.Abort();
            if (comPort != null) { comPort.Close(); }
            System.Environment.Exit(0);
        }

        private void clrOutputButton_Click(object sender, EventArgs e){
            OutputBox.Text = "";
            Debug.Text = "Output cleared";
        }

        private void updatePortsButton_Click(object sender, EventArgs e){
            ComboBox.Items.Clear();
            ports = SerialPort.GetPortNames();
            ComboBox.Items.Add("Null");
            ComboBox.Items.AddRange(ports);
            GUIflag = 0;
            ComboBox.SelectedItem = "Null";
            Debug.Text = "Port list updated";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newMessage = cyclicCode.StringToBin(InputBox.Text);
            Console.WriteLine("Decode this string");
            Console.WriteLine("Result - " + cyclicCode.BinToString(newMessage));
        }
    }
}