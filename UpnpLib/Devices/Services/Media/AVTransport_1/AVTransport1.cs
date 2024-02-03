using UpnpLib.Ssdp;

namespace UpnpLib.Devices.Services.Media.AVTransport_1
{
	public class AVTransport1 : ServiceBase
	{
		private SsdpServer _ssdpServer;
		private Device _device;

		internal AVTransport1(SsdpServer ssdpServer, Device device)
			:base(ssdpServer, device)
		{
			_ssdpServer = ssdpServer;
			_device = device;
		}

		public async Task<DefaultResponse?> SetAVTransportURI<T>(string currentURI, T item, string instanceID = "0")
			where T : class
		{
			var metaData = new DIDL_Item<T>()
			{
				Item = item
			};

			var ms = metaData.ToString();

			return await CreateAction("SetAVTransportURI")
			.AddParameter("InstanceID", instanceID)
			.AddParameter("CurrentURI", currentURI)
			.AddParameter("CurrentURIMetaData", metaData.ToString())
			.Invoke();
		}

		public async Task<DefaultResponse?> SeekRealTime(TimeSpan unit, string instanceID = "0")
		{
			return await CreateAction("Seek")
			.AddParameter("InstanceID", instanceID)
			.AddParameter("Unit", "REL_TIME")
			.AddParameter("Target", unit.ToString("hh\\:mm\\:ss"))
			.Invoke();
		}

		public async Task<DefaultResponse?> SeekTimeDelta(TimeSpan unit, string instanceID = "0")
		{
			return await CreateAction("Seek")
			.AddParameter("InstanceID", instanceID)
			.AddParameter("Unit", "TIME_DELTA")
			.AddParameter("Target", unit.ToString("hh\\:mm\\:ss"))
			.Invoke();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="track">Starts at 1</param>
		/// <returns></returns>
		public async Task<DefaultResponse?> SeekTrack(int track, string instanceID = "0")
		{
			return await CreateAction("Seek")
			.AddParameter("InstanceID", instanceID)
			.AddParameter("Unit", track.ToString())
			.AddParameter("Target", "TRACK_NR")
			.Invoke();
		}

		public async Task<DefaultResponse?> Play(string speed = "1", string instanceID = "0")
		{
			return await CreateAction("Play")
			.AddParameter("InstanceID", instanceID)
			.AddParameter("Speed", speed)
			.Invoke();
		}

		public async Task<DefaultResponse?> Pause(string instanceID = "0")
		{
			return await CreateAction("Pause")
			.AddParameter("InstanceID", instanceID)
			.Invoke();
		}

		public async Task<DefaultResponse?> Stop(string instanceID = "0")
		{
			return await CreateAction("Stop")
			.AddParameter("InstanceID", instanceID)
			.Invoke();
		}

		public async Task<MediaInfoResponse> GetMediaInfo(string instanceID = "0")
		{
			return await CreateAction("GetMediaInfo")
			.AddParameter("InstanceID", instanceID)
			.Invoke<MediaInfoResponse>();
		}

		public async Task<TransportInfoResponse> GetTransportInfo(string instanceID = "0")
		{
			return await CreateAction("GetTransportInfo")
			.AddParameter("InstanceID", instanceID)
			.Invoke<TransportInfoResponse>();
		}

		public async Task<PositionInfoResponse> GetPositionInfo(string instanceID = "0")
		{
			return await CreateAction("GetPositionInfo")
			.AddParameter("InstanceID", instanceID)
			.Invoke<PositionInfoResponse>();
		}

		public async Task<DefaultResponse?> SetNextAVTransportURI<T>(string currentURI, T item, string instanceID = "0")
			where T : class
		{
			var metaData = new DIDL_Item<T>()
			{
				Item = item
			};

			var ms = metaData.ToString();

			return await CreateAction("SetNextAVTransportURI")
			.AddParameter("InstanceID", instanceID)
			.AddParameter("NextURI", currentURI)
			.AddParameter("NextURIMetaData ", metaData.ToString())
			.Invoke();
		}
	}
}
