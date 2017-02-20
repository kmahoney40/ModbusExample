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

        public Form1()
        {
            InitializeComponent();

            plcStatusLabel = this.PLCStatusLabel;
            messageReceived = this.MessageRecieved;
            messageSent = this.MessageSent;
            ReadBitValue = this.readBitValue;
            Read32FloatValue = this.read32FloatValue;
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

        //private void CloseIfAnotherProcessIsUsingDevice()
        //{
        //    Process[] pname = Process.GetProcessesByName("vFactory");
        //    Process[] pnameViewer = Process.GetProcessesByName("vFactory Viewer");
        //    Process[] pnameBuilder = Process.GetProcessesByName("vBuilder");

        //    if (pname.Length != 0)
        //    {
        //        MessageBox.Show("This and vFactory can't run at the same time.  \nClose vFactory or vFactory Viewer before runing This.");
        //        this.Close();
        //    }
        //    else if (pnameViewer.Length != 0)
        //    {
        //        MessageBox.Show("This and vFactory Viewer can't run at the same time.  \nClose vFactory Viewer before runing This.");
        //        this.Close();
        //    }
        //    else if (pnameBuilder.Length != 0)
        //    {
        //        MessageBox.Show("This and vBuilder can't run at the same time.  \nClose vBuilder before runing This.");
        //        this.Close();
        //    }
        //    else
        //    {
        //        //this.Invoke((MethodInvoker)delegate { checkConns(); });
        //        this.Invoke((MethodInvoker)delegate { SerialComms.checkConns(); });
        //    }
        //}

        SerialPort _serialPort = new SerialPort();
        private const int WM_DEVICECHANGE = 0x0219;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_DEVICECHANGE)
            {
                //this.Invoke((MethodInvoker)delegate { checkConns(); });
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
        static string[] GetUSBCOMDevices()
        {

            List<String> list = new List<String>();
            try
            {
                ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject mo2 in searcher2.Get())
                {
                    string name = mo2["Name"].ToString();
                    // Name will have a substring like "(COM12)" in it.
                    if (name.Contains("(COM"))
                    {
                        list.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore this exception
            }

            string[] usbDevices = list.Distinct().OrderBy(s => s).ToArray();
            return usbDevices;
        }

        private void checkConns()
        {
            if (!bInsideCheckConns)
            {
                bInsideCheckConns = true;
                while ((bCheckConnsCheckAgain) || (bInsideCheckConns))
                {
                    try
                    {
                        if (_serialPort.IsOpen)
                        {
                            _serialPort.Close(); //doing this early on to reduce likelyhood of trying to write to closed port which throws unrecoverable error
                            _serialPort.Dispose();
                        }
                    }
                    catch
                    {
                        //unplugging USB Serial ports (which is what our PLCs act like) will cause an exception.  This try/catch lets the program continue after an exception.
                    }
                    try
                    {

                        String[] portNames = SerialPort.GetPortNames();
                        ArrayList alCommPortInfo = new ArrayList();
                        foreach (String s in portNames)
                        {
                            // s is like "COM14"
                            COMPortInfo ci = new COMPortInfo();
                            ci.portName = s;
                            ci.friendlyName = s;
                            alCommPortInfo.Add(ci);
                        }
                        String[] usbDevs = USBSerialComms.GetUSBCOMDevices();


                        foreach (String s in usbDevs)
                        {
                            // Name will be like "USB Bridge (COM14)"
                            int start = s.IndexOf("(COM") + 1;
                            if (start >= 0)
                            {
                                int end = s.IndexOf(")", start + 3);
                                if (end >= 0)
                                {
                                    // cname is like "COM14"
                                    String cname = s.Substring(start, end - start);
                                    for (int i = 0; i < alCommPortInfo.Count; i++)
                                    {
                                        if (((COMPortInfo)alCommPortInfo[i]).portName == cname)
                                        {
                                            ((COMPortInfo)alCommPortInfo[i]).friendlyName = s;
                                        }
                                    }
                                }
                            }
                        }

                        int iCommNumTemp = -1;
                        for (int i = 0; i < alCommPortInfo.Count; i++)
                        {
                            COMPortInfo myPort = (COMPortInfo)alCommPortInfo[i];
                            if (myPort.friendlyName.Contains("VelocioComm"))
                            {
                                String sCommNum = myPort.portName;
                                sCommNum = sCommNum.Remove(0, 3);
                                iCommNumTemp = Int32.Parse(sCommNum);
                                break;
                            }
                        }

                        if ((iCommNumTemp != iCommNum) ||
                            ((iCommNumTemp == -1) && (bDevicePluggedIn == true)) ||
                            ((iCommNumTemp != -1) && (bDevicePluggedIn == false)))
                        {

                            if (iCommNumTemp == -1)
                            {
                                Invoke(new delegateUpdateUSBEnabled(updateUSBEnabled), false);
                            }
                            else
                            {
                                Invoke(new delegateUpdateUSBEnabled(updateUSBEnabled), true);
                                _serialPort = new SerialPort("COM" + iCommNumTemp.ToString(), 115200, Parity.None, 8, StopBits.One);
                                _serialPort.Handshake = Handshake.None;
                                if (!_serialPort.IsOpen)
                                {
                                    _serialPort.Open();
                                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                                    iCommNum = iCommNumTemp;
                                }
                            }
                        }
                    }
                    catch
                    {
                        //unplugging USB Serial ports (which is what our PLCs act like) will cause an exception.  This try/catch lets the program continue after an exception.
                        Invoke(new delegateUpdateUSBEnabled(updateUSBEnabled), false);
                    }
                    if (bCheckConnsCheckAgain)
                    {
                        bCheckConnsCheckAgain = false;
                    }
                    else
                    {
                        bInsideCheckConns = false;
                    }
                }
            }
            else
            {
                bCheckConnsCheckAgain = true;
            }
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

        bool bDevicePluggedIn = false;
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] readValBytes = new byte[((SerialPort)sender).BytesToRead];
                int iResult = ((SerialPort)sender).Read(readValBytes, 0, ((SerialPort)sender).BytesToRead);

                this.BeginInvoke(new SetTextDeleg(si_DataReceivedEventArgs), new object[] { readValBytes, ((SerialPort)sender) });
            }
            catch
            {
            }
        }
        private delegate void SetTextDeleg(byte[] readVal, SerialPort inputPort);

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

        private void si_DataReceivedEventArgs(byte[] readVal, SerialPort inputPort)
        {
            if (readVal.Count() > 0)
            {
                String sMessageRecieved = "";
                for (int i = 0; i < readVal.Count(); i++)
                {
                    sMessageRecieved += readVal[i].ToString("X").PadLeft(2, '0') + " ";
                }


                //this checks if the CRC passes
                bool bCRCPasses = false;
                UInt16 crc16 = 0xFFFF;
                for (int i = 0; i < readVal.Count(); i++ )
                {
                    crc16 = CalculateCRC((Byte)readVal[i], crc16);
                }
                if (crc16 == 0)
                    bCRCPasses = true;  // In this example, we're not doing anything with this result, but you could make sure it passes before using the data


                /*
                 * Add an xml or JSON file to declare addresses and data types
                 * */


                MessageRecieved.Text = sMessageRecieved;
                switch (readVal[1])
                {
                    case 0x01: // Read Bit Command (Modbus Read Coils Command)
                    case 0x02: // (Modbus Read Discrete Inputs)
                        lastSent = -1;
                        int iMessageLength = readVal[2]; // Byte Count (# of data bytes)
                        UInt16 messageValue = (UInt16)(readVal[3]); // the bit value(s).  In our case we're only reading 1 bit.
                        readBitValue.Text = messageValue.ToString();

                        break;
                    case 0x03: // Read 16Bit Command (Modbus Read Holding Registers)
                        iMessageLength = readVal[2]; // Byte Count (# of data bytes)
                        UInt16 val = (UInt16)(readVal[4] + readVal[3] * 0x100);
                        read16Value.Text = val.ToString();
                        break;
                    case 0x04: // (Modbus Read Input Registers)
                        lastSent = -1;
                        byte[] myFloatArray = new byte[4];
                        float myFloat = System.BitConverter.ToSingle(myFloatArray, 0);
                        myFloatArray[2] = readVal[4];
                        myFloatArray[3] = readVal[3];
                        myFloatArray[0] = readVal[6];
                        myFloatArray[1] = readVal[5];
                        myFloat = System.BitConverter.ToSingle(myFloatArray, 0);
                        read32FloatValue.Text = myFloat.ToString();


                        iMessageLength = readVal[2]; // Byte Count (# of data bytes)
                        UInt32 val1 = (UInt32)(readVal[4] + readVal[3] * 0x100) * 65536;
                        UInt32 val2 = (UInt32)(readVal[6] + readVal[5] * 0x100);
                        UInt32 valToDisplay = val1 + val2;
                        read32Value.Text = valToDisplay.ToString(); 
                        break;
                    //case 0x04: // (Modbus Read Input Registers)
                    //     byte[] myI32Array = new byte[4];
                    //     myI32Array[0] = readVal[4];
                    //     myI32Array[1] = readVal[3];
                    //     myI32Array[2] = readVal[6];
                    //     myI32Array[3] = readVal[5];
                    //     Int32 myI32 = System.BitConverter.ToInt32(myI32Array, 0);
                    //     break;
                    case 0x05:    // (Modbus Write Single Coil)
                        break;

                    case 0x16:    
                        //byte[] myFloatArray = new byte[4];
                        //myFloatArray[0] = readVal[4];
                        //myFloatArray[1] = readVal[3];
                        //myFloatArray[2] = readVal[6];
                        //myFloatArray[3] = readVal[5];
                        //float myFloat = System.BitConverter.ToSingle(myFloatArray, 0);                        
                        break;
                    case 0x17: // (Modbus Read/Write Multiple Registers)
                        break;
                }
            }
        }
        public void sendMessage(ArrayList alToSend)
        {
            if (_serialPort != null)
            {
                if (!_serialPort.IsOpen)
                {
                    checkConns();
                    //SerialComms.checkConns();
                }
                if (alToSend.Count > 0)
                {
                    byte[] bytesToSend = new byte[alToSend.Count+2]; // the 2 is for the CRC we'll add at the end
                    String sMessageSent = "";
                    UInt16 crc16 = 0xFFFF;
                    for (int i = 0; i < alToSend.Count; i++)
                    {
                        Byte byteFromArray = (Byte)alToSend[i];
                        bytesToSend[i] = byteFromArray;
                        crc16 = CalculateCRC(byteFromArray, crc16);
                        sMessageSent += bytesToSend[i].ToString("X").PadLeft(2, '0') + " ";
                    }

                    bytesToSend[bytesToSend.Count() - 2] = (Byte)(crc16 % 0x100);
                    sMessageSent += bytesToSend[bytesToSend.Count() - 2].ToString("X").PadLeft(2, '0') + " ";

                    bytesToSend[bytesToSend.Count() - 1] = (Byte)(crc16 / 0x100);
                    sMessageSent += bytesToSend[bytesToSend.Count() - 1].ToString("X").PadLeft(2, '0') + " ";

                    MessageSent.Text = sMessageSent;
                    try
                    {
                        _serialPort.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                    catch
                    {
                    }
                }
            }
        }
        public void sendMessage(List<byte> alToSend)
        {
            if (_serialPort != null)
            {
                if (!_serialPort.IsOpen)
                {
                    //checkConns();
                    SerialComms.checkConns();
                }
                if (alToSend.Count > 0)
                {
                    byte[] bytesToSend = new byte[alToSend.Count + 2]; // the 2 is for the CRC we'll add at the end
                    String sMessageSent = "";
                    UInt16 crc16 = 0xFFFF;
                    for (int i = 0; i < alToSend.Count; i++)
                    {
                        Byte byteFromArray = (Byte)alToSend[i];
                        bytesToSend[i] = byteFromArray;
                        crc16 = CalculateCRC(byteFromArray, crc16);
                        sMessageSent += bytesToSend[i].ToString("X").PadLeft(2, '0') + " ";
                    }

                    bytesToSend[bytesToSend.Count() - 2] = (Byte)(crc16 % 0x100);
                    sMessageSent += bytesToSend[bytesToSend.Count() - 2].ToString("X").PadLeft(2, '0') + " ";

                    bytesToSend[bytesToSend.Count() - 1] = (Byte)(crc16 / 0x100);
                    sMessageSent += bytesToSend[bytesToSend.Count() - 1].ToString("X").PadLeft(2, '0') + " ";

                    MessageSent.Text = sMessageSent;
                    try
                    {
                        _serialPort.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                    catch
                    {
                    }
                }
            }
        }
        public void WriteBitMessage(Int16 iValue)
        {
            //ArrayList alReturn = new ArrayList();
            //alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            //alReturn.Add((byte)0x05); // Write Bit (Modbus Write Single Coil)


            ////In this example we're just sending this to address 0.
            //// **Note: these are offset by -1 from the # you setup in vBuilder.
            //alReturn.Add((byte)0x00);               // Starting Address Hi
            //alReturn.Add((byte)0x00);               // Starting Address Lo. 

            //if (iValue == 1)
            //{
            //    alReturn.Add((byte)0xFF);            // Quantity of Outputs Hi
            //}
            //else
            //{
            //    alReturn.Add((byte)0x00);            // Quantity of Outputs Hi
            //}
            //alReturn.Add((byte)0x00);                // Quantity of Outputs Lo

            ////sendMessage(alReturn);
            //MBBit thisMbo = new MBBit((byte)0x01, (byte)0x00, (byte)0x00);

            //lastSent = 0;
            var thisMbo = MBLst[0];
            //sendMessage(new ArrayList(thisMbo.formatWrite((byte)iValue)));
            //SerialComms.sendMessage(new ArrayList(thisMbo.formatWrite((byte)iValue)));
            SerialComms.sendMessage(thisMbo.GetWriteCmndMsg((byte)iValue));
        }

        public void Write16BitMessage(int iValue)
        {
            ArrayList alReturn = new ArrayList();
            alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            alReturn.Add((byte)0x06); // Write 16Bit (Modbus Write Single Coil)


            //In this example we're just sending this to address 0.
            // **Note: these are offset by -1 from the # you setup in vBuilder.
            alReturn.Add((byte)0x00);               // Starting Address Hi
            alReturn.Add((byte)0x02);               // Starting Address Lo. 

            var byteConversion = BitConverter.GetBytes((Int16)iValue);
            alReturn.Add(byteConversion[1]);
            alReturn.Add(byteConversion[0]);

            //SerialComms.sendMessage(alReturn);

            var thisMbo = MBLst[2];
            SerialComms.sendMessage(thisMbo.GetWriteCmndMsg((Int16)iValue));
        }

        private void WriteUInt16_Click(object sender, EventArgs e)
        {
            var thisMbo = MBLst[2];
        }

        private void WriteFloatMessage(float fValue)
        {
            //ArrayList alReturn = new ArrayList();
            //alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            //alReturn.Add((byte)0x10); // Write multiple 16Bit values (Modbus Write Multiple Registers)

            ////In this example we're just sending this to address 0.
            //// **Note: these are offset by -1 from the # you setup in vBuilder.
            //alReturn.Add((byte)0x00); // Starting Address Hi
            //alReturn.Add((byte)0x00); // Starting Address Lo. 

            //alReturn.Add((byte)0x00); // Quantity of Registers Hi
            //alReturn.Add((byte)0x02); // Quantity of Registers Lo. 

            //alReturn.Add((byte)0x04); // ByteCount in this case we're sending 4 bytes 

            //Byte[] myFloatBytes = new Byte[4];
            //myFloatBytes = BitConverter.GetBytes(fValue);

            //Byte[] ab = new Byte[4];
            //Byte[] bb = new Byte[4];
            //ab = BitConverter.GetBytes((float)2.0);
            //bb = BitConverter.GetBytes((float)((-1)*(2.0)));

            //alReturn.Add(myFloatBytes[3]); // Quantity of Outputs Hi of Least Significant Word
            //alReturn.Add(myFloatBytes[2]); // Quantity of Outputs Lo of Least Significant Word
            //alReturn.Add(myFloatBytes[1]); // Quantity of Outputs Hi of Most Significant Word
            //alReturn.Add(myFloatBytes[0]); // Quantity of Outputs Lo of Most Significant Word

            //sendMessage(alReturn);
            //lastSent = 1;
            //MB32Bit mb32Bit = new MB32Bit((byte)0x01, (byte)0x00, (byte)0x00);
            ////sendMessage(new ArrayList(mbFloat.formatWrite(fValue)));
            //List<byte> byteArray = mb32Bit.formatWrite(fValue);
            //PlcRequest plcRequest = new PlcRequest(byteArray, true, false);
            ////SerialComms.sendMessage(new ArrayList(mbFloat.formatWrite(fValue)), 1);
            //SerialComms.sendMessage(plcRequest);

            var thisMbo = MBLst[1];
            //sendMessage(new ArrayList(thisMbo.formatWrite((byte)iValue)));
            //SerialComms.sendMessage(new ArrayList(thisMbo.formatWrite((byte)iValue)));
            SerialComms.sendMessage(thisMbo.GetWriteCmndMsg(fValue));
        }   

        private void Write32BitMessage(int iValue)   
        {
            List<byte> alReturn = new List<byte>();
            alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            alReturn.Add((byte)0x10); // Write 16Bit (Modbus Write Single Coil)


            //In this example we're just sending this to address 0.
            // **Note: these are offset by -1 from the # you setup in vBuilder.
            alReturn.Add((byte)0x00);               // Starting Address Hi
            alReturn.Add((byte)0x00);               // Starting Address Lo. 

            alReturn.Add((byte)0x00);            // Quantity of Outputs Hi
            alReturn.Add((byte)0x02);            // Quantity of Outputs Lo
            alReturn.Add((byte)0x04);            // Byte Count

            //UInt32 reg7Val = (UInt32)this.write32Value.Value;
            //Byte byHi = (Byte)(reg7Val / 0x100);
            //Byte byLo = (Byte)(reg7Val);
            //Byte byHi1 = (Byte)(reg7Val / 0x1000000);
            //Byte byLo1 = (Byte)(reg7Val / 0x10000);
            //alReturn.Add(byHi1);            // Quantity of Outputs Hi
            //alReturn.Add(byLo1);            // Quantity of Outputs Lo
            //alReturn.Add(byHi);            // Quantity of Outputs Hi
            //alReturn.Add(byLo);            // Quantity of Outputs Lo

            Byte[] myIntBytes = BitConverter.GetBytes(iValue);
            alReturn.Add(myIntBytes[3]);            // Quantity of Outputs Hi
            alReturn.Add(myIntBytes[2]);            // Quantity of Outputs Lo
            alReturn.Add(myIntBytes[1]);            // Quantity of Outputs Hi
            alReturn.Add(myIntBytes[0]);            // Quantity of Outputs Lo

            //MB32Bit mb32Bit = new MB32Bit((byte)0x01, (byte)0x00, (byte)0x00);
            //List<byte> byteArray = mb32Bit.formatWrite(iValue);
            //PlcRequest plcRequest = new PlcRequest(byteArray, false, false);
            ////sendMessage(alReturn);
            //SerialComms.sendMessage(plcRequest);



            var thisMbo = MBLst[3];
            List<byte> lb = thisMbo.GetWriteCmndMsg(iValue);
            //sendMessage(new ArrayList(thisMbo.formatWrite((byte)iValue)));
            //SerialComms.sendMessage(new ArrayList(thisMbo.formatWrite((byte)iValue)));
            SerialComms.sendMessage(lb);

        }

        public void ReadBitMessage()
        {
            ArrayList alReturn = new ArrayList();
            alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            alReturn.Add((byte)0x01); // Read Bit Command (Modbus Read Coils Command)


            //In this example we're just sending this to address 0 (called Address 1 in vBuilder).
            // **Note: these are offset by -1 from the # you setup in vBuilder.
            //alReturn.Add((byte)0x00);               // Starting Address Hi
            //alReturn.Add((byte)0x00);               // Starting Address Lo. 
            alReturn.Add((byte)0x00);               // Starting Address Hi
            alReturn.Add((byte)0x00);               // Starting Address Lo. 

            //you can set the quantity to read more than 1 contiquous bits
            alReturn.Add((byte)0x00);                // Quantity of Outputs Hi
            alReturn.Add((byte)0x01);               // Quantity of Outputs Lo

            //MBBit mbBit = new MBBit((byte)0x01, (byte)0x00, (byte)0x00);
            var thisMbo = MBLst[0];
            SerialComms.sendMessage(thisMbo.readCmndMsg);
            //sendMessage(alReturn);
            //sendMessage(mbBit.readCmndMsg);
            //SerialComms.sendMessage(mbBit.readCmndMsg);
        }
        public void Read16BitMessage()
        {
            ArrayList alReturn = new ArrayList();
            alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            alReturn.Add((byte)0x03); // Read 16Bit Command (Modbus Read Holding Registers)


            //In this example we're just sending this to address 0 (called Address 1 in vBuilder).
            // **Note: these are offset by -1 from the # you setup in vBuilder.
            alReturn.Add((byte)0x00);               // Starting Address Hi
            alReturn.Add((byte)0x02);               // Starting Address Lo. 

            //you can set the quantity to read more than 1 contiquous bits
            alReturn.Add((byte)0x00);                // Quantity of Outputs Hi
            alReturn.Add((byte)0x01);               // Quantity of Outputs Lo

            //sendMessage(alReturn);
            //SerialComms.sendMessage(alReturn);

            var thisMbo = MBLst[2];
            var plcRequest = new PlcRequest(false, false);
            plcRequest.requestBytes = new List<byte>(thisMbo.readCmndMsg);
            SerialComms.sendMessage(plcRequest);


            //var thisMbo = MBLst[1];
            //var plcRequest = new PlcRequest(true, false);
            //plcRequest.requestBytes = new List<byte>(thisMbo.readCmndMsg);
            //Read32BitMessage(plcRequest);

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


        public void Read32BitMessage()
        {
            ArrayList alReturn = new ArrayList();
            alReturn.Add((byte)0x01); // PLC ID# in this example we set it to 1
            alReturn.Add((byte)0x04); // Read 16Bit Command (Modbus Read Holding Registers)
            //In this example we're just sending this to address 0 (called Address 1 in vBuilder).
            // **Note: these are offset by -1 from the # you setup in vBuilder.
            alReturn.Add((byte)0x00); // Starting Address Hi
            alReturn.Add((byte)0x00); // Starting Address Lo. 

            //you can set the quantity to read more than 1 contiguous bits
            alReturn.Add((byte)0x00); // Quantity of 16 Bit words to read
            alReturn.Add((byte)0x02); // Quantity of 16 Bit words to read

            //sendMessage(alReturn);
            SerialComms.sendMessage(alReturn);
        }
        public void Read32BitMessage(PlcRequest plcRequest)
        {
            //plcRequest.requestBytes.Add((byte)0x01);
            //plcRequest.requestBytes.Add((byte)0x04);

            //plcRequest.requestBytes.Add((byte)0x00);
            //plcRequest.requestBytes.Add((byte)0x00);

            //plcRequest.requestBytes.Add((byte)0x00);
            //plcRequest.requestBytes.Add((byte)0x02);
            SerialComms.sendMessage(plcRequest);
        }
        public void ReadFloatMessage()
        {
            // On the read we know what we are looking for, signed/unsigned, int/float, need to pass both values
            // use an object to contain the alReturn and both indicators
            /*
             * class plcRequest
             * {
             *      ArrayList requestBytes = new ArrayList();
             *      bool isFloat;
             *      bool isSigned;
             * }
             */

            var thisMbo = MBLst[1];
            var plcRequest = new PlcRequest(true, false);
            plcRequest.requestBytes = new List<byte>(thisMbo.readCmndMsg);
            Read32BitMessage(plcRequest);
        }

        private void write32value_ValueChanged(object sender, EventArgs e)
        {

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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void MessageRecieved_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void readFloat_Click(object sender, EventArgs e)
        {
            ReadFloatMessage();
        }

        private void read32FloatValue_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
