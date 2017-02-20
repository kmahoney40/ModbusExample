using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Modbus_Example2.mbLib
{
    public class MBBase
    {
        protected byte deviceNumber;
        protected byte addressHi;
        protected byte addressLo;
        protected byte writeCmnd;
        protected byte readCmnd;
        protected byte countOutputHi;
        protected byte countOutputLo;
        public List<byte> readCmndMsg;
        public List<byte> writeCmndMsg;
        private ModbusObjInitializer initializer;


        public MBBase() { }
        protected MBBase(byte deviceNumber, byte addressHi, byte addressLo, byte writeCmnd, byte readCmnd, byte countOutputHi, byte countOutputLo)
        {
            this.countOutputHi = countOutputHi;
            this.countOutputLo = countOutputLo;
            CreateReadCmndMsg();
        }

        public MBBase(ModbusObjInitializer initializer)
        {
            InitializerToThis(initializer);
            CreatWriteCmndMsg();
            CreateReadCmndMsg();
        }

        private void CreateReadCmndMsg()
        {
            readCmndMsg = new List<byte>();
            readCmndMsg.Add(deviceNumber);
            readCmndMsg.Add(readCmnd);
            readCmndMsg.Add(addressHi);
            readCmndMsg.Add(addressLo);
            readCmndMsg.Add(countOutputHi);
            readCmndMsg.Add(countOutputLo);
        }
        private void CreatWriteCmndMsg()
        {
            writeCmndMsg = new List<byte>();
            writeCmndMsg.Add(deviceNumber);
            writeCmndMsg.Add(writeCmnd);
            //writeCmndMsg.Add(0x10);
            writeCmndMsg.Add(addressHi);
            writeCmndMsg.Add(addressLo);
        }
        private void InitializerToThis(ModbusObjInitializer init)
        {
            this.deviceNumber = init.deviceNum;
            this.readCmnd = init.readCmnd;
            this.writeCmnd = init.writeCmnd;
            this.addressHi = init.addressHi;
            this.addressLo = init.addressLo;
            this.countOutputHi = init.countOutputHi;
            this.countOutputLo = init.countOutputLo;

        }

        virtual public List<byte> formatWrite(byte value) { return new List<byte>(); }
        virtual public List<byte> formatWrite(Int16 value) { return new List<byte>(); }
        virtual public List<byte> formatWrite(float value) { return new List<byte>(); }
        virtual public List<byte> formatWrite(Int32 value) { return new List<byte>(); }

        virtual public List<byte> GetWriteCmndMsg(byte value) { return new List<byte>(); }
        virtual public List<byte> GetWriteCmndMsg(Int16 value) { return new List<byte>(); }
        virtual public List<byte> GetWriteCmndMsg(UInt16 value) { return new List<byte>(); }
        virtual public List<byte> GetWriteCmndMsg(float value) { return new List<byte>(); }
        virtual public List<byte> GetWriteCmndMsg(Int32 value) { return new List<byte>(); }
        //virtual public List<byte> GetWriteCmndMsg(Int32 value);

        virtual public void WriteMessage(int value) { throw new NotImplementedException(); }

    }

    public class ModbusObjInitializer
    {
        public ModbusObjType type { get; set; }
        public byte deviceNum { get; set; }
        public byte addressHi { get; set; }
        public byte addressLo { get; set; }
        public byte writeCmnd { get; set; }
        public byte readCmnd { get; set; }
        public byte countOutputHi { get; set; }
        public byte countOutputLo { get; set; }
        public ModbusObjInitializer(string type, byte deviceNum, byte writeCmnd, byte readCmnd, byte addressHi, byte addressLo)
        {
            this.type = ConvertToModbusObjType(type);
            this.deviceNum = deviceNum;
            this.writeCmnd = writeCmnd;
            this.readCmnd = readCmnd;
            this.addressHi = addressHi;
            this.addressLo = addressLo;
            setReadWriteCmnd();
            setReadCmnd();
        }

        private void setReadCmnd()
        {
            
        }

        private ModbusObjType ConvertToModbusObjType(string type)
        {
            switch (type)
            {
                case "Bit":
                    return ModbusObjType.Bit;
                case "Int16":
                    return ModbusObjType.Int16;
                case "Float":
                    return ModbusObjType.Float;
                case "Int32":
                    return ModbusObjType.Int32;
                default:
                    throw new NotSupportedException();
            }
        }
        private void setReadWriteCmnd()
        {
            switch (type)
            {
                case ModbusObjType.Bit:
                    countOutputHi = (byte)0x00;
                    countOutputLo = (byte)0x01;
                    break;
                case ModbusObjType.Int16:
                    countOutputHi = (byte)0x00;
                    countOutputLo = (byte)0x01;
                    break;
                case ModbusObjType.Float:
                case ModbusObjType.Int32:
                    countOutputHi = (byte)0x00;
                    countOutputLo = (byte)0x02;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
