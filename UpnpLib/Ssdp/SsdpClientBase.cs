using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Buffers.Text;
using System.Text;
using System.Threading.Tasks;

namespace UpnpLib.Ssdp
{
    internal abstract class SsdpClientBase : IDisposable
    {
        protected const int SIO_UDP_CONNRESET = -1744830452;
        protected static byte[] _inValue = new byte[] { 0, 0, 0, 0 };     // == false
        protected static byte[] _outValue = new byte[] { 0, 0, 0, 0 };    // initialize to 0
        protected const int SSDP_PORT = 1900;

        private static string _searchMethod = "M-SEARCH";

        public event EventHandler<DataReceivedEvent>? OnDataReceived;

        protected SsdpClientBase(IPAddress ipAddress)
        {
            IPAddress = ipAddress;
        }

        public void Start()
        {
            Initialize();
            Receive();
        }

        public IPAddress IPAddress { get; }
        public abstract void Initialize();
        protected abstract void Reset();
        protected abstract void Receive();
        public abstract void Dispose();

        protected void ProcessReceivedData(byte[] data, IPAddress clientIpAddress)
        {
            try
            {
                var ssdpRequest = new SsdpMessage(data);
				ssdpRequest.ClientIpAddress = clientIpAddress;

				if (ssdpRequest.Method != _searchMethod)
                {
                    OnDataReceived?.Invoke(this, new DataReceivedEvent(ssdpRequest));
                }
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //Reset();
            }
        }

        public abstract Task Broadcast(SsdpMessage ssdpMessage);

        public class DataReceivedEvent : EventArgs
        {
            public DataReceivedEvent(SsdpMessage ssdpMessage)
            {
                SsdpMessage = ssdpMessage;
            }

            public SsdpMessage SsdpMessage { get; }
        }
    }
}