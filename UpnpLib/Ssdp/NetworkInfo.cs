using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UpnpLib.Ssdp
{
    internal static class NetworkInfo
    {
        public static event EventHandler<AddressListChangeEvent>? OnInterfaceDisabled;
        public static event EventHandler<AddressListChangeEvent>? OnNewInterface;

        private static List<IPAddress> _iPAddresses = new List<IPAddress>();

        public static void Start()
        {
            SetupInterfaces();

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    SetupInterfaces();
                }
            });
        }

        private static void SetupInterfaces()
        {
            try
            {
                var currentAddresses = new List<IPAddress>();

                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface networkInterface in interfaces)
                {
                    if (networkInterface.IsReceiveOnly == false &&
                        networkInterface.OperationalStatus == OperationalStatus.Up &&
                        networkInterface.SupportsMulticast == true)
                    {
                        var properties = networkInterface.GetIPProperties();
                        foreach (UnicastIPAddressInformation addressInformation in properties.UnicastAddresses)
                        {
                            if (!currentAddresses.Contains(addressInformation.Address) &&
                                !addressInformation.Address.Equals(IPAddress.IPv6Loopback))
                            {
                                currentAddresses.Add(addressInformation.Address);
                            }
                        }
                    }
                }

                var oldAddresses = _iPAddresses;
                _iPAddresses = currentAddresses;

                CompairLists(currentAddresses, oldAddresses, OnNewInterface!);
                CompairLists(oldAddresses, currentAddresses, OnInterfaceDisabled!);
            }
            catch
            {
                //Nothing for now
            }
        }

        public static IEnumerable<IPAddress> Addresses => _iPAddresses;

        private static void CompairLists(List<IPAddress> a, List<IPAddress> b, EventHandler<AddressListChangeEvent> eventHandler)
        {
            foreach (var address in a)
            {
                if (!b.Contains(address))
                {
                    eventHandler?.Invoke(null, new AddressListChangeEvent(address));
                }
            }
        }

        public class AddressListChangeEvent : EventArgs
        {
            public AddressListChangeEvent(IPAddress iPAddress)
            {
                IPAddress = iPAddress;
            }

            public IPAddress IPAddress { get; }
        }
    }
}
