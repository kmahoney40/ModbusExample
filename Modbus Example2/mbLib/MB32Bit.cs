using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus_Example2.mbLib
{
    class MB32Bit : MBBase
    {
        public MB32Bit() { }
        //public MB32Bit(ModbusObjInitializer initializer) :
        //    base(initializer.deviceNum, initializer.addressHi, initializer.addressLo, 
        //    (byte)0x05, (byte)0x01, (byte)0x00, (byte)0x01) { }
        public MB32Bit(ModbusObjInitializer initializer) : base(initializer)
        {
            CompleteWrtieMsg();
        }

        public MB32Bit(byte deviceNumber, byte addressHi, byte addressLo) : 
            base(deviceNumber, addressHi, addressLo, (byte)0x10, (byte)0x04, (byte)0x00, (byte)0x02) { }

        private void CompleteWrtieMsg()
        {
            writeCmndMsg.Add(0); // Quantity of Outputs Hi of Least Significant Word
            writeCmndMsg.Add(2); // Quantity of Outputs Lo of Least Significant Word
            writeCmndMsg.Add((byte)0x04); // ByteCount in this case we're sending 4 bytes 
            writeCmndMsg.Add(0); // Quantity of Outputs Hi of Least Significant Word
            writeCmndMsg.Add(0); // Quantity of Outputs Lo of Least Significant Word
            writeCmndMsg.Add(0); // Quantity of Outputs Hi of Most Significant Word
            writeCmndMsg.Add(0); // Quantity of Outputs Lo of Most Significant Word

        }

        public override List<byte> GetWriteCmndMsg(float value)
        {
            Byte[] myFloatBytes = new byte[4];
            myFloatBytes = BitConverter.GetBytes(value);

            writeCmndMsg[7] = myFloatBytes[3];
            writeCmndMsg[8] = myFloatBytes[2];
            writeCmndMsg[9] = myFloatBytes[1];
            writeCmndMsg[10] = myFloatBytes[0];

            return writeCmndMsg;
        }

        public override List<byte> GetWriteCmndMsg(Int32 value)
        {
            Byte[] myFloatBytes = new Byte[4];
            myFloatBytes = BitConverter.GetBytes(value);

            writeCmndMsg[7] = myFloatBytes[3];
            writeCmndMsg[8] = myFloatBytes[2];
            writeCmndMsg[9] = myFloatBytes[1];
            writeCmndMsg[10] = myFloatBytes[0];

            return writeCmndMsg;
        }


        public override List<byte> formatWrite(float value)
        {
            List<byte> byteList = createByteList();// new List<byte>();
            //retLst.Add(deviceNumber);
            //retLst.Add(writeCmnd);
            //retLst.Add(addressHi);
            //retLst.Add(addressLo);

            //retLst.Add((byte)0x00); // Quantity of Registers Hi
            //retLst.Add((byte)0x02); // Quantity of Registers Lo. 

            //retLst.Add((byte)0x04); // ByteCount in this case we're sending 4 bytes 

            Byte[] myFloatBytes = new Byte[4];
            myFloatBytes = BitConverter.GetBytes(value);

            byteList[7] = myFloatBytes[3];
            byteList[8] = myFloatBytes[2];
            byteList[9] = myFloatBytes[1];
            byteList[10] = myFloatBytes[0];

            //retLst.Add(myFloatBytes[3]); // Quantity of Outputs Hi of Least Significant Word
            //retLst.Add(myFloatBytes[2]); // Quantity of Outputs Lo of Least Significant Word
            //retLst.Add(myFloatBytes[1]); // Quantity of Outputs Hi of Most Significant Word
            //retLst.Add(myFloatBytes[0]); // Quantity of Outputs Lo of Most Significant Word

            return byteList;
        }
        public override List<byte> formatWrite(int value)
        {
            List<byte> byteList = createByteList();

            //byteArray.Add(deviceNumber);
            //byteArray.Add(writeCmnd);
            //byteArray.Add(addressHi);
            //byteArray.Add(addressLo);

            //byteArray.Add((byte)0x00); // Quantity of Registers Hi
            //byteArray.Add((byte)0x02); // Quantity of Registers Lo. 

            //byteArray.Add((byte)0x04); // ByteCount in this case we're sending 4 bytes 

            Byte[] myFloatBytes = new Byte[4];
            myFloatBytes = BitConverter.GetBytes(value);
            byteList[7] = myFloatBytes[3];
            byteList[8] = myFloatBytes[2];
            byteList[9] = myFloatBytes[1];
            byteList[10] = myFloatBytes[0];
            //byteArray.Add(myFloatBytes[3]);
            //byteArray.Add(myFloatBytes[2]);
            //byteArray.Add(myFloatBytes[1]);
            //byteArray.Add(myFloatBytes[0]);

            return byteList;
        }

        private List<byte> createByteList()
        {
            List<byte> retLst = new List<byte>();
            retLst.Add(deviceNumber);
            retLst.Add(writeCmnd);
            retLst.Add(addressHi);
            retLst.Add(addressLo);

            retLst.Add((byte)0x00); // Quantity of Registers Hi
            retLst.Add((byte)0x02); // Quantity of Registers Lo. 

            retLst.Add((byte)0x04); // ByteCount in this case we're sending 4 bytes 

            retLst.Add(0); // Quantity of Outputs Hi of Least Significant Word
            retLst.Add(0); // Quantity of Outputs Lo of Least Significant Word
            retLst.Add(0); // Quantity of Outputs Hi of Most Significant Word
            retLst.Add(0); // Quantity of Outputs Lo of Most Significant Word

            return retLst;
        }
    }
}
