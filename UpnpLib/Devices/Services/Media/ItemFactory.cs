using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace UpnpLib.Devices.Services.Media
{
	internal static class MetadataFactory
	{
		public static Item GetItem(XmlNode node)
		{
			var nameSpaceManager = new XmlNamespaceManager(node.OwnerDocument!.NameTable);
			nameSpaceManager.AddNamespace("upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
			string ic = node.SelectSingleNode("upnp:class", nameSpaceManager)!.InnerText;

			Type type = ic switch
			{
				_ when ic.Contains("object.item.videoItem") => typeof(Item),
				_ => typeof(Item)
			};

			throw new NotImplementedException(); //Not finished
		}

		private static T Deserialize<T>(string xml)
		{
			var didl = DIDL_Item<T>.Deserialize(xml);
			return didl.Item!;
		}
	}
}
