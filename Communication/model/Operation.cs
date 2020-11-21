using System;

namespace Communication.model
{
    [Serializable]
    public enum Operation
    {
        CreateUser,
        LoginUser,
        ListTopics,
        CreateTopic,
        JoinTopic,
        LeaveTopic,
        SendToTopic,
        SendToUser,
        
        Refused,
        Reception
    }
}