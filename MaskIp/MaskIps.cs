using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var ipKey in ipsList)
            {
                if (_networkAddress.Value.ContainsKey(ipKey.Key) == false)
                {
                    var networkAddress = MaskNetwork();
                    _networkAddress.Value.Add(ipKey.Key, networkAddress);
                }

                var distinctIps = ipKey.Value.Distinct();

                foreach (var ip in distinctIps)
                {
                    var maskedIp = MaskIpAddress(ipKey);
                    i_content = i_content.Replace(ip, maskedIp);
                }
            }

            return i_content;
        }

        private string MaskIpAddress(KeyValuePair<string, List<string>> ipKey)
        {
            return $"{_networkAddress.Value[ipKey.Key]}.{_rand.Value.Next(1, 256)}";
        }

        private string MaskNetwork()
        {
            return $"{_rand.Value.Next(1, 256)}.{_rand.Value.Next(1, 256)}.{_rand.Value.Next(1, 256)}";
        }

        private Dictionary<string, List<string>> GetIpsFromFileContent(string fileContent)
        {
            var ips = Regex.Matches(fileContent, @"(?<network>\d{1,3}\.\d{1,3}\.\d{1,3})\.\d{1,3}");
            var ipsDictionary = new Dictionary<string, List<string>>();
            foreach (Match ip in ips)
            {
                var networkAddress = ip.Groups["network"].Value;
                if (ipsDictionary.ContainsKey(networkAddress))
                    ipsDictionary[networkAddress].Add(ip.Groups[0].Value);
                else
                {
                    ipsDictionary.Add(networkAddress, new List<string> { ip.Groups[0].Value });
                }
            }

            return ipsDictionary;
        }
    }
}
