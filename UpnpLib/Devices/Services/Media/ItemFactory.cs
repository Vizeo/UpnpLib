using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace UpnpLib.Devices.Services.Media
{
	internal static class MetadataFactory
	{
		public static Item GetItem(XmlDocument metaDataDocument)
		{
			var nameSpaceManager = new XmlNamespaceManager(metaDataDocument.NameTable);
			nameSpaceManager.AddNamespace("upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
			nameSpaceManager.AddNamespace("didl", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
			
			var metadataNode = metaDataDocument.SelectSingleNode("//didl:item", nameSpaceManager)!;
			string ic = metadataNode.SelectSingleNode("upnp:class", nameSpaceManager)!.InnerText;

			return ic switch
			{
				_ when ic.Contains("object.item.videoItem") => Deserialize<VideoItem>(metaDataDocument.OuterXml),
				_ => throw new Exception("Dunno")
			};
		}

		private static T Deserialize<T>(string xml)
		{
			var didl = DIDL_Item<T>.Deserialize(xml);
			return didl.Item!;
		}
	}
}
