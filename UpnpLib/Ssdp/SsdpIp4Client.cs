using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UpnpLib.Ssdp
{
    internal class SsdpIp4Client : SsdpClientBase
    {
        private const string MULTICAST_ADDDRESS = "239.255.255.250";
        private static IPAddress _multicastAddress = IPAddress.Parse(MULTICAST_ADDDRESS);
        private static IPEndPoint _upnpMulticastEndPoint = new IPEndPoint(_multicastAddress, SSDP_PORT);

        private UdpClient? _multicastClient;
        private UdpClient? _localClient;
        private bool _running = true;

        public SsdpIp4Client(IPAddress iPAddress)
            : base(iPAddress)
        {
        }

        public override void Initialize()
        {
            //Listener
            _multicastClient = new UdpClient(AddressFamily.InterNetwork);
            _multicastClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _multicastClient.ExclusiveAddressUse = false;
            _multicastClient.Client.Bind(new IPEndPoint(IPAddress, SSDP_PORT));
            _multicastClient.EnableBroadcast = true;
            _multicastClient.JoinMulticastGroup(_multicastAddress, IPAddress);

            try
            {
                _multicastClient.Client.IOControl(SIO_UDP_CONNRESET, _inValue, _outValue);
            }
            catch /*(SocketException ex)*/
            {
                //Log this
            }

            _localClient = new UdpClient(AddressFamily.InterNetwork);
            _localClient.Client.Bind(new IPEndPoint(IPAddress, 0));
            try
            {
                _localClient.Client.IOControl(SIO_UDP_CONNRESET, _inValue, _outValue);
            }
            catch /*(SocketException ex)*/
            {
                //Log this
            }
        }

        protected override void Receive()
        {
            Task.Run(() => ReceiveLoop(_multicastClient!));
            Task.Run(() => ReceiveLoop(_localClient!));
        }

        private async Task ReceiveLoop(UdpClient udpClient)
        {
            try
            {
                while (_running)
                {
                    var result = await udpClient.ReceiveAsync();
                    if (result.Buffer[result.Buffer.Length - 1] == 10) //The last byte should be 10
                    {
                        ProcessReceivedData(result.Buffer, IPAddress);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Reset();
            }
        }

        protected override void Reset()
        {
#if DEBUG
            Console.WriteLine("Resetting SSDP");
#endif
            try
            {
                _multicastClient!.Dispose();
            }
            catch { }

            try
            {
                _localClient!.Dispose();
            }
            catch { }

            try
            {
                Initialize();
                Receive();
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif                
            }
        }

        public override async Task Broadcast(SsdpMessage ssdpMessage)
        {
            var data = Encoding.UTF8.GetBytes(ssdpMessage.ToString()!);

            try
            {
                _localClient!.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, IPAddress.GetAddressBytes());

                //UPNP requires all broadcast messages to be sent at least twice
                await _localClient.SendAsync(data, data.Length, _upnpMulticastEndPoint);
                await _localClient.SendAsync(data, data.Length, _upnpMulticastEndPoint);
            }
            catch /*(SocketException ex)*/
            {
                //Log this
            }
        }

        public override void Dispose()
        {
            _running = false;
            _multicastClient!.DropMulticastGroup(_multicastAddress);
            _multicastClient.Dispose();
        }
    }
}
