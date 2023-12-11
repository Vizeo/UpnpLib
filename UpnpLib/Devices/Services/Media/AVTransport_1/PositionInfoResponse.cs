using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UpnpLib.Devices.Services.Media.AVTransport_1
{
	public class PositionInfoResponse : ActionResponseBase
	{
		internal PositionInfoResponse(string response)
			: base(response)
		{
			TrackDuration = TimeSpan.Parse(XmlDocument.SelectSingleNode("//TrackDuration")!.InnerText);
			RelTime = TimeSpan.Parse(XmlDocument.SelectSingleNode("//RelTime")!.InnerText);

			XmlNode node = XmlDocument.SelectSingleNode("//TrackMetaData")!;
			if (node.InnerText != string.Empty &&
				node.InnerText != "NOT_IMPLEMENTED")
			{
				XmlDocument metaDataDocument = new XmlDocument();
				metaDataDocument.LoadXml(node.InnerText);

				var nameSpaceManager = new XmlNamespaceManager(metaDataDocument.NameTable);
				nameSpaceManager.AddNamespace("didl", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
				var metadataNode = metaDataDocument.SelectSingleNode("//didl:item", nameSpaceManager)!;
				Item = MetadataFactory.GetItem(metadataNode);
			}
		}

		public Item? Item { get; private set; }
		public TimeSpan TrackDuration { get; internal set; }
		public TimeSpan RelTime { get; internal set; }		
	}
}
