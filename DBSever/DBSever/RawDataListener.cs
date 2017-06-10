using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Server.DBdata;

namespace DBServer
{
    static class RawDataListener
    {
        static private int accessCounter = 0;
        static private int recieveCounter = 0;

        static List<Socket> _client = new List<Socket>(); 

        static private int _port = 50100;
        static private int _backLogQueue = 5;
        //private int _listenTime = 100;




        static private Socket lisSocket;
        
        static public Queue<byte[]> lisQueue { get; private set; }

        /// <summary>
        /// Listening Server를 시작한다.
        /// </summary>
        static public void StartServer()
        {
            lisQueue = new Queue<byte[]>();

            lisSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            lisSocket.Bind(new IPEndPoint(IPAddress.Any, _port));

            lisSocket.Listen(_backLogQueue);

            Console.WriteLine("---------------------");
            Console.WriteLine("LIsten 시작 port number: " + _port );
            Console.WriteLine("---------------------");

            SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
            acceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);

            lisSocket.AcceptAsync(acceptArgs);

            Console.WriteLine("---------------------");
            Console.WriteLine("LIstening Server Start");
            Console.WriteLine("---------------------");
        }

        private static void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            
            if(e.SocketError == SocketError.Success)
            {
                //성공한경우
                accessCounter++;

                // 새로 생긴 소켓
                Socket clientSocket = e.AcceptSocket;
                _client.Add(clientSocket);

                SocketAsyncEventArgs recieveArgs = new SocketAsyncEventArgs();
                recieveArgs.SetBuffer(new Byte[100], 0, 100);

                recieveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecieveCompleted);

                //비동기적으로 받기 시작
                clientSocket.ReceiveAsync(recieveArgs);

                //받기 다시 시작
                Socket server = sender as Socket;
                e.AcceptSocket = null;
                server.AcceptAsync(e);

            }
            else if(e.SocketError == SocketError.OperationAborted)
            {
                //도중에 실패
                ExceptionMessage.PrintListenFailOnListening();


            }
            else
            {
                //실패
                ExceptionMessage.PrintListenFail();

                //받기 다시 시작
                Socket server = sender as Socket;
                e.AcceptSocket = null;
                server.AcceptAsync(e);
            }
        }

        private static void OnRecieveCompleted(object sender,SocketAsyncEventArgs e)
        {
            if(e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                recieveCounter++;

                //수신한 바이트로 할 일
                DealBuffer(e.Buffer);

                Socket client = sender as Socket;
                e.SetBuffer(new Byte[100], 0, 100);
                client.ReceiveAsync(e);
            }
        }

        // 버퍼를 큐에 넣는다
        private static void DealBuffer(byte[] buffer)
        {
            lock(lisQueue)
            {
                lisQueue.Enqueue(buffer);
            }
        }
    }
}
