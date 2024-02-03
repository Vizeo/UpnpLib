using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UpnpLib.Ssdp;

namespace UpnpLib.Devices.Services
{
	public abstract class ServiceBase
	{
		private HttpClient? _client;
		private SsdpServer _ssdpServer;
		private Device _device;

		protected ServiceBase(SsdpServer ssdpServer, Device device)
		{
			_ssdpServer = ssdpServer;
			_device = device;
		}

		internal void Load(HttpClient client, XmlNode xmlNode, XmlNamespaceManager nameSpaceManager)
		{
			_client = client;
			ServiceType = xmlNode.SelectSingleNode("bk:serviceType", nameSpaceManager)!.InnerText;
			ServiceId = xmlNode.SelectSingleNode("bk:serviceId", nameSpaceManager)!
				.InnerText
				.Replace("urn:upnp-org:serviceId:", string.Empty);

			ControlUri = xmlNode.SelectSingleNode("bk:controlURL", nameSpaceManager)!.InnerText;
		}

		protected ActionBuilder CreateAction(string name) {
			return new ActionBuilder(name, new SoapDispatcher(_ssdpServer, _device, _client!, this));
		}

		public string? ControlUri { get; private set; }
		public string? ServiceId { get; private set; }
		public string? ServiceType { get; private set; }
	}
}
