using System;

namespace Communication.model
{
    [Serializable]
    public class CustomPacket
    {
        private Operation operation;
        private DataPacket data;
        
        public Operation OperationOrder
        {
            get;
        }

        public DataPacket Data
        {
            get;
            set;
        }

        public CustomPacket(Operation operationOrder, DataPacket data)
        {
            OperationOrder = operationOrder;
            Data = data;
        }
    }
}