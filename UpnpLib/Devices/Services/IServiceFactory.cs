using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpLib.Devices.Services
{
	public interface IServiceFactory
	{
		ServiceBase? GetService(string service);
	}
}
