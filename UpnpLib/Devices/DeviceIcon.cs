using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpLib.Devices
{
	public class DeviceIcon
	{
		internal DeviceIcon(string mimeType, int width, int height, int depth, string url)
		{
			MimeType = mimeType;
			Width = width;
			Height = height;
			Depth = depth;
			Url = url;
		}

		public string MimeType { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Depth { get; private set; }
		public string Url { get; private set; }

		public override string ToString()
		{
			return string.Format("Width {0}, Height {1}, Depth {2}, MimeType {3}", Width, Height, Depth, MimeType);
		}
	}
}
