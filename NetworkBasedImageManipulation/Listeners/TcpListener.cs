using NetworkBasedImageManipulation.Workers;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;
using System;

namespace NetworkBasedImageManipulation.Listeners
{
    public class TcpListener : BaseListener
    {
        private Thread _socketThread;

        private object padlock = new object();
        private int __backlog = 5;

        public string IpSetup;
        public int PortSetup;
        public bool KeepRunning { get; set; }

        public TcpListener(System.Windows.Forms.RichTextBox networkLog) : base(networkLog)
        {
            mSockBroadCastReceiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mIPepLocal = new IPEndPoint(IPAddress.Any, 23000);
            mSockBroadCastReceiver.Bind(mIPepLocal);
            LogEvent("TcpListener is ready...");
        }

        public void StartListeningForIncomingConnection(IPAddress ipaddr = null, int port = 23000)
        {
            _socketThread = new Thread(SocketThreadFunc);
            _socketThread.Start();
        }

        private void SocketThreadFunc(object state)
        {
            LogEvent("Started new thread...");
            var buffer = new byte[CBUFFERSIZE];
            int numberOfReceivedBytes = 0;
            mSockBroadCastReceiver.Listen(__backlog);
            Socket client = mSockBroadCastReceiver.Accept();
            LogEvent($"Client connected. {client} - IP End Point: {client.RemoteEndPoint}");
            StringBuilder fullCommand = new StringBuilder("");
            CommandWorker inputProcessor = new CommandWorker();
            client.Send(
                Encoding.UTF8.GetBytes(
                    inputProcessor.StateMenu[inputProcessor.CurrentState]));
            string returnMsg;
            while (true)
            {
                numberOfReceivedBytes = client.Receive(buffer);
                string receivedText = Encoding.UTF8.GetString(buffer, 0, numberOfReceivedBytes);

                if (receivedText == "\r\n")
                {
                    LogEvent($"Command sent by client is: {fullCommand}");
                    returnMsg = inputProcessor.ProcessInput(fullCommand.ToString());
                    client.Send(
                        Encoding.UTF8.GetBytes(returnMsg));

                    if (returnMsg.Equals("Good bye !!!"))
                        break;
                    else if (returnMsg.Contains("Setup completed"))
                    {
                        lock (padlock)
                        {
                            IpSetup = inputProcessor.IP.Value;
                            PortSetup = int.Parse(inputProcessor.PORT.Value);
                        }
                    }

                    fullCommand = new StringBuilder("");
                }
                else if (receivedText == "\t")
                {
                    fullCommand = new StringBuilder(
                        inputProcessor.Autocomplete(fullCommand.ToString()));
                    client.Send(Encoding.UTF8.GetBytes(fullCommand.ToString()));                    
                }
                else
                    fullCommand.Append(receivedText);

                Array.Clear(buffer, 0, buffer.Length);
                numberOfReceivedBytes = 0;
            }
            client.Shutdown(SocketShutdown.Both);
            client.Disconnect(false);
            client.Close();
        }
    }
}
