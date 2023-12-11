using UpnpLib.Devices.Services;
using UpnpLib.Devices.Services.Media.AVTransport_1;

namespace UpnpLib
{
	public class KnownServiceFactory : IServiceFactory
	{
		public ServiceBase? GetService(string service)
		{
			switch(service)
			{
				case "URN:SCHEMAS-UPNP-ORG:SERVICE:AVTRANSPORT:1":
					return new AVTransport1();
			}

			return null;
		}
	}
}
