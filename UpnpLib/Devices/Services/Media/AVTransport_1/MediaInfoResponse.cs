using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UpnpLib.Devices.Services.Media.AVTransport_1
{
    public class MediaInfoResponse : ActionResponseBase
    {
        internal MediaInfoResponse(string response)
            : base(response)
        {
            XmlNode node = XmlDocument.SelectSingleNode("//CurrentURIMetaData")!;
            if (node.InnerText != string.Empty)
            {
                XmlDocument metaDataDocument = new XmlDocument();
                metaDataDocument.LoadXml(node.InnerText);

                var nameSpaceManager = new XmlNamespaceManager(metaDataDocument.NameTable);
                nameSpaceManager.AddNamespace("didl", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
				Item = MetadataFactory.GetItem(metaDataDocument);
            }

            NumberOfTracks = int.Parse(XmlDocument.SelectSingleNode("//NrTracks")!.InnerText);
            MediaDuration = TimeSpan.Parse(XmlDocument.SelectSingleNode("//MediaDuration")!.InnerText);
            CurrentUri = XmlDocument.SelectSingleNode("//CurrentURI")!.InnerText;
            NextUri = XmlDocument.SelectSingleNode("//NextURI")!.InnerText;
        }

        public int NumberOfTracks { get; private set; }
        public TimeSpan MediaDuration { get; private set; }
        public string? CurrentUri { get; private set; }
        public string? NextUri { get; private set; }
        public Item? Item { get; private set; }
    }
}
