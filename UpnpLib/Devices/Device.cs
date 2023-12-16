using System.Net;
using System.Xml;
using UpnpLib.Devices.Services;
using UpnpLib.Ssdp;

namespace UpnpLib.Devices
{
    public class Device : IDisposable
	{
		private readonly IServiceFactory _serviceFactory;
		private readonly string _host;
		private readonly string _location;
		private XmlDocument? _xmlDocument;
		private XmlNamespaceManager? _nameSpaceManager;
		private DateTime _expiredDate;
		private HttpClient? _client;

		internal Device(SsdpMessage message, IServiceFactory serviceFactory) {
			_serviceFactory = serviceFactory;
			string maxAge = message["CACHE-CONTROL"]!;
			_expiredDate = DateTime.Now.AddSeconds(Convert.ToInt32(maxAge.Substring(maxAge.IndexOf('=') + 1)));
			
			_location = message["LOCATION"]!;
			_host = _location.Substring(0, _location.IndexOf('/', 8));
			_location = _location.Replace(_host, string.Empty);

			UniqueServiceName = message["USN"]!;
			UniformResourceName = UniqueServiceName.Substring(UniqueServiceName.IndexOf("::") + 2);
			ClientIpAddress = message.ClientIpAddress!;

			Services = new List<ServiceBase>();
			Icons = new List<DeviceIcon>();
		}

		public string UniformResourceName { get; }
		public IPAddress ClientIpAddress { get; }
		public string UniqueServiceName { get; }
		public List<ServiceBase> Services { get; }
		public List<DeviceIcon> Icons { get; }
		public bool IsExpired => _expiredDate < DateTime.Now;
		public string FriendlyName => SelectSingleNode("/bk:root/bk:device/bk:friendlyName")!.InnerText;
		public string ModelName => SelectSingleNode("/bk:root/bk:device/bk:modelName")!.InnerText;

		private void LoadIcons()
		{
			XmlNodeList iconList = SelectNodes("/bk:root/bk:device/bk:iconList/bk:icon")!;
			foreach (XmlNode xmlNode in iconList)
			{
				var mimeType = xmlNode.SelectSingleNode("bk:mimetype", _nameSpaceManager!)!.InnerText;
				var width = Convert.ToInt32(xmlNode.SelectSingleNode("bk:width", _nameSpaceManager!)!.InnerText);
				var height = Convert.ToInt32(xmlNode.SelectSingleNode("bk:height", _nameSpaceManager!)!.InnerText);
				var depth = Convert.ToInt32(xmlNode.SelectSingleNode("bk:depth", _nameSpaceManager!)!.InnerText);
				var url = xmlNode.SelectSingleNode("bk:url", _nameSpaceManager!)!.InnerText;

				Icons.Add(new DeviceIcon(mimeType, width, height, depth, url));
			}
		}

		internal void Update(SsdpMessage message)
		{
			string maxAge = message["CACHE-CONTROL"]!;
			_expiredDate = DateTime.Now.AddSeconds(Convert.ToInt32(maxAge.Substring(maxAge.IndexOf('=') + 1)));
		}

		public async Task Load()
		{
			if (_xmlDocument == null)
			{
				if (_client == null)
				{
					_client = new HttpClient();
					_client.BaseAddress = new Uri(_host);
					_client.DefaultRequestHeaders.Host = _client.BaseAddress.Authority;
				}
				var result = await _client.GetAsync(_location);
				var xml = await result.Content.ReadAsStringAsync();

				_xmlDocument = new XmlDocument();
				_xmlDocument.LoadXml(xml);
				_nameSpaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
				_nameSpaceManager.AddNamespace("bk", "urn:schemas-upnp-org:device-1-0");

				LoadIcons();
				LoadServices();
			}
		}

		protected XmlNode? SelectSingleNode(string xpath)
		{
			LoadedCheck();
			return _xmlDocument!.SelectSingleNode(xpath, _nameSpaceManager!);
		}

		protected XmlNodeList? SelectNodes(string xpath)
		{
			LoadedCheck();
			return _xmlDocument!.SelectNodes(xpath, _nameSpaceManager!)!;
		}

		private void LoadedCheck()
		{
			if(_xmlDocument == null)
			{
				throw new Exception("The device has not been loaded");
			}
		}

		private void LoadServices()
		{
			XmlNodeList serviceList = SelectNodes("/bk:root/bk:device/bk:serviceList/bk:service")!;
			foreach (XmlNode xmlNode in serviceList)
			{
				var serviceBase = _serviceFactory.GetService(xmlNode.SelectSingleNode("bk:serviceType", _nameSpaceManager!)!.InnerText.ToUpper());				
				if (serviceBase != null)
				{
					serviceBase.Load(_client!, xmlNode, _nameSpaceManager!);
					Services.Add(serviceBase);
				}
			}
		}

		public void Dispose()
		{
			_client?.Dispose();
		}
	}
}