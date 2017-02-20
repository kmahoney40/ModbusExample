using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus_Example2
{
    public class ReqType
    {
        public bool isFloat;
        public bool isSigned;
        public ReqType(bool isFloat, bool isSigned)
        {
            this.isFloat = isFloat;
            this.isSigned = isSigned;
        }
    }
    public class PlcRequest : ReqType
    {
        public List<byte> requestBytes = new List<byte>();

        public PlcRequest(bool isFloat, bool isSigned) : base(isFloat, isSigned)
        {
        }

        public PlcRequest(List<byte> requestBytes, bool isFloat, bool isSigned) : base(isFloat, isSigned)
        {
            this.requestBytes = requestBytes;
        }
    }
}
