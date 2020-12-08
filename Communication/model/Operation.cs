using System;

namespace Communication.model
{
    /// <summary>
    /// Enumeration of actions possible, or responses
    /// </summary>
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