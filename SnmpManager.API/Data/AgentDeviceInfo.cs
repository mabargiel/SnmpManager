using System;

namespace SnmpManager.API.Data
{
    public class AgentDeviceInfo : IEquatable<AgentDeviceInfo>
    {
        public string IpAddress { get; }
        public string SupportedVersion { get; }

        public AgentDeviceInfo(string ipAddress, string supportedVersion)
        {
            IpAddress = ipAddress;
            SupportedVersion = supportedVersion;
        }

        public bool Equals(AgentDeviceInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(IpAddress, other.IpAddress) && string.Equals(SupportedVersion, other.SupportedVersion);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((AgentDeviceInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((IpAddress != null ? IpAddress.GetHashCode() : 0) * 397) ^ (SupportedVersion != null ? SupportedVersion.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"IpAddress: {IpAddress}, SupportedVersion: {SupportedVersion}";
        }
    }
}