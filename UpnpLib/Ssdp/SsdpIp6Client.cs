using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UpnpLib.Ssdp
{
	internal class SsdpIp6Client : SsdpClientBase
	{
		private string _multicastIp;
		private IPAddress _multicastAddress;
		private static IPEndPoint? _upnpMulticastEndPoint;
		private static Task[]? _runningTasks;

		private UdpClient? _multicastClient;
		private UdpClient? _localClient;
		private bool _running = true;
		private CancellationTokenSource? _cancellationTokenSource;

		public SsdpIp6Client(string multicastIp, IPAddress iPAddress)
			: base(iPAddress)
		{
			_multicastIp = multicastIp;
			_multicastAddress = IPAddress.Parse(multicastIp);
			_upnpMulticastEndPoint = new IPEndPoint(_multicastAddress, SSDP_PORT);
		}

		public override void Initialize()
		{
			_multicastClient = new UdpClient(AddressFamily.InterNetworkV6);
			_multicastClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			_multicastClient.ExclusiveAddressUse = false;
			_multicastClient.Client.Bind(new IPEndPoint(IPAddress.IPv6Any, SSDP_PORT));
			_multicastClient.EnableBroadcast = true;
			_multicastClient.JoinMulticastGroup(IPAddress.Parse(_multicastIp), 2);

			try
			{
				_multicastClient.Client.IOControl(SIO_UDP_CONNRESET, _inValue, _outValue);
			}
			catch /*(SocketException ex)*/
			{
				//Log this
			}

			_localClient = new UdpClient(AddressFamily.InterNetworkV6);
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

		public override async Task Broadcast(SsdpMessage ssdpMessage)
		{
			var data = Encoding.UTF8.GetBytes(ssdpMessage.ToString()!);

			try
			{
				_localClient!.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastInterface, BitConverter.GetBytes((int)IPAddress.ScopeId));

				//UPNP requires all broadcast messages to be sent at least twice
				await _localClient.SendAsync(data, data.Length, _upnpMulticastEndPoint);
				await _localClient.SendAsync(data, data.Length, _upnpMulticastEndPoint);
			}
			catch /*(SocketException ex)*/
			{
				//Log this
			}
		}

		protected override void Reset()
		{
#if DEBUG
			Console.WriteLine("Resetting SSDP");
#endif

			try
			{
				_cancellationTokenSource = new CancellationTokenSource();
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

		protected override void Receive()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_runningTasks = new Task[] {
				Task.Run(() => ReceiveLoop(_multicastClient!)),
				Task.Run(() => ReceiveLoop(_localClient!))
			};
		}

		private async Task ReceiveLoop(UdpClient udpClient)
		{
			try
			{
				while (_running)
				{
					var result = await udpClient.ReceiveAsync(_cancellationTokenSource!.Token);
					if (result.Buffer[result.Buffer.Length - 1] == 10) //The last byte should be 10
					{
						ProcessReceivedData(result.Buffer, IPAddress);
					}
				}
			}
			catch (Exception e)
			{
#if DEBUG
				Console.WriteLine(e);
#endif
				_cancellationTokenSource?.Cancel();
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

				if (_runningTasks != null)
				{
					await Task.WhenAll(_runningTasks);
				}

				Reset();
			}
		}

		public override void Dispose()
		{
			_running = false;
			_cancellationTokenSource?.Cancel();
			_multicastClient?.DropMulticastGroup(_multicastAddress);
			_multicastClient?.Dispose();
		}
	}
}
