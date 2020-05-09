using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ChatServer
{
    static class TCPhandler
    {
        public static Socket TCPListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static void StartWork() 
        {
            IPEndPoint serverEndPoint = new IPEndPoint(Program.server.nodeInformation.NodeIP, Server.PortNumber);
            try
            {
                TCPListener.Bind(serverEndPoint);
                TCPListener.Listen(Server.MaxClientsAmount);
                Console.WriteLine("TCP");
                while (true)
                {
                    Socket messageHandler = TCPListener.Accept();
                    Program.server.ConnectionsCount++;
                    Program.server.Connections.Add(Program.server.ConnectionsCount, new Connection(messageHandler));
                    Thread thread = new Thread(new ParameterizedThreadStart(ReceiveMessages)); 
                    thread.Start(Program.server.ConnectionsCount); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 

        static void ReceiveMessages(object messageHandlerID)
        {
            Socket handler = Program.server.Connections[(int)messageHandlerID].socket;
            while (handler.Connected)
            {
                byte[] data = new byte[10000];
                MemoryStream message = new MemoryStream();
                int sum = 0;
                do
                {
                    try
                    {
                        int count = handler.Receive(data);
                        message.Write(data, sum, count);
                        sum += count;
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex);
                        Program.server.HandleExit((int)messageHandlerID);
                    }
                    catch 
                    {

                    }
                }
                while (handler.Available > 0);
                if (message.GetBuffer().Length > 0)
                    Program.server.HandleMessage(Program.server.serializer.Deserialize(message.GetBuffer()), (int)messageHandlerID);
            }
        }
    }
}
