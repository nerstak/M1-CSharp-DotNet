using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ClientText.view;
using Communication.model;

namespace ClientText.controller
{
    /// <summary>
    /// Parse a command
    /// </summary>
    public class CommandParser
    {

        private static readonly string ERROR_ARGS = "Error in the number of argument";
        private static readonly string ERROR_UNKNOWN = "This command does not exists";
        private string _message;

        public string Message => _message;

        /// <summary>
        /// Parse a command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <returns>Packet or null</returns>
        public CustomPacket ParseCommand(string cmd)
        {
            if (String.IsNullOrWhiteSpace(cmd)) return null;
            
            string[] parts = cmd.Split(" ");
            parts[0] = parts[0].ToLower();
            return CreatePacket(parts);
        }

        /// <summary>
        /// Create a packet from command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <returns>Packet or null</returns>
        private CustomPacket CreatePacket(string[] cmd)
        {
            switch (cmd[0])
            {
                case "join-topic":
                    return ParseTopic(cmd, Operation.JoinTopic);
                case "leave-topic":
                    return ParseTopic(cmd, Operation.LeaveTopic);
                case "list-topic":
                    return ParseList(cmd);
                case "create-topic":
                    return ParseTopic(cmd, Operation.CreateUser);
                case "pm":
                    return ParsePm(cmd, Operation.SendToUser);
                case "tell":
                    return ParseTell(cmd, Operation.SendToTopic);
                default:
                    _message = ERROR_UNKNOWN;
                    break;
            }

            return null;
        }

        /// <summary>
        /// Parse an on topic command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="op">Operation</param>
        /// <returns>Packet or null</returns>
        private CustomPacket ParseTopic(string[] cmd, Operation op)
        {
            if (cmd.Length == 2)
            {
                return new CustomPacket(op, new Topic(cmd[1]));
            }

            _message = ERROR_ARGS;

            return null;
        }

        /// <summary>
        /// Parse a list command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <returns>Packet or null</returns>
        private CustomPacket ParseList(string[] cmd)
        {
            if (cmd.Length != 1)
            {
                return new CustomPacket(Operation.ListTopics,null);
            }
            _message = ERROR_ARGS;

            return null;
        }

        /// <summary>
        /// Parse a PM command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="op">Operation</param>
        /// <returns>Packet or null</returns>
        private CustomPacket ParsePm(string[] cmd, Operation op)
        {
            if (cmd.Length >= 2)
            {
                User u = new User {Username = cmd[1]};
                return ParseMessage(cmd, op, u);
            }
            _message = ERROR_ARGS;

            return null;
        }

        /// <summary>
        /// Parse a tell command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="op">Operation</param>
        /// <returns>Packet or null</returns>
        private CustomPacket ParseTell(string[] cmd, Operation op)
        {
            if (cmd.Length >= 2)
            {
                Topic t = new Topic(cmd[1]);
                return ParseMessage(cmd, op, t);
            }
            _message = ERROR_ARGS;

            return null;
        }

        /// <summary>
        /// Parse a message command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="op">Operation</param>
        /// <param name="r">Recipient</param>
        /// <returns>Custom Packet</returns>
        private CustomPacket ParseMessage(string[] cmd, Operation op, Recipient r)
        {
            // Recovering message
            List<string> tmp = cmd.ToList();
            for(var i = 0; i < 2 ;i++)tmp.RemoveAt(0);
            
            // Creating message
            Message msg = new Message(string.Join(" ", tmp), Client.CurrentUser, r);
            
            return new CustomPacket(op,msg);
        }

    }
}