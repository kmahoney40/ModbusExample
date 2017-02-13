using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus_Example2.mbLib
{
    public enum ModbusObjType
    {
        Bit,
        Bit16,
        Bit32
    }

    class Factory
    {
        public MBBase GetModbusObj(ModbusObjInitializer initializer)
        {
            switch (initializer.type)
            {
                case ModbusObjType.Bit:
                    return new MBBit(initializer);
                case ModbusObjType.Bit16:
                    throw new NotSupportedException();
                case ModbusObjType.Bit32:
                    return new MB32Bit(initializer);
                default:
                    throw new NotSupportedException();
            }
        }

    }
}
