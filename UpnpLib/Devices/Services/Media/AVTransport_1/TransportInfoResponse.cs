using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UpnpLib.Devices.Services.Media.AVTransport_1
{
	public class TransportInfoResponse : ActionResponseBase
	{
		internal TransportInfoResponse(string response)
			: base(response)
		{
			SetTransportState(XmlDocument.SelectSingleNode("//CurrentTransportState")!.InnerText);
			CurrentTransportStatus = XmlDocument.SelectSingleNode("//CurrentTransportStatus")!.InnerText;
			CurrentSpeed = Convert.ToInt32(XmlDocument.SelectSingleNode("//CurrentSpeed")!.InnerText);
		}

		public TransportState TransportState { get; private set; }
		public string CurrentTransportStatus { get; private set; }
		public int CurrentSpeed { get; private set; }

		private void SetTransportState(string state)
		{
			switch (state)
			{
				case "STOPPED":
					TransportState = TransportState.Stopped;
					break;
				case "PLAYING":
					TransportState = TransportState.Playing;
					break;
				case "TRANSITIONING":
					TransportState = TransportState.Transitioning;
					break;
				case "PAUSED_PLAYBACK":
					TransportState = TransportState.PausedPlayback;
					break;
				case "PAUSED_RECORDING":
					TransportState = TransportState.PausedRecording;
					break;
				case "NO_MEDIA_PRESENT":
					TransportState = TransportState.NoMediaPresent;
					break;
			}
		}
	}
}
