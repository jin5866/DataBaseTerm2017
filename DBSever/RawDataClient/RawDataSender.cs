using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace RawDataClient
{
    static class RawDataSender
    {
        static Socket _client;
        static string _ServerIP = "127.0.0.1";
        static int _ServerPort = 50100;

        public static void StartSender()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ServerIP),_ServerPort);

            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.RemoteEndPoint = ep;

            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);

            _client.ConnectAsync(connectArgs);

            while(true)
            {
                if(_client.Connected)
                {
                    string input = Console.ReadLine();

                    SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                    byte[] msg = Encoding.Default.GetBytes(input);

                    sendArgs.SetBuffer(msg, 0, msg.Length);
                    sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

                    _client.SendAsync(sendArgs);
                }

            }
        }

        static void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            if(e.SocketError == SocketError.Success)
            {
                Console.WriteLine("Connect Completed");
            }
            else
            {
                Console.WriteLine("Connect Failed");
            }
        }

        static void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if(e.SocketError != SocketError.Success)
            {
                Console.WriteLine("Send Failed");
            }
        }



    }
}
