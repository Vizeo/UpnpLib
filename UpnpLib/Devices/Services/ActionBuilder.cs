using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UpnpLib.Devices.Services
{
	public class ActionBuilder
	{
		private string _name;
		private SoapDispatcher _soapDispatcher;

		internal ActionBuilder(string name, SoapDispatcher soapDispatcher)
		{
			_name = name;
			_soapDispatcher = soapDispatcher;
		}

		public ActionBuilder AddParameter(string name, string value) 
		{
			_soapDispatcher.AddParameter(name, value);
			return this;
		}

		public async Task<DefaultResponse> Invoke()
		{
			var response = await _soapDispatcher.Invoke(_name);
			return new DefaultResponse(response);
		}

		public async Task<T> Invoke<T>()
			where T : ActionResponseBase
		{
			var response = await _soapDispatcher.Invoke(_name);
			return (T)Activator.CreateInstance(typeof(T), response)!;
		}
	}
}
