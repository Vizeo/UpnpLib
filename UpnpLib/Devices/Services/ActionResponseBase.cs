using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UpnpLib.Devices.Services
{
	public abstract class ActionResponseBase
	{
		internal ActionResponseBase(string response)
		{
			XmlDocument = new XmlDocument();
			XmlDocument.LoadXml(response);

			HasError = XmlDocument.SelectSingleNode("//control:errorCode", NamespaceManager) != null;
		}

		protected XmlDocument XmlDocument { get; }
		public bool HasError { get; private set; }

		protected virtual XmlNamespaceManager NamespaceManager
		{
			get
			{
				var namespaceManager = new XmlNamespaceManager(XmlDocument.CreateNavigator()!.NameTable);

				namespaceManager.AddNamespace("control", "urn:schemas-upnp-org:control-1-0");
				namespaceManager.AddNamespace("upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
				namespaceManager.AddNamespace("dlna", "urn:schemas-dlna-org:metadata-1-0/");
				namespaceManager.AddNamespace("didl", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
				namespaceManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
				namespaceManager.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");

				return namespaceManager;
			}
		}

		protected string GetVariable(string name)
		{
			return XmlDocument.SelectSingleNode(string.Format("//{0}", name))!.InnerText;
		}

		public string ErrorMessage
		{
			get
			{
				return string.Format("Code {0} - {1}",
					XmlDocument.SelectSingleNode("//control:errorCode", NamespaceManager)!.InnerText,
					XmlDocument.SelectSingleNode("//control:errorDescription", NamespaceManager)!.InnerText);
			}
		}
	}
}
