using System;

namespace Communication.model
{
    [Serializable]
    public class CustomPacket
    {
        private Operation _operation;
        private DataPacket _data;

        public Operation Operation => _operation;

        public DataPacket Data => _data;

        public CustomPacket(Operation operationOrder, DataPacket data)
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