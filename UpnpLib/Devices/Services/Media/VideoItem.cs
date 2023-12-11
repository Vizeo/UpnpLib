using UpnpLib.Devices.Services.Media.Dlna;
using System.Xml;
using System.Xml.Serialization;

namespace UpnpLib.Devices.Services.Media
{
	public class VideoItem : Item
	{
		public VideoItem()
		{
			UpnpClass = "object.item.videoItem";
		}

		public VideoItem(string Id, string title)
			: this()
		{
			base.Id = Id;
			Title = title;
		}

		[XmlElement(ElementName = "res")]
		public List<Resource>? Resources { get; set; }

		//DC VALUES
		[XmlElement(ElementName = "title", Namespace = "http://purl.org/dc/elements/1.1/")]
		public string? Title { get; set; }

		[XmlElement(ElementName = "DateO", Namespace = "http://purl.org/dc/elements/1.1/")]
		public DateTime? Date { get; set; }

		[XmlElement(ElementName = "description", Namespace = "http://purl.org/dc/elements/1.1/")]
		public string? Description { get; set; }

		[XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
		public string? Creator { get; set; }

		//UPNP Values
		[XmlElement(ElementName = "genre", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public string? Genre { get; set; }

		[XmlElement(ElementName = "artist", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public Person? Artist { get; set; }

		[XmlElement(ElementName = "performer", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public Person? Performer { get; set; }

		[XmlElement(ElementName = "album", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public string? Album { get; set; }

		[XmlElement(ElementName = "track", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public int Track { get; set; }

		[XmlElement(ElementName = "director", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public string? Director { get; set; }

		[XmlElement(ElementName = "actors", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
		public List<string>? Actors { get; set; }
	}

	public class Person
	{
		[XmlAttribute(AttributeName = "role")]
		public string? Role { get; set; }
	}

	public class Resource {
		[XmlText]
		public string? Value { get; set; }

		[XmlIgnore]
		public string? MimeType { get; set; }



		[XmlAttribute(AttributeName = "protocolInfo")]
		public string? ProtocolInfo
		{
			get
			{
				if(MimeType != null)
				{
					return $"http-get:*:{MimeType}:DLNA.ORG_OP=01;DLNA.ORG_FLAGS={DefaultDlnaFlags.DefaultStreaming}";
				}
				else
				{
					return null;
				}
			}
			set
			{
				//Do nothing
			}
		}

		[XmlAttribute(AttributeName = "size")]
		public long Size { get; set; }

		//Working on these
		/// <summary>
		/// Example 320x200
		/// </summary>
		[XmlAttribute(AttributeName = "resolution")]
		public string? Resolution { get; set; }


		[XmlAttribute(AttributeName = "sampleFrequency")]
		public string? SampleFrequency { get; set; }

		[XmlAttribute(AttributeName = "nrAudioChannels")]
		public string? AudioChannels { get; set; }

		//Working on these
		/// <summary>
		/// Example 0:00:10.000
		/// </summary>
		[XmlAttribute(AttributeName = "duration")]
		public string? Duration { get; set; }

		[XmlAttribute(AttributeName = "bitrate")]
		public string? Bitrate { get; set; }
	}
}
