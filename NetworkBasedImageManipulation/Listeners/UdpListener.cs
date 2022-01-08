using NetworkBasedImageManipulation.Workers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System;

namespace NetworkBasedImageManipulation.Listeners
{
    public class UdpListener
    {
        IPEndPoint mIPepLocal;
        Socket mSockBroadCastReceiver;
        private readonly int CBUFFERSIZE = 1024;

        private System.Windows.Forms.RichTextBox networkLog;
        private VideoWorker videoCodec;
        private SocketAsyncEventArgs _asyncSocket;

        private string ipToListen;
        private int portToListen;

        private bool IsClosed = true;

        public List<EndPoint> mListOfClients;

        public UdpListener(System.Windows.Forms.RichTextBox networkLog)
        {
            mSockBroadCastReceiver = new Socket(
                AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mSockBroadCastReceiver.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.networkLog = networkLog;
            IsClosed = false;
        }

        public VideoWorker GetVideoStream()
        {
            CloseListener();
            videoCodec = new VideoWorker(udpStreamSource: ipToListen, udpStreamIp: portToListen);
            return videoCodec;
        }

        private async void ReceiveCompletedCallback(object Sender, SocketAsyncEventArgs saea)
        {
            // number of received bytes is present in saea.BytesTransferred
            string textReceived = $"{DateTime.Now} : [UdpListener] --> Received {saea.BytesTransferred} bytes from stream";
            //Encoding.ASCII.GetString(saea.Buffer, 0, saea.BytesTransferred);

            // Checking if this thread has access to the object.
            if (!String.IsNullOrEmpty(textReceived))
            {
                if (networkLog.InvokeRequired)
                    networkLog.Invoke(new Action(() => networkLog.AppendText(textReceived + Environment.NewLine)));
                else
                    networkLog.Text += textReceived;
            }
            await Task.Delay(5000);

            // clear buffer before you start another receive operation
            Array.Clear(saea.Buffer, 0, saea.Count);
            (Sender as Socket).ReceiveFromAsync(saea);
        }

        public void ReceiveBroadcast(string ipToListen, int portToListen, bool newStream = false)
        {
            if (newStream)
            {
                this.ipToListen = ipToListen;
                this.portToListen = portToListen;
                mIPepLocal = new IPEndPoint(IPAddress.Any, portToListen);
            }
            if (IsClosed)
                mSockBroadCastReceiver = new Socket(
                    AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                _asyncSocket = new SocketAsyncEventArgs();

                _asyncSocket.SetBuffer(new byte[CBUFFERSIZE], 0, CBUFFERSIZE);
                _asyncSocket.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                _asyncSocket.Completed -= CloseCompletedCallback;
                _asyncSocket.Completed += ReceiveCompletedCallback;

                if (!mSockBroadCastReceiver.IsBound)
                    mSockBroadCastReceiver.Bind(mIPepLocal);

                if (!mSockBroadCastReceiver.ReceiveFromAsync(_asyncSocket))
                {
                    Console.WriteLine("Failed to receive broadcast. " + _asyncSocket.SocketError);
                    ReceiveBroadcast(ipToListen, portToListen, false);
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        private void CloseCompletedCallback(object Sender, SocketAsyncEventArgs saea)
        {
            (Sender as Socket).Shutdown(SocketShutdown.Both);
            (Sender as Socket).Close();
            //saea.Dispose();
        }

        public void CloseListener()
        {
            _asyncSocket.Completed -= ReceiveCompletedCallback;
            _asyncSocket.Completed += CloseCompletedCallback;
            IsClosed = true;
        }

        private void SendCompletedCallback(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("Send operation completed.");
        }
    }
}
