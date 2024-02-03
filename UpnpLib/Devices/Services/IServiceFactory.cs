using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpnpLib.Ssdp;

namespace UpnpLib.Devices.Services
{
	public interface IServiceFactory
	{
		ServiceBase? GetService(SsdpServer ssdpServer, Device device, string service);
	}
}
