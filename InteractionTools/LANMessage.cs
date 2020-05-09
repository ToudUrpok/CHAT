﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;


namespace InteractionTools
{
    public enum MessageType { UDPRequest, UDPResponse, PrivateMess, CommonMess, Identification, ClientJoin, ClientExit, ClientHistory };

    [Serializable]
    public class LANMessage
    {
        public MessageType messageType;
        public int SenderID;
        public string SenderName;
        public int Port;
        public string IP;
        public string content;
        public List<DialogInfo> Dialogs;
        public LANMessage(MessageType messType, int senderId, int receiverId, string ip, string cont)
        {
            messageType = messType;
            SenderID = senderId;
            Port = receiverId;
            IP = ip;
            content = cont;
        }
        public LANMessage(MessageType messType, string ip, int port)
        {
            messageType = messType;
            Port = port;
            IP = ip;
        }
        public LANMessage(MessageType messType, string cont, int senderId, string ip)
        {
            messageType = messType;
            SenderName = cont;
            content = cont;
            SenderID = senderId; 
            IP = ip;
        }
        public LANMessage(MessageType messType, List<DialogInfo> dialogs)
        {
            messageType = messType;
            Dialogs = dialogs;
        }
        public LANMessage(MessageType messType, int newClientId, string name)
        {
            messageType = messType;
            SenderID = newClientId;
            SenderName = name;
        }
        public LANMessage(MessageType messType, int newClientId, DialogInfo dialog)
        {
            messageType = messType;
            SenderID = newClientId;
            Dialogs = new List<DialogInfo>
            {
                dialog
            };
        }

        public LANMessage()
        {
        }
    }
}