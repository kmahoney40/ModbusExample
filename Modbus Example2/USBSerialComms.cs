using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
using Modbus_Example2.mbLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace Modbus_Example2
{
    class USBSerialComms
    {
        private SerialPort serialPort = new SerialPort();
        private bool bInsideCheckConns = false;
        private bool bCheckConnsCheckAgain = false;
        private SerialPort _serialPort = new SerialPort();
        private bool bDevicePluggedIn = false;
        private int lastSent = 0;
        private ReqType reqType;
        private int iCommNum = -1;
        private PlcRequest plcRequest;
        private Form1 form;

        public USBSerialComms(Form1 form)
        {
            this.form = form;
        }

        public static string[] GetUSBCOMDevices()
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

        public void CloseIfAnotherProcessIsUsingDevice()
        {
            Process[] pname = Process.GetProcessesByName("vFactory");
            Process[] pnameViewer = Process.GetProcessesByName("vFactory Viewer");
            Process[] pnameBuilder = Process.GetProcessesByName("vBuilder");

            if (pname.Length != 0)
            {
                MessageBox.Show("This and vFactory can't run at the same time.  \nClose vFactory or vFactory Viewer before runing This.");
                form.Close();
            }
            else if (pnameViewer.Length != 0)
            {
                MessageBox.Show("This and vFactory Viewer can't run at the same time.  \nClose vFactory Viewer before runing This.");
                form.Close();
            }
            else if (pnameBuilder.Length != 0)
            {
                MessageBox.Show("This and vBuilder can't run at the same time.  \nClose vBuilder before runing This.");
                form.Close();
            }
            else
            {
                //this.Invoke((MethodInvoker)delegate { checkConns(); });
                form.Invoke((MethodInvoker)delegate { checkConns(); });
            }
        }

        public void checkConns()
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
                        //ArrayList alCommPortInfo = new ArrayList();
                        List<COMPortInfo> alCommPortInfo = new List<COMPortInfo>();
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
                                form.Invoke(new delegateUpdateUSBEnabled(updateUSBEnabled), false);
                            }
                            else
                            {
                                form.Invoke(new delegateUpdateUSBEnabled(updateUSBEnabled), true);
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
                        form.Invoke(new delegateUpdateUSBEnabled(updateUSBEnabled), false);
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
        public void updateUSBEnabled(bool bEnabled)
        {
            if (bEnabled)
            {
                form.plcStatusLabel.ForeColor = Color.Green;
                form.plcStatusLabel.Text = "PLC Connected";
                //PLCStatusLabel.ForeColor = Color.Green;
                //PLCStatusLabel.Text = "PLC Connected";
            }
            else
            {
                form.plcStatusLabel.ForeColor = Color.Red;
                form.plcStatusLabel.Text = "PLC Disconnected";
                //PLCStatusLabel.ForeColor = Color.Red;
                //PLCStatusLabel.Text = "PLC Disconnected";
            }
        }

        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] readValBytes = new byte[((SerialPort)sender).BytesToRead];
                int iResult = ((SerialPort)sender).Read(readValBytes, 0, ((SerialPort)sender).BytesToRead);

                form.BeginInvoke(new SetTextDeleg(si_DataReceivedEventArgs), new object[] { readValBytes, ((SerialPort)sender) });
            }
            catch
            {
            }
        }
        private delegate void SetTextDeleg(byte[] readVal, SerialPort inputPort);

        private UInt16 CalculateCRC(Byte dchar, UInt16 crc16)
        {
            UInt16 mask = (UInt16)(dchar & 0x00FF);
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
                for (int i = 0; i < readVal.Count(); i++)
                {
                    crc16 = CalculateCRC((Byte)readVal[i], crc16);
                }
                if (crc16 == 0)
                    bCRCPasses = true;  // In this example, we're not doing anything with this result, but you could make sure it passes before using the data


                /*
                 * Add an xml or JSON file to declare addresses and data types
                 * */


                form.messageReceived.Text = sMessageRecieved;

                byte[] byteArray = { (byte)0, (byte)0, (byte)0, (byte)0 };

                switch (readVal[1])
                {
                    case 0x01: // Read Bit Command (Modbus Read Coils Command)
                    case 0x02: // (Modbus Read Discrete Inputs)
                        int iMessageLength = readVal[2]; // Byte Count (# of data bytes)
                        UInt16 messageValue = (UInt16)(readVal[3]); // the bit value(s).  In our case we're only reading 1 bit.
                        form.ReadBitValue.Text = messageValue.ToString();

                        break;
                    case 0x03: // Read 16Bit Command (Modbus Read Holding Registers)
                        //byteArray[0] = readVal[4];
                        //byteArray[1] = readVal[3];
                        //Int16 myInt16 = System.BitConverter.ToInt16(byteArray, 0);

                        // readVal[2] is the byte count

                        byteArray[0] = readVal[4];
                        byteArray[1] = readVal[3];
                        if (readVal[2] == 2)
                        {
                            Int16 myInt16 = System.BitConverter.ToInt16(byteArray, 0);
                            form.Read16Value.Text = myInt16.ToString();
                        }
                        else
                        {
                            byteArray[2] = readVal[6];
                            byteArray[3] = readVal[5];
                            var Float0x03 = System.BitConverter.ToSingle(byteArray, 0);
                            form.Read32FloatValue.Text = Float0x03.ToString();
                        }
                        //byteArray[2] = readVal[6];
                        //byteArray[3] = readVal[5];
                        //var tmpp = System.BitConverter.ToSingle(byteArray, 0);
                        //Int16 myInt16 = System.BitConverter.ToInt16(byteArray, 0);
                        //var asdf = System.BitConverter.ToInt16(byteArray, 0);
                        //float myFloa = System.BitConverter.ToSingle(byteArray, 0);
                        ////form.Read16Value.Text = myInt16.ToString();
                        //var tmp = Math.Round((Decimal)myFloa, 1, MidpointRounding.AwayFromZero);
                        //form.Read16Value.Text = tmp.ToString();
                        //form.Read16Value.Text = myInt16.ToString();
                        break;
                    case 0x04: // (Modbus Read Input Registers)

                        // this is good for reading holding registers, the uncommented below
                        // is for inpyut registers, ie therm input. Use Cmnd 3 for ordering like
                        // the uncommentd below

                        //byteArray[2] = readVal[4];
                        //byteArray[3] = readVal[3];
                        //byteArray[0] = readVal[6];
                        //byteArray[1] = readVal[5];

                        // use a swithc with an exception for != 2 and != 4
                        if (readVal[2] == 2)
                        {
                            byteArray[0] = readVal[4];
                            byteArray[1] = readVal[3];
                            if (reqType.isSigned)
                            {
                                Int16 myInt16 = System.BitConverter.ToInt16(byteArray, 0);
                                form.Read16Value.Text = myInt16.ToString();
                            }
                            else
                            {
                                UInt16 myUInt16 = System.BitConverter.ToUInt16(byteArray, 0);
                                form.Read16Value.Text = myUInt16.ToString();
                            }
                        }
                        else
                        {
                            byteArray[2] = readVal[4];
                            byteArray[3] = readVal[3];
                            byteArray[0] = readVal[6];
                            byteArray[1] = readVal[5];
                            if (reqType.isFloat)
                            {
                                var i = System.BitConverter.ToUInt32(byteArray, 0);
                                float myFloat = System.BitConverter.ToSingle(byteArray, 0);
                                var temp = Math.Round((Decimal)myFloat, 2, MidpointRounding.AwayFromZero);
                                //form.Read32FloatValue.Text = myFloat.ToString();
                                form.Read32FloatValue.Text = temp.ToString();
                            }
                            else
                            {
                                Int32 myInt = System.BitConverter.ToInt32(byteArray, 0);
                                form.Read32Value.Text = myInt.ToString();
                            }
                        }

                        break;

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

        public void sendMessage(ArrayList alToSend, int lastSent = 0)
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

                    form.messageSent.Text = sMessageSent;
                    try
                    {
                        this.lastSent = lastSent;
                        _serialPort.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                    catch
                    {
                    }
                }
            }
        }


        public void sendMessage(PlcRequest plcRequest)
        {
            if (_serialPort != null)
            {
                if (!_serialPort.IsOpen)
                {
                    checkConns();
                    //SerialComms.checkConns();
                }
                List<byte> requestBytes = plcRequest.requestBytes;
                //if (alToSend.Count > 0)
                if (requestBytes.Count > 0)
                {
                    //byte[] bytesToSend = new byte[alToSend.Count + 2]; // the 2 is for the CRC we'll add at the end
                    byte[] bytesToSend = new byte[requestBytes.Count + 2]; // the 2 is for the CRC we'll add at the end
                    String sMessageSent = "";
                    UInt16 crc16 = 0xFFFF;
                    //for (int i = 0; i < alToSend.Count; i++)
                    for (int i = 0; i < requestBytes.Count; i++)
                    {
                        //Byte byteFromArray = (Byte)alToSend[i];
                        Byte byteFromArray = (Byte)requestBytes[i];
                        bytesToSend[i] = byteFromArray;
                        crc16 = CalculateCRC(byteFromArray, crc16);
                        sMessageSent += bytesToSend[i].ToString("X").PadLeft(2, '0') + " ";
                    }

                    bytesToSend[bytesToSend.Count() - 2] = (Byte)(crc16 % 0x100);
                    sMessageSent += bytesToSend[bytesToSend.Count() - 2].ToString("X").PadLeft(2, '0') + " ";

                    bytesToSend[bytesToSend.Count() - 1] = (Byte)(crc16 / 0x100);
                    sMessageSent += bytesToSend[bytesToSend.Count() - 1].ToString("X").PadLeft(2, '0') + " ";

                    form.messageSent.Text = sMessageSent;
                    try
                    {
                        reqType = new ReqType(plcRequest.isFloat, plcRequest.isSigned);
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
                    checkConns();
                    //SerialComms.checkConns();
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

                    form.messageSent.Text = sMessageSent;
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

        private string GetIntBinaryString(int n, bool bit32)
        {
            int pos = 15;
            int i = 0;
            int size = 16;
            if (bit32)
            {
                size = 32;
                pos = 31;
            }
            char[] b = new char[size];

            while (i < size)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }
    }

    public class COMPortInfo
    {
        public String portName;
        public String friendlyName;
    }

}
