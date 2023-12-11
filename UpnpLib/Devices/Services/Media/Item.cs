
using System.Xml.Serialization;

namespace UpnpLib.Devices.Services.Media
{
	public abstract class Item 
	{
		protected Item()
		{ 
		}

		[XmlAttribute(AttributeName = "id")]
		public string? Id { get; set; }

		[XmlAttribute(AttributeName = "parentID")]
		public string? ParentId { get; set; } = "-1";

		[XmlAttribute(AttributeName = "restricted")]
		public byte Restricted { get; set; }

		[XmlElement(ElementName = "class", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public string? UpnpClass { get; set; }
	}
}
