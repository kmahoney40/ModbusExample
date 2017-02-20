using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Net.Sockets;
using System.Threading;
using System.IO.Ports;
using System.Collections;
using System.Management;
using System.Diagnostics;
using Modbus_Example2.mbLib;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Modbus_Example2
{
    public partial class Form1 : Form
    {
        private List<MBBase> MBLst = new List<MBBase>();
        private int lastSent = -1;
        private USBSerialComms SerialComms;
        public System.Windows.Forms.ToolStripStatusLabel plcStatusLabel { get; set; }
        public System.Windows.Forms.TextBox messageReceived { get; set; }
        public System.Windows.Forms.TextBox messageSent { get; set; }
        public System.Windows.Forms.TextBox ReadBitValue { get; set; }
        public System.Windows.Forms.TextBox Read16Value { get; set; }
        public System.Windows.Forms.TextBox Read32Value { get; set; }
        public System.Windows.Forms.TextBox Read32FloatValue { get; set; }
        public System.Windows.Forms.TextBox Read32InFloatValue { get; set; }

        public Form1()
        {
            InitializeComponent();

            plcStatusLabel = this.PLCStatusLabel;
            messageReceived = this.MessageRecieved;
            messageSent = this.MessageSent;
            ReadBitValue = this.readBitValue;
            Read32FloatValue = this.read32FloatValue;
            Read32InFloatValue = this.read32InFloatValue;
            Read32Value = this.read32Value;
            Read16Value = this.read16Value;
            SerialComms = new USBSerialComms(this);

            Factory mbFactory = new Factory();

            var json = System.IO.File.ReadAllText(@"c:\users\kevin\desktop\modbus.json");

            var objects = JArray.Parse(json); // parse as array  
            foreach (JObject root in objects)
            {
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    var n = app.Key;

                    var appName = app.Key;
                    var type = (String)app.Value["Type"];
                    byte device = (byte)app.Value["Device"];
                    byte writeCmnd = (byte)app.Value["WriteCmnd"];
                    byte readCmnd = (byte)app.Value["ReadCmnd"];
                    byte addHi = (byte)app.Value["addHi"];
                    byte addLo = (byte)app.Value["addLo"];
                    ModbusObjInitializer init = new ModbusObjInitializer(type, device, writeCmnd, readCmnd, addHi, addLo);
                    MBLst.Add(mbFactory.GetModbusObj(init));
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SerialComms.CloseIfAnotherProcessIsUsingDevice();
        }

        SerialPort _serialPort = new SerialPort();
        private const int WM_DEVICECHANGE = 0x0219;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_DEVICECHANGE)
            {
                this.Invoke((MethodInvoker)delegate { SerialComms.checkConns(); });
            }
        }
        bool bInsideCheckConns = false;
        bool bCheckConnsCheckAgain = false;

        int iCommNum = -1;

        public class COMPortInfo
        {
            public String portName;
            public String friendlyName;
        }

        public delegate void delegateUpdateUSBEnabled(bool bEnabled);
        private void updateUSBEnabled(bool bEnabled)
        {
            if (bEnabled)
            {
                PLCStatusLabel.ForeColor = Color.Green;
                PLCStatusLabel.Text = "PLC Connected";
            }
            else
            {
                PLCStatusLabel.ForeColor = Color.Red;
                PLCStatusLabel.Text = "PLC Disconnected";
            }
        }

        UInt16 CalculateCRC(Byte dchar, UInt16 crc16)
        {
            UInt16 mask = (UInt16) (dchar & 0x00FF);
            crc16 = (UInt16)(crc16 ^ mask); 
            for (int i = 0; i < 8; i++)
            {
                if ((UInt16)(crc16 & 0x0001) == 1)
                {
                    mask = (UInt16)(crc16 / 2);
                    crc16 = (UInt16)(mask ^ 0xA001);
                }
                else 
                {
                    mask = (UInt16)(crc16 / 2);
                    crc16 = mask;
                }
            }
            return crc16;
        }
       
        public void WriteBitMessage(Int16 iValue)
        {
            var thisMbo = MBLst[0];
            var writeCmnd = thisMbo.GetWriteCmndMsg((byte)iValue);
            SerialComms.sendMessage(writeCmnd);
        }

        public void Write16BitMessage(int iValue)
        {
             var thisMbo = MBLst[2];
            var writeCmnd = thisMbo.GetWriteCmndMsg((Int16)iValue);
            SerialComms.sendMessage(writeCmnd);
        }

        private void WriteFloatMessage(float fValue)
        {
            var thisMbo = MBLst[1];
            var writeCmnd = thisMbo.GetWriteCmndMsg(fValue);
            SerialComms.sendMessage(writeCmnd);
        }   

        private void Write32BitMessage(int iValue)   
        {
            var thisMbo = MBLst[3];
            List<byte> writeCmnd = thisMbo.GetWriteCmndMsg(iValue);
            SerialComms.sendMessage(writeCmnd);
        }

        public void ReadBitMessage()
        {
            var thisMbo = MBLst[0];
            SerialComms.sendMessage(thisMbo.readCmndMsg);
        }
        public void Read16BitMessage()
        {
            var thisMbo = MBLst[2];
            var plcRequest = new PlcRequest(false, false);
            plcRequest.requestBytes = new List<byte>(thisMbo.readCmndMsg);
            SerialComms.sendMessage(plcRequest);
        }

        private void ReadUInt16_Click(object sender, EventArgs e)
        {

        }

        private void write0_Click(object sender, EventArgs e)
        {
            WriteBitMessage(0);
        }

        private void write1_Click(object sender, EventArgs e)
        {
            WriteBitMessage(1);
        }

        private void readBit_Click(object sender, EventArgs e)
        {
            ReadBitMessage();
        }

        private void write16_Click(object sender, EventArgs e)
        {
            Write16BitMessage((int)write16Value.Value);
        }

        private void read16_Click(object sender, EventArgs e)
        {
            Read16BitMessage();
        }

        private void writeFloat_Click(object sender, EventArgs e)
        {
            WriteFloatMessage((float)writeFloatValue.Value);
        }

        public void Read32BitMessage(PlcRequest plcRequest)
        {
            SerialComms.sendMessage(plcRequest);
        }
        public void ReadFloatMessage()
        {
            var thisMbo = MBLst[1];
            var plcRequest = new PlcRequest(true, false);
            plcRequest.requestBytes = new List<byte>(thisMbo.readCmndMsg);
            Read32BitMessage(plcRequest);
        }

        private void readInFloat_Click(object sender, EventArgs e)
        {
            var thisMob = MBLst[4];
            var plcRequst = new PlcRequest(true, true);
            plcRequst.requestBytes = new List<byte>(thisMob.readCmndMsg);
            Read32BitMessage(plcRequst);
        }

        private void write32_Click(object sender, EventArgs e)
        {
            Write32BitMessage((int)write32Value.Value);
        }

        private void read32_Click(object sender, EventArgs e)
        {
            var thisMbo = MBLst[3];
            var plcRequest = new PlcRequest(false, true);
            plcRequest.requestBytes = new List<byte>(thisMbo.readCmndMsg);
            Read32BitMessage(plcRequest);
        }

        private void readFloat_Click(object sender, EventArgs e)
        {
            ReadFloatMessage();
        }
    }
}
