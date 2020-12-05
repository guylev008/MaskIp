using System;

namespace MaskIp
{
    public interface IMaskIps
    {
        string Mask(string i_ip);
    }
    public class MaskIps : IMaskIps
    {
        private string _networkAddress;
        private readonly Lazy<Random> _rand = new Lazy<Random>();

        public string Mask(string i_ip)
        {
            if (string.IsNullOrEmpty(_networkAddress))
            {
                _networkAddress = $"{_rand.Value.Next(1, 256)}.{_rand.Value.Next(1, 256)}.{_rand.Value.Next(1, 256)}";
            }

            return $"{_networkAddress}.{_rand.Value.Next(1, 256)}";
        }
    }
}
