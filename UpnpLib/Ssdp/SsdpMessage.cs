using System;
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
            var test = _encoder.GetString(data);
			var str = _encoder.GetString(data).AsSpan();

            while (str.Length > 0)
            {
                var index = str.IndexOfAny('\r', '\n');
                var line = str.Slice(0, index)
                    .Trim();
                if (Method == null)
                {
                    Span<char> requestLine = new char[line.Length];
                    line.ToUpper(requestLine, System.Globalization.CultureInfo.CurrentCulture);

                    var methodEnd = requestLine.IndexOf(' ');
                    Method = requestLine.Slice(0, methodEnd).ToString();

                    if (Method.Contains("HTTP"))
                    {
                        Method = string.Empty;
                        Protocol = "HTTP/1.1 200 OK";
                        RequestUri = string.Empty;
                        continue;
                    }

                    var uriEnd = requestLine.LastIndexOf(' ');
                    RequestUri = requestLine.Slice(methodEnd + 1, uriEnd - methodEnd - 1).ToString();

                    Protocol = requestLine.Slice(uriEnd + 1).ToString();
                }
                else
                {
                    //parse header
                    int splitPosition = line.IndexOf(':');
                    if (splitPosition != -1)
                    {
                        Span<char> key = new char[splitPosition];
                        line.Slice(0, splitPosition)
                            .Trim()
                            .ToUpper(key, System.Globalization.CultureInfo.CurrentCulture);

                        var value = line.Slice(splitPosition + 1)
                            .Trim();

                        _headers.Add(new KeyValuePair<string, string>(key.ToString(), value.ToString()));
                    }
                }

                str = str.Slice(index + 2); //Moves past the \r\n
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
