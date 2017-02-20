using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus_Example2.mbLib
{
    class MB16Bit : MBBase
    {
        public MB16Bit() { }
        public MB16Bit(ModbusObjInitializer initializer) : base(initializer)
        {
            CompleteWrtieMsg();
        }

        private void CompleteWrtieMsg()
        {
            writeCmndMsg.Add((byte)0x00); // The 16 bit command
            writeCmndMsg.Add((byte)0x00);
        }

        public override List<byte> GetWriteCmndMsg(Int16 value)
        {
            byte[] byteCmnd = new byte[4];
            byteCmnd = BitConverter.GetBytes(value);

            writeCmndMsg[4] = byteCmnd[1];
            writeCmndMsg[5] = byteCmnd[0];

            return writeCmndMsg;
        }

        public override List<byte> GetWriteCmndMsg(UInt16 value)
        {
            byte[] byteCmnd = new byte[4];
            byteCmnd = BitConverter.GetBytes(value);

            writeCmndMsg[4] = byteCmnd[1];
            writeCmndMsg[5] = byteCmnd[0];

            return writeCmndMsg;
        }

        public override List<byte> formatWrite(Int16 value)
        {
            List<byte> byteList = createByteList();

            byte[] byteValue = new byte[2];
            byteValue = BitConverter.GetBytes(value);
            byteList[7] = byteValue[1];
            byteList[8] = byteValue[0];

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
            retLst.Add((byte)0x01); // Quantity of Registers Lo. 

            retLst.Add(0); // Quantity of Outputs Hi
            retLst.Add(0); // Quantity of Outputs Lo

            return retLst;
        }

    }
}
