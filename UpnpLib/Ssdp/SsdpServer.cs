using System.Collections.Concurrent;
using UpnpLib.Devices;
using UpnpLib.Devices.Services;

namespace UpnpLib.Ssdp
{
	public class SsdpServer
	{
		private readonly ConcurrentDictionary<string, Device> _devices = new ConcurrentDictionary<string, Device>();
		private readonly List<SsdpClientBase> _clients = new List<SsdpClientBase>();
		private readonly object _locket = new object();
		private readonly IServiceFactory _serviceFactory;
		private bool _running = false;
		public event EventHandler<DeviceChangeArg>? DeviceDiscovered;
		public event EventHandler<DeviceChangeArg>? DeviceOffline;

		public SsdpServer(IServiceFactory serviceFactory)
		{
			_serviceFactory = serviceFactory;

			Task.Run(() =>
			{
				while (_running)
				{
					var devices = _devices.Values;
					foreach (var device in devices)
					{
						if (device.IsExpired)
						{
							lock (_devices)
							{
								if (_devices.TryRemove(device.UniqueServiceName, out _))
								{
									DeviceOffline?.Invoke(this, new DeviceChangeArg(device));
								}
							}
						}
					}
				}
			});
		}

		public SsdpServer()
			: this(new KnownServiceFactory())
		{
		}

		public void Start()
		{
			_running = true;
			NetworkInfo.Start();

			lock (_locket)
			{
				foreach (var address in NetworkInfo.Addresses)
				{
					SsdpClientBase? client = null;
					if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
					{
						client = new SsdpIp4Client(address);
					}
					else
					{
						if (address.IsIPv6LinkLocal)
						{
							client = new SsdpIp6Client("FF02::C", address);
						}
						else if (address.IsIPv6SiteLocal)
						{
							client = new SsdpIp6Client("FF05::C", address);
						}
						else
						{
							continue;
						}
					}

					client!.OnDataReceived += Received;
					client!.Start();
					_clients.Add(client);
				}
			}
		}

		public void Stop()
		{
			_running = true;
			foreach (var client in _clients)
			{
				client.Dispose();
			}
		}

		public void Search(string criteria)
		{
			var message = new SsdpMessage("M-SEARCH");
			message["MAN"] = "\"ssdp:discover\"";
			message["ST"] = criteria;
			message["MX"] = "2";

			lock (_locket)
			{
				foreach (var client in _clients)
				{
					try
					{
						client.Broadcast(message);
					}
					catch { }
				}
			}
		}

		private void Received(object? sender, SsdpClientBase.DataReceivedEvent e)
		{
			if (e.SsdpMessage.Method == "NOTIFY")
			{
				if (e.SsdpMessage["NTS"] == "ssdp:byebye")
				{
					ProcessDeviceOffline(e.SsdpMessage);
					return;
				}
			}
			ProcessDeviceAddOrMerge(e.SsdpMessage);
		}

		private void ProcessDeviceAddOrMerge(SsdpMessage ssdpMessage)
		{
			lock (_devices)
			{
				var usn = ssdpMessage["USN"]!;
				if (_devices.TryGetValue(usn, out var device))
				{
					device.Update(ssdpMessage);
				}
				else
				{
					if (ssdpMessage["LOCATION"] != null)
					{
						device = new Device(this, ssdpMessage, _serviceFactory);
						_devices.TryAdd(usn, device);
						DeviceDiscovered?.Invoke(this, new DeviceChangeArg(device));
					}
				}
			}
		}

		private void ProcessDeviceOffline(SsdpMessage ssdpMessage)
		{
			lock (_devices)
			{
				var usn = ssdpMessage["USN"]!;
				if (_devices.TryRemove(usn, out var device))
				{
					DeviceOffline?.Invoke(this, new DeviceChangeArg(device));
				}
			}
		}

		internal void DeviceCrashHandler(Device device)
		{
			if (_devices.TryRemove(device.UniqueServiceName, out _))
			{
				DeviceOffline?.Invoke(this, new DeviceChangeArg(device));
			}
		}

		public IEnumerable<Device> Devices
		{
			get
			{
				lock (_devices)
				{
					return _devices.Values;
				}
			}
		}
	}

	public class DeviceChangeArg : EventArgs
	{
		internal DeviceChangeArg(Device device)
		{
			Device = device;
		}

		public Device Device { get; }
	}
}
