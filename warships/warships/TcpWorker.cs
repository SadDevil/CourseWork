using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace warships
{
    public class TcpWorker
    {
        private TcpClient _client;
        private TcpListener _listener;

        public event Action<string> RecieveMessagesEvent;

        protected virtual void OnRecieveMessagesEvent(string obj)
        {
            Action<string> handler = RecieveMessagesEvent;
            if (handler != null) handler(obj);
        }

        public void Reset()
        {
            if (_listener != null)
                _listener.Stop();

            if (_client != null)
                _client.Close();
        }

        /// <summary>
        /// Викличе SocketException, якщо порт вже прослуховується
        /// </summary>
        /// <param name="port"></param>
        public void Listen(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _client = _listener.AcceptTcpClient();
            StartRecieve();

        }

        public void Join(string ip, int port)
        {
            try
            {
                _client = new TcpClient(ip, port);
            }
            catch (SocketException e)
            {
                Join(ip, port);
            }
            StartRecieve();

        }


        public void SendMessage(string message)
        {
            try
            {
              
                byte[] encodedMessage = Encoding.ASCII.GetBytes(message);
                _client.GetStream().Write(encodedMessage, 0, encodedMessage.Length);


            }
            catch (InvalidOperationException)
            {
            }


        }

        private IAsyncResult _recieveResult;
        private void StartRecieve()
        {

            var recieveData = new byte[1024];
            var networkStream = _client.GetStream();
            _recieveResult = networkStream.BeginRead(recieveData, 0, recieveData.Length, (IAsyncResult result) =>
            {
                try
                {
                    var stream = (NetworkStream)result.AsyncState;
                    int length = stream.EndRead(result);
                    if (length > 0)
                    {
                        string message = Encoding.ASCII.GetString(recieveData, 0, length);
                        OnRecieveMessagesEvent(message);

                    }
                    StartRecieve();
                }
                catch (ObjectDisposedException)
                {
                } 
                catch (IOException)
                {
                } 

            }, networkStream);
        }

    }
}
