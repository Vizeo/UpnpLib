using UpnpLib.Devices;
using UpnpLib.Devices.Services;
using UpnpLib.Devices.Services.Media.AVTransport_1;
using UpnpLib.Ssdp;

namespace UpnpLib
{
	public class KnownServiceFactory : IServiceFactory
	{
		public ServiceBase? GetService(SsdpServer ssdpServer, Device device, string service)
		{
			switch(service)
			{
				case "URN:SCHEMAS-UPNP-ORG:SERVICE:AVTRANSPORT:1":
					return new AVTransport1(ssdpServer, device);
			}

			return null;
		}
	}
}
