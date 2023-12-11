using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpLib.Devices.Services.Media
{
	public enum TransportState
	{
		Stopped,
		Playing,
		Transitioning,
		PausedPlayback,
		PausedRecording,
		Recording,
		NoMediaPresent
	}
}
