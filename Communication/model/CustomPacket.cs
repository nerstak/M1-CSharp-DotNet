using System;

namespace Communication.model
{
    public interface CustomPacket
    {
        Operation OperationOrder
        {
            get;
        }

        DataPacket Data
        {
            get;
            set;
        }
    }
}