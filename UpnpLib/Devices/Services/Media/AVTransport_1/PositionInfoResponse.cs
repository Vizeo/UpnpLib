using System.Xml;

namespace UpnpLib.Devices.Services.Media.AVTransport_1
{
	public class PositionInfoResponse : ActionResponseBase
	{
		internal PositionInfoResponse(string response)
			: base(response)
		{
			if(TimeSpan.TryParse(XmlDocument.SelectSingleNode("//TrackDuration")!.InnerText, out var durVal))
			{
                TrackDuration = durVal;
            }

            if (TimeSpan.TryParse(XmlDocument.SelectSingleNode("//RelTime")!.InnerText, out var relVal))
            {
				RelTime = relVal;
            }


            XmlNode node = XmlDocument.SelectSingleNode("//TrackMetaData")!;
			if (node.InnerText != string.Empty &&
				node.InnerText != "NOT_IMPLEMENTED")
			{
				XmlDocument metaDataDocument = new XmlDocument();
				metaDataDocument.LoadXml(node.InnerText);

				Item = MetadataFactory.GetItem(metaDataDocument);
			}
		}

		public Item? Item { get; private set; }
		public TimeSpan TrackDuration { get; internal set; }
		public TimeSpan RelTime { get; internal set; }		
	}
}
