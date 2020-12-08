using System;

namespace Communication.model
{
    /// <summary>
    /// Custom Packet, that encapsulate data sent
    /// </summary>
    [Serializable]
    public class CustomPacket
    {
        private Operation _operation;
        private IDataPacket _data;

        public Operation Operation => _operation;

        public IDataPacket Data => _data;

        public CustomPacket(Operation operationOrder, IDataPacket data)
        {
            this._operation = operationOrder;
            this._data = data;
        }

        public override string ToString()
        {
            string tmp = "";
            if (_operation == Operation.Refused)
            {
                tmp += "Error: ";
            }

            return tmp + _data;
        }
    }
}