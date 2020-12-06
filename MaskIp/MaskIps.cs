using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MaskIp
{
    public interface IMaskIps
    {
        string Mask(string i_content);
    }
    public class MaskIps : IMaskIps
    {
        private Lazy<Dictionary<string, string>> _networkAddress = new Lazy<Dictionary<string, string>>();
        private readonly Lazy<Random> _rand = new Lazy<Random>();

        public string Mask(string i_content)
        {
            var ipsList = GetIpsFromFileContent(i_content);
            var mask = _rand.Value.Next(1, 256);
            foreach (var ip in ipsList)
            {
                var ipAddress = IPAddress.Parse(ip);
                var maskedIp = MaskIpAddress(ipAddress, mask);
                i_content = i_content.Replace(ip, maskedIp);
            }

            return i_content;
        }

        private string MaskIpAddress(IPAddress ip, int mask)
        {
            var ipAddressBytes = ip.GetAddressBytes();
            var maskedIpAddressValues = ipAddressBytes.Select(ipAddressByte => ipAddressByte ^ mask);
            return string.Join('.', maskedIpAddressValues);
        }

        private IEnumerable<string> GetIpsFromFileContent(string fileContent)
        {
            return Regex.Matches(fileContent, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
                .Select(m => m.Value)
                .Distinct();
        }
    }
}
