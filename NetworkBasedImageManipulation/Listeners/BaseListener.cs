using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBasedImageManipulation.Listeners
{
    public class BaseListener
    {

        protected readonly int CBUFFERSIZE = 1024;
        protected IPEndPoint mIPepLocal;
        protected Socket mSockBroadCastReceiver;

        protected System.Windows.Forms.RichTextBox networkLog;
        protected SocketAsyncEventArgs _asyncSocket;

        protected string ipToListen;
        protected int portToListen;

        protected bool IsClosed = true;

        public List<EndPoint> mListOfClients;

        public BaseListener(System.Windows.Forms.RichTextBox networkLog)
        {
            this.networkLog = networkLog;
            IsClosed = false;
        }

        protected void LogEvent(string message)
        {
            string logLine = $"{DateTime.Now} : [{this.GetType().Name}] --> {message}.{Environment.NewLine}";

            // Checking if this thread has access to the object.
            if (networkLog.InvokeRequired)
                networkLog.Invoke(new Action(() => networkLog.AppendText(logLine)));
            else
                networkLog.Text += logLine;
        }
    }
}
