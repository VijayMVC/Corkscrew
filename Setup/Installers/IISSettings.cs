using Corkscrew.SDK.tools;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Contains settings for one IIS app
    /// </summary>
    public class IISSettings
    {

        #region Properties

        /// <summary>
        /// Name of the app. Will be used to name the Website and Application Pool
        /// </summary>
        public string AppName
        {
            get { return _appName; }
            set
            {
                _appName = value.SafeString(80, false, true, nameof(AppName), expectStart: "Corkscrew - ");
            }
        }
        private string _appName = "Corkscrew - ";

        /// <summary>
        /// IP address to bind to. Set to IPAddress.Any to bind to all IP addresses
        /// </summary>
        public IPAddress BindingHostAddress
        {
            get { return _bindingHostAddress; }
            set
            {
                if (value != IPAddress.Any)
                {
                    // must be IPv4 or IPv6
                    if ((value.AddressFamily != AddressFamily.InterNetwork) && (value.AddressFamily != AddressFamily.InterNetworkV6))
                    {
                        throw new ArgumentException("IP address must be of IPv4 or IPv6 type only.");
                    }

                    // check that IP address is of the local machine
                    string valueString = value.ToString();
                    bool isLocalAddress = false;
                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach(IPAddress addr in host.AddressList.AsEnumerable().Where(h => IsAcceptableIPAddress(h)).OrderBy(h => h.AddressFamily.ToString()))
                    {
                        if (valueString.Equals(addr.ToString()))
                        {
                            isLocalAddress = true;
                            break;
                        }
                    }

                    if (! isLocalAddress)
                    {
                        throw new ArgumentException("IP address must belong to local machine.");
                    }
                }

                _bindingHostAddress = value;
            }
        }
        private IPAddress _bindingHostAddress = IPAddress.Any;

        /// <summary>
        /// Port to bind to.
        /// </summary>
        public int BindingPort
        {
            get { return _bindingPort; }
            set
            {
                if ((value <= 0) || (value >= 65535))
                {
                    throw new ArgumentOutOfRangeException("Port must be between 0 and 65535");
                }

                _bindingPort = value;
            }
        }
        private int _bindingPort = 80;

        /// <summary>
        /// Hostname to set in binding
        /// </summary>
        public string BindingHostname
        {
            get { return _bindingHostname; }
            set
            {
                // RFC1035 defines a 255 char limit for hostnames
                _bindingHostname = value.SafeString(255, true, onEmpty: string.Empty);
            }
        }
        private string _bindingHostname = string.Empty;

        /// <summary>
        /// If set, configures a HTTPS (SSL) binding. Will require certificate configuration also.
        /// </summary>
        public bool IsSSLBinding
        {
            get;
            set;
        }

        /// <summary>
        /// SSL certificate. Must be non-expired and valid.
        /// </summary>
        public X509Certificate2 SSLCertificate
        {
            get
            {
                // dont return certificate if IsSSLBinding is false
                return (IsSSLBinding ? _sslCertificate : null);
            }
            set
            {
                if ((! IsSSLBinding) && (value != null))
                {
                    throw new ArgumentException("Cannot set SSL certificate. Set IsSSLBinding property first.");
                }

                if (value != null)
                {
                    if (!value.Verify())
                    {
                        throw new ArgumentException("Certificate has expired or is otherwise invalid.");
                    }

                    // ensure certificate purpose is correct
                    bool isValidSsl = false;
                    foreach (X509Extension xtn in value.Extensions)
                    {
                        X509EnhancedKeyUsageExtension extn = xtn as X509EnhancedKeyUsageExtension;
                        if (extn != null)
                        {
                            foreach (Oid use in extn.EnhancedKeyUsages)
                            {
                                if (use.FriendlyName.Equals("Server Authentication"))
                                {
                                    isValidSsl = true;
                                    break;
                                }
                            }
                        }

                        if (isValidSsl)
                        {
                            break;
                        }
                    }

                    if (!isValidSsl)
                    {
                        throw new Exception("Certificate is not a valid SSL certificate. At least one Intended Usage must be set to \"Server Authentication\".");
                    }

                    // ensure certificate matches hostname
                    if (!string.IsNullOrEmpty(BindingHostname))
                    {
                        if ((!value.GetNameInfo(X509NameType.SimpleName, false).Equals(BindingHostname, StringComparison.InvariantCultureIgnoreCase))
                            && (!value.GetNameInfo(X509NameType.DnsName, false).Equals(BindingHostname, StringComparison.InvariantCultureIgnoreCase))
                            && (!value.GetNameInfo(X509NameType.DnsFromAlternativeName, false).Contains(BindingHostname))
                            && (!value.GetNameInfo(X509NameType.UrlName, false).Contains(BindingHostname))
                        )
                        {
                            throw new ArgumentException("Certificate does not match " + BindingHostname);
                        }
                    }
                }

                // if we are still here ... 
                _sslCertificate = value;
            }
        }
        private X509Certificate2 _sslCertificate = null;

        /// <summary>
        /// Full path to the disk folder containing the web application
        /// </summary>
        public string WebApplicationFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the website to create
        /// </summary>
        public string WebsiteName
        {
            get { return _webName; }
            set
            {
                _webName = value.SafeString(80, false, true, nameof(AppName), expectStart: "Corkscrew - Web Site - ");
            }
        }
        private string _webName = "Corkscrew - Web Site - ";



        #endregion

        #region Constructors

        /// <summary>
        /// Blank constructor
        /// </summary>
        public IISSettings() { }

        /// <summary>
        /// Returns a basic default binding object. Can be used to initialize any UI, etc
        /// </summary>
        /// <returns>IISSettings populated with usable default settings</returns>
        public static IISSettings GetDefaultBinding()
        {
            string g = Guid.NewGuid().ToString("n");

            return new IISSettings()
            {
                AppName = "Corkscrew - Application for Website - " + g,
                WebsiteName = "Corkscrew - Website - " + Guid.NewGuid().ToString("n"),
                WebApplicationFolder = g,
                BindingHostAddress = IPAddress.Any,
                BindingPort = 80,
                BindingHostname = string.Empty,     // no hostname
                IsSSLBinding = false,
                SSLCertificate = null
            };
        }

        #endregion

        #region Methods

        private bool IsAcceptableIPAddress(IPAddress addr)
        {
            return (
                (addr.AddressFamily.HasFlag(AddressFamily.InterNetwork) || addr.AddressFamily.HasFlag(AddressFamily.InterNetworkV6))
                && (!addr.IsIPv6Multicast) && (!addr.IsIPv6Teredo) && (!addr.IsIPv6LinkLocal)
            );
        }

        /// <summary>
        /// Returns if the BindingPort value is the default port for the protocol (SSLBinding).
        /// </summary>
        /// <returns>True if port is a defaults</returns>
        public bool IsProtocolDefaultPort()
        {
            return (
                ((!IsSSLBinding) && (BindingPort == 80))
                || (IsSSLBinding && (BindingPort == 443))
            );
        }

        /// <summary>
        /// Get Http or Https as per binding
        /// </summary>
        /// <returns>String containing "http" or "https"</returns>
        public string GetProtocolName()
        {
            return (IsSSLBinding ? "https" : "http");
        }

        /// <summary>
        /// Generates a random number between 1 and 65534. Does NOT check if the port is in use!
        /// </summary>
        /// <returns>Integer. Random number between 1 and 65534</returns>
        public static int GenerateRandomPort()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            return random.Next(1, 65534);
        }

        /// <summary>
        /// Returns the binding info string for IIS
        /// </summary>
        /// <returns>String of the format "ipaddress:port:hostname"</returns>
        public string GetBindingInfo()
        {
            string ipAddress = BindingHostAddress.ToString();
            return string.Format("{0}:{1}:{2}", ((BindingHostAddress == IPAddress.Any) ? "*" : ipAddress), BindingPort, BindingHostname);
        }

        #endregion
    }
}
