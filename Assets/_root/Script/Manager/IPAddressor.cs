using System.Linq;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace _root.Script.Manager
{
    public static class IPAddressor
    {
        [CanBeNull]
        public static string GetLocalIP()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
        }
    }
}