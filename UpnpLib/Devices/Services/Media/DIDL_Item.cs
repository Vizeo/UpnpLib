using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UpnpLib.Devices.Services.Media
{
	[XmlRoot(ElementName = "DIDL-Lite", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
	public class DIDL_Item<T> : DIDL_Light
	{
		public DIDL_Item()
		{
		}

		[XmlElement(ElementName = "item")]
		public T? Item { get; set; }

		public override string ToString()
		{

			var responseSerializer = new XmlSerializer(GetType());

			var settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.Encoding = Encoding.UTF8;
			settings.OmitXmlDeclaration = true;
			
			using (var stream = new StringWriter())
			{
				using (var writer = XmlWriter.Create(stream, settings))
				{
					responseSerializer.Serialize(writer, this, GetNamespaces());
					return stream.ToString();
				}
			}
		}

		private static XmlSerializerNamespaces GetNamespaces()
		{
			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add(string.Empty, "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
			namespaces.Add("dc", "http://purl.org/dc/elements/1.1/");
			namespaces.Add("upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
			namespaces.Add("dlna", "urn:schemas-dlna-org:metadata-1-0/");
			return namespaces;
		}

		public static DIDL_Item<T> Deserialize(string xml)
		{
			var responseSerializer = new XmlSerializer(typeof(DIDL_Item<T>));
			using (var reader = new StringReader(xml))
			{
				return (DIDL_Item<T>)responseSerializer.Deserialize(reader)!;
			}
		}
	}
}
