using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus_Example2.mbLib
{
    public enum ModbusObjType
    {
        Bit,
        UInt16,
        Int16,
        Float,
        Int32
    }

    class Factory
    {
        public MBBase GetModbusObj(ModbusObjInitializer initializer)
        {
            switch (initializer.type)
            {
                case ModbusObjType.Bit:
                    return new MBBit(initializer);
                case ModbusObjType.Int16:
                    return new MB16Bit(initializer);
                case ModbusObjType.Float:
                case ModbusObjType.Int32:
                    return new MB32Bit(initializer);
                default:
                    throw new NotSupportedException();
            }
        }

    }
}
