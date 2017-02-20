using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus_Example2.mbLib
{
    class MBBit : MBBase
    {
        public MBBit() { }

        public MBBit(ModbusObjInitializer initializer) : base(initializer) 
        {
            CompleteWriteMsg();
        }

        public MBBit(byte deviceNumber, byte addressHi, byte addressLo) : 
            base(deviceNumber, addressHi, addressLo, (byte)0x05, (byte)0x01, (byte)0x00, (byte)0x01) { }

        private void CompleteWriteMsg()
        {
            writeCmndMsg.Add((byte)0x00); // cmndOutputHi, will be assigned 0x00 or 0xFF
            writeCmndMsg.Add((byte)0x00);
        }

        public override List<byte> GetWriteCmndMsg(byte value)
        {
            switch (value)
            {
                case 0x00:
                    writeCmndMsg[4] = (byte)0x00;
                    break;
                case 0x01:
                case 0xFF:
                    writeCmndMsg[4] = (byte)0xFF;
                    break;
                default:
                    throw new NotSupportedException();
            }
            return writeCmndMsg;
        }
        public override List<byte> formatWrite(byte value) 
        {
            List<byte> retLst = new List<byte>();
            retLst.Add(deviceNumber);
            retLst.Add(writeCmnd);
            retLst.Add(addressHi);
            retLst.Add(addressLo);

            if (value == 0x01)
            {
                retLst.Add((byte)0xFF);
            }
            else
            {   
                retLst.Add((byte)0x00);
            }
            retLst.Add((byte)0x00);
                
            return retLst; 
        }

        public override void WriteMessage(int value)
        {

        }
    }
}
