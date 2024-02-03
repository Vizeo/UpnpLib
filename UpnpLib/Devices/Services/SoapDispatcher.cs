using System.Text;
using System.Xml;
using UpnpLib.Ssdp;

namespace UpnpLib.Devices.Services
{
	internal class SoapDispatcher
	{
		private SsdpServer _ssdpServer;
		private List<KeyValuePair<string, string>> _parameters;
		private HttpClient _client;
		private Device _device;
		private ServiceBase _service;

		internal SoapDispatcher(SsdpServer ssdpServer, Device device, HttpClient client, ServiceBase service)
		{
			_ssdpServer = ssdpServer;
			_device = device;
			_client = client;
			_service = service;
			_parameters = new List<KeyValuePair<string, string>>();
		}

		public void AddParameter(string name, string value)
		{
			_parameters.Add(new KeyValuePair<string, string>(name, value));
		}

		public async Task<string> Invoke(string function)
		{
			string soapBody = GenerateSoapRequest(_service.ServiceType!, function);

			var content = new StringContent(soapBody, Encoding.UTF8, "text/xml");
			content.Headers.Add("Soapaction", $"\"{_service.ServiceType}#{function}\"");

			try
			{
				var result = await _client.PostAsync(_service.ControlUri, content);
				return await result.Content.ReadAsStringAsync();
			}
			catch {
				_ssdpServer.DeviceCrashHandler(_device);
				throw;
			}
		}

		private string GenerateSoapRequest(string service, string function)
		{
			var envelopeNameSpace = "http://schemas.xmlsoap.org/soap/envelope/";

			var xmlDocument = new XmlDocument();
			var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes");
			var rootNode = xmlDocument.CreateElement("s", "Envelope", envelopeNameSpace);
			var xmlAttribute = xmlDocument.CreateAttribute("s", "encodingStyle", envelopeNameSpace);
			xmlAttribute.Value = "http://schemas.xmlsoap.org/soap/encoding/";
			rootNode.Attributes.Append(xmlAttribute);
			xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);
			xmlDocument.AppendChild(rootNode);
			var bodyNode = xmlDocument.CreateElement("s", "Body", envelopeNameSpace);
			rootNode.AppendChild(bodyNode);
			var xmlFunction = xmlDocument.CreateElement("u", function, $"{service}");
			bodyNode.AppendChild(xmlFunction);

			foreach (KeyValuePair<string, string> parameter in _parameters)
			{
				var xmlParameter = xmlDocument.CreateElement(parameter.Key);
				xmlParameter.InnerText = parameter.Value;
				xmlFunction.AppendChild(xmlParameter);
			}

			return xmlDocument.OuterXml;
		}
	}
}
