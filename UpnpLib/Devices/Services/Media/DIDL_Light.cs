using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UpnpLib.Devices.Services.Media
{
	[XmlRoot(ElementName = "DIDL-Lite", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
	public abstract class DIDL_Light
	{
		protected DIDL_Light() 
		{ 
		}
	}
}
