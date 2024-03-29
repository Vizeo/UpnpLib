﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UpnpLib.Ssdp
{
    public class SsdpMessage
    {
        private static readonly Encoding _encoder = new UTF8Encoding();
        private List<KeyValuePair<string, string>>? _headers;

        public SsdpMessage(string method, string requestUri = "*", string protocol = "HTTP/1.1")
        {
            _headers = new List<KeyValuePair<string, string>>();
            Method = method;
            RequestUri = requestUri;
            Protocol = protocol;
        }

        internal SsdpMessage(byte[] data)
        {
            _headers = new List<KeyValuePair<string, string>>();
			var str = _encoder.GetString(data);

            while (str.Length > 0)
            {
                var index = str.IndexOf("\r\n");
                if (index == -1)
                {
                    break;
                }
                var line = str.Substring(0, index)
                    .Trim();
                if (Method == null)
                {                    
                    var requestLine = line.ToUpper();

                    var methodEnd = requestLine.IndexOf(' ');
                    Method = requestLine.Substring(0, methodEnd).ToString();

                    if (Method.Contains("HTTP"))
                    {
                        Method = string.Empty;
                        Protocol = "HTTP/1.1 200 OK";
                        RequestUri = string.Empty;
                        continue;
                    }

                    var uriEnd = requestLine.LastIndexOf(' ');
                    RequestUri = requestLine.Substring(methodEnd + 1, uriEnd - methodEnd - 1).ToString();

                    Protocol = requestLine.Substring(uriEnd + 1).ToString();
                }
                if(Method == "M-SEARCH")
                {
                    break;
                }
                else
                {
                    //parse header
                    int splitPosition = line.IndexOf(':');
                    if (splitPosition != -1)
                    {                        
                        var key = line.Substring(0, splitPosition)
                            .Trim()
                            .ToUpper();

                        var value = line.Substring(splitPosition + 1)
                            .Trim();

                        _headers.Add(new KeyValuePair<string, string>(key.ToString(), value.ToString()));
                    }
                }

                str = str.Substring(index + 2); //Moves past the \r\n
            }
        }

        public string? this[string key]
        {
            get
            {
                var header = Headers!.FirstOrDefault(k => k.Key == key);
                if (!default(KeyValuePair<string, string>).Equals(header))
                {
                    return header.Value;
                }
                return null;
            }
            set
            {
                var header = Headers!.FirstOrDefault(k => k.Key == key);
                if (!default(KeyValuePair<string, string>).Equals(header))
                {
                    throw new Exception("Header already set");
                }
                else
                {
                    var val = value == null ? string.Empty : value!;
                    header = new KeyValuePair<string, string>(key, val);
                    _headers!.Add(header);
                }
            }
        }

        public string? Method { get; private set; }
        public string? RequestUri { get; private set; }
        public string? Protocol { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> Headers => _headers!;
		public IPAddress? ClientIpAddress { get; internal set; }

		public override string ToString()
        {
            var result = new StringBuilder($"{Method} {RequestUri} {Protocol}\r\n");
            result.Append("HOST: 239.255.255.250:1900\r\n");

            foreach (var header in _headers!)
            {
                result.Append($"{header.Key}: {header.Value}\r\n");
            }
            result.Append($"\r\n"); //The second newline ends the message

            return result.ToString();
        }
    }
}
