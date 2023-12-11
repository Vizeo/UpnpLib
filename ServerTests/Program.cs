using UpnpLib;
using UpnpLib.Ssdp;
using UpnpLib.Devices.Services.Media;

namespace ServerTests
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var SsdpServer = new SsdpServer();
			SsdpServer.Start();

			SsdpServer.DeviceDiscovered = (s, e) =>
			{
				var device = e.Device;
				if (device.UniformResourceName == KnownDevices.MediaRenderer1)
				{
					Task.Run(async () =>
					{
						await device.Load();
						Console.WriteLine($"{device.FriendlyName} {device.ModelName} {device.UniqueServiceName}");

						if (device.UniqueServiceName == "uuid:a5b35fe1-7e27-4fee-9c53-0745a9cfee8b::urn:schemas-upnp-org:device:MediaRenderer:1")
						{
							var videoItem = new VideoItem("Test1", "movie.mp4");

							videoItem.Resources = new List<Resource>();
							var file = "http://192.168.1.234/mediaServer/streamingService?UniqueKey=827528c3-fe29-49c2-a8e6-ff4c6a5a37d2";
							videoItem.Resources.Add(new Resource()
							{
								MimeType = "video/mp4",
								Value = file
							});
							//videoItem.Resources.Add(new Resource()
							//{
							//	//ProtocolInfo = "http-get:*:video/mp4:DLNA.ORG_OP=01;DLNA.ORG_FLAGS=01700000000000000000000000000000",
							//	//AudioChannels = "2",
							//	//Bitrate = "78639",
							//	//SampleFrequency = "48000",
							//	//Duration = "0:00:10.000",
							//	//Resolution = "320x176",
							//	//Size = 788493,
							//	Value = "http://192.168.1.56/mediaServer/streamingService?mediaItemId=2367"
							//});

							var service = device.Services.First() as UpnpLib.Devices.Services.Media.AVTransport_1.AVTransport1;
							var result = await service!.SetAVTransportURI(file, videoItem);
							var result2 = await service!.Play();
						}
					});
				}
			};

			SsdpServer.Search(KnownDevices.MediaRenderer1);

			Console.ReadKey();
			
			foreach(var device in SsdpServer.Devices)
			{
				Console.WriteLine($"{device.FriendlyName} {device.ModelName} {device.UniqueServiceName}");
			}
		}
	}
}
