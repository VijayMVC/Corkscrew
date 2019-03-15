using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace Corkscrew.SDK.tools
{

    /// <summary>
    /// Global Utility class
    /// </summary>
    public static class Utility
    {

        #region HTTP Functions
        /// <summary>
        /// Get absolute Url from a given UrlFragment
        /// </summary>
        /// <param name="scheme">HTTP or HTTPS scheme (defaults to HTTP)</param>
        /// <param name="dnsHostName">Hostname or domain name of website (defaults to localhost)</param>
        /// <param name="port">Port of website (default is 80)</param>
        /// <param name="urlFragment">The Url fragment to append (default is /)</param>
        /// <returns>Fully qualified Url</returns>
        public static string GetAbsoluteUrl(string scheme = "http", string dnsHostName = "localhost", int port = 80, string urlFragment = "/")
        {

            scheme = SafeString(scheme, "http", removeAtEnd: ":").ToLower();
            dnsHostName = SafeString(dnsHostName, "localhost").ToLower();
            port = ((port > 0) ? port : 80);                                                    // leave it at 80, we will strip it out below.
            urlFragment = SafeString(urlFragment, removeAtStart: "/", removeAtEnd: "/");

            string url =
                string.Format(
                    "{0}://{1}:{2}/{3}",
                    scheme.ToLower(),
                    dnsHostName.ToLower(),
                    port,
                    urlFragment
                );

            // if port is well known for scheme, clean up the port 
            if ((scheme.Equals("http")) && (port == 80))
            {
                url = url.Replace(":80", "");
            }

            if ((scheme.Equals("https")) && (port == 443))
            {
                url = url.Replace(":443", "");
            }

            url = url.Replace("\\", "/");


            return url;
        }

        /// <summary>
        /// Gets the rooted local path address from the given Uri. For example, given 
        /// "http://aquariusos.in/hello/world" we return "/hello/world"
        /// </summary>
        /// <param name="address">The Uri to process</param>
        /// <param name="pathSeperator">[Optional] Path seperator character. Defaults to "/"</param>
        /// <returns>String containing the Uri string</returns>
        public static string GetAbsoluteLocalAddress(this Uri address, string pathSeperator = "/")
        {
            if (address == null)
            {
                return null;
            }

            return SafeString(string.Join(pathSeperator, address.Segments).RemoveExtraSlashes(pathSeperator), expectStart: pathSeperator, removeAtEnd: pathSeperator);
        }

        /// <summary>
        /// Gets the rooted local path parent address from the given Uri. For example, given 
        /// "http://aquariusos.in/hello/world" we return "/hello"
        /// </summary>
        /// <param name="address">The Uri to process</param>
        /// <param name="pathSeparator">[Optional] Path separator character. Defaults to "/"</param>
        /// <returns>String containing the Uri string</returns>
        public static string GetAbsoluteLocalAddressParent(this string address, string pathSeparator = "/")
        {
            if ((string.IsNullOrEmpty(address)) || (address.Equals("/")) || (!address.Contains("/")))
            {
                return null;
            }

            string[] addressArray = address.Split(new string[] { pathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return SafeString(string.Join(pathSeparator, addressArray, 0, addressArray.Length - 1).RemoveExtraSlashes(pathSeparator), expectStart: pathSeparator, removeAtEnd: pathSeparator);
        }

        /// <summary>
        /// Check the given string and determine if it is a valid Dns Hostname. 
        /// Only names are returned as valid strings, IP addresses, etc are treated as invalid hostnames.
        /// </summary>
        /// <param name="hostname">String to check for validity</param>
        /// <returns>True if the string is a valid DNS hostname</returns>
        public static bool IsValidDnsHostname(this string hostname)
        {
            if (string.IsNullOrEmpty(hostname))
            {
                return false;
            }

            UriHostNameType hnt = Uri.CheckHostName(hostname);
            if (hnt == UriHostNameType.Dns)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Encryption and Encoding
        /// <summary>
        /// Get SHA 256 hash of the given string
        /// </summary>
        /// <param name="data">String to get the hash for</param>
        /// <returns>SHA 256 hash or Empty</returns>
        public static string GetSha256Hash(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                SHA256 hasher = SHA256.Create();
                byte[] input = ASCIIEncoding.ASCII.GetBytes(data);
                byte[] hash = hasher.ComputeHash(input);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Calculate the SHA256 hash for [originalString] and compare it with the [hashString] to 
        /// determine if they are the same.
        /// </summary>
        /// <param name="originalString">Plain text string</param>
        /// <param name="hashString">Previously determined SHA256 hash value</param>
        /// <returns>True if the two hashes match. False if not.</returns>
        public static bool IsSha256HashOf(string originalString, string hashString)
        {
            if (string.IsNullOrEmpty(originalString) || string.IsNullOrEmpty(hashString))
            {
                return false;
            }

            return (GetSha256Hash(originalString).Equals(hashString));
        }

        /// <summary>
        /// Get Base64 encoded value of provided string
        /// </summary>
        /// <param name="data">Data to encode</param>
        /// <returns>Base64 encoded value</returns>
        public static string Base64Encode(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
            }

            return null;
        }

        /// <summary>
        /// Decode a given Base64 encoded string
        /// </summary>
        /// <param name="data">String to decode</param>
        /// <returns>The decoded string</returns>
        public static string Base64Decode(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(data));
            }

            return null;
        }

        /// <summary>
        /// Convert a byte array to a UTF-8 string
        /// </summary>
        /// <param name="data">Byte array to convert</param>
        /// <returns>UTF-8 encoded string</returns>
        public static string ConvertToUTF8String(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
        #endregion

        #region Safely Convert to String or other data types
        /// <summary>
        /// Safely wraps a string from an [object] boxed data. Also modifies the result string as per expectations. Removal expectations are performed before Expect expectations. 
        /// For Empty or Null source, none of the expectations are processed except [onEmpty].
        /// </summary>
        /// <param name="source">[object] boxed data</param>
        /// <param name="onEmpty">Value to return if [source] was empty</param>
        /// <param name="expectStart">The string should start with this string, if not prefix</param>
        /// <param name="expectEnd">The string should end with this string, if not suffix</param>
        /// <param name="removeAtStart">If this string occurs at start of string, it is removed</param>
        /// <param name="removeAtEnd">If this string occurs at end of string, it is removed</param>
        /// <returns>String value with all expectations applied, or value of onEmpty</returns>
        public static string SafeString(object source, string onEmpty = "", string expectStart = "", string expectEnd = "", string removeAtStart = "", string removeAtEnd = "")
        {
            string result = onEmpty;

            // sanity!
            if (
                (((!string.IsNullOrEmpty(expectStart)) && (!string.IsNullOrEmpty(removeAtStart))) && (expectStart.Equals(removeAtStart)))
                || (((!string.IsNullOrEmpty(expectEnd)) && (!string.IsNullOrEmpty(removeAtEnd))) && (expectEnd.Equals(removeAtEnd)))
            )
            {
                throw new ArgumentException("expect and remove values cannot be the same.");
            }

            if (expectStart == null) expectStart = string.Empty;
            if (expectEnd == null) expectEnd = string.Empty;
            if (removeAtStart == null) removeAtStart = string.Empty;
            if (removeAtEnd == null) removeAtEnd = string.Empty;

            if ((source == null) || (string.Format("{0}", source).Equals(string.Empty)))
            {
                return onEmpty;
            }

            result = string.Format("{0}", source);

            if ((!string.IsNullOrEmpty(removeAtStart)) && (result.StartsWith(removeAtStart, StringComparison.InvariantCultureIgnoreCase)))
            {
                result = result.Remove(0, removeAtStart.Length);
            }

            if ((!string.IsNullOrEmpty(removeAtEnd)) && (result.EndsWith(removeAtEnd, StringComparison.InvariantCultureIgnoreCase)))
            {
                result = result.Remove((result.Length - removeAtEnd.Length));
            }

            if ((!string.IsNullOrEmpty(expectStart)) && (!result.StartsWith(expectStart, StringComparison.InvariantCultureIgnoreCase)))
            {
                result = string.Format("{0}{1}", expectStart, result);
            }

            if ((!string.IsNullOrEmpty(expectEnd)) && (!result.EndsWith(expectEnd, StringComparison.InvariantCultureIgnoreCase)))
            {
                result = string.Format("{0}{1}", result, expectEnd);
            }

            if (string.IsNullOrEmpty(result))
            {
                return onEmpty;
            }

            return result;
        }

        /// <summary>
        /// Processes a string for emptiness and start/end characters. And checks if string is of a given length.
        /// </summary>
        /// <param name="source">The string to format</param>
        /// <param name="length">Length of the desired string. If set to -1, assigns entire string without length checking</param>
        /// <param name="canBeNullOrEmpty">If set, does not check [source] for NULL or Empty string value.</param>
        /// <param name="throwExceptionOnLength">If true, throws an OverflowException if the length does not match expectation. Else, returns the first [length] characters of the string.</param>
        /// <param name="variableName">Name of the variable related to the string. Used in the thrown exception message</param>
        /// <param name="onEmpty">Value to return if [source] was empty</param>
        /// <param name="expectStart">The string should start with this string, if not prefix</param>
        /// <param name="expectEnd">The string should end with this string, if not suffix</param>
        /// <param name="removeAtStart">If this string occurs at start of string, it is removed</param>
        /// <param name="removeAtEnd">If this string occurs at end of string, it is removed</param>
        /// <returns>String value with all expectations applied, or value of onEmpty</returns>
        public static string SafeString(this string source, int length = -1, bool canBeNullOrEmpty = true, bool throwExceptionOnLength = true, string variableName = "",
            string onEmpty = "", string expectStart = "", string expectEnd = "", string removeAtStart = "", string removeAtEnd = "")
        {

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // DO NOT UNCOMMENT! This is a valid sanity check, but will cause too many runtime exceptions 
            // because of how many times we have said "cannot be null" and expected an empty string set!
            // 
            //if ((!canBeNullOrEmpty) && (string.IsNullOrEmpty(onEmpty)))
            //{
            //    throw new ArgumentException("Cannot set both canBeNullOrEmpty=FALSE and onEmpty=NullOrEmpty at the same time.");
            //}

            string result = SafeString(source, onEmpty, expectStart, expectEnd, removeAtStart, removeAtEnd);
            if ((!canBeNullOrEmpty) && (string.IsNullOrEmpty(result)))
            {
                throw new ArgumentNullException(string.Format("{0} value cannot null or empty.", variableName).Trim());
            }

            // now we have a valid string, validate for length
            if (result != onEmpty)
            {
                if ((length > -1) && (result.Length > length))
                {
                    if (throwExceptionOnLength)
                    {
                        throw new OverflowException(string.Format("{0} value cannot be larger than {1} characters.", variableName, length));
                    }
                    else
                    {
                        result = result.Substring(0, length);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Safely convert object to an Int
        /// </summary>
        /// <param name="source">Object to box</param>
        /// <returns>Int value of object or zero</returns>
        public static int SafeConvertToInt(object source)
        {
            int i = 0;
            if (source != null)
            {
                int.TryParse(SafeString(source, "0"), out i);
            }
            return i;
        }

        /// <summary>
        /// Safely convert object to an Long
        /// </summary>
        /// <param name="source">Object to box</param>
        /// <returns>Long value of object or zero</returns>
        public static long SafeConvertToLong(object source)
        {
            long i = 0L;
            if (source != null)
            {
                long.TryParse(SafeString(source, "0"), out i);
            }
            return i;
        }

        /// <summary>
        /// Safely convert object to a Bool
        /// </summary>
        /// <param name="source">Object to box</param>
        /// <returns>Bool value of object or default(bool)</returns>
        public static bool SafeConvertToBool(object source)
        {
            bool b = default(bool);
            if (source != null)
            {
                source = SafeString(source, "false");   // default(bool) == false !
                b = Convert.ToBoolean(source);
            }
            return b;
        }

        /// <summary>
        /// Safely convert object to a Guid
        /// </summary>
        /// <param name="source">Object to box</param>
        /// <returns>Guid value of object or Empty Guid</returns>
        public static Guid SafeConvertToGuid(object source)
        {
            Guid g = Guid.Empty;

            if (source != null)
            {
                Guid.TryParse(SafeString(source, Guid.Empty.ToString("D")), out g);
            }

            return g;
        }

        /// <summary>
        /// Safely convert object to DateTime
        /// </summary>
        /// <param name="source">Object to box</param>
        /// <returns>DateTime value of object or default(DateTime)</returns>
        public static DateTime SafeConvertToDateTime(object source)
        {
            DateTime dt = default(DateTime);
            if (source != null)
            {
                DateTime.TryParse(SafeString(source, default(DateTime).ToString()), out dt);
            }
            return dt;
        }

        #endregion

        #region List operations
        /// <summary>
        /// Check if the List contains the given string, optionally checking for partial matches while ignoring case
        /// </summary>
        /// <param name="source">List to check</param>
        /// <param name="match">String to search for</param>
        /// <param name="includePartialMatches">If set, also check for partial matches</param>
        /// <returns>True if a match (or partial) was found</returns>
        public static bool ContainsNoCase(this IEnumerable<string> source, string match, bool includePartialMatches = false)
        {
            bool result = false;

            if ((source != null) && (source.Count() > 0))
            {
                if (includePartialMatches)
                {
                    if (source.Count(item => ((!string.IsNullOrEmpty(item)) && (item.IndexOf(match, StringComparison.InvariantCultureIgnoreCase) >= 0))) > 0)
                    {
                        result = true;
                    }
                }
                else if (source.Count(item => ((!string.IsNullOrEmpty(item)) && (item.Equals(match, StringComparison.InvariantCultureIgnoreCase)))) > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region String cleanup

        /// <summary>
        /// From the given string, remove all non alphaumeric characters, optionally substitute dashes instead of spaces
        /// </summary>
        /// <param name="source">String to cleanup</param>
        /// <param name="useDashInsteadOfSpace">If set will cause spaces to be replaced with dashes</param>
        /// <returns>String with alphanumeric characters, optionally with spaces or dashes</returns>
        public static string RemoveNonAlphanumericCharacters(this string source, bool useDashInsteadOfSpace = false)
        {
            string replacement = source;

            if (!string.IsNullOrEmpty(replacement))
            {
                // remove non-alphanumeric characters, replace with space
                replacement = Regex.Replace(replacement.Trim(), @"\W+", " ");

                // remove extra spaces
                replacement = Regex.Replace(replacement.Trim(), @"\s+", " ").Trim();

                // we asked to do so, replace spaces with dashes (useful for Urls)
                if (useDashInsteadOfSpace)
                {
                    replacement = replacement.Replace(" ", "-");
                }
                else
                {
                    replacement = replacement.Replace(" ", "");
                }

                // replace all occurences of a double dash (--) with a single dash (-)
                while (replacement.IndexOf("--") >= 0)
                {
                    replacement = replacement.Replace("--", "-");
                }
            }

            return replacement;
        }

        /// <summary>
        /// Take a column name (database table column name) and 
        /// humanize it so that it is readable. Useful for Grid column headings
        /// </summary>
        /// <param name="columnName">Name of the column from the backend</param>
        /// <returns>Cleaned up string</returns>
        public static string CleanupDatabaseColumnNameForHeading(string columnName)
        {
            StringBuilder title = new StringBuilder();
            bool passedSpace = false;

            foreach (Char c in columnName.ToCharArray())
            {
                if (!Char.IsUpper(c))
                {
                    if (passedSpace && (c != ' '))
                    {
                        title.Append(char.ToUpper(c));
                    }
                    else
                    {
                        title.Append(char.ToLower(c));
                    }

                    passedSpace = (c == ' ');
                }
                else
                {
                    if (passedSpace)
                    {
                        title.Append(c);
                    }
                    else
                    {
                        title.AppendFormat(" {0}", c);
                    }
                    passedSpace = false;
                }
            }

            return title.ToString().Trim();
        }

        /// <summary>
        /// Removes extra "/" characters from a path string
        /// </summary>
        /// <param name="path">Path to process</param>
        /// <param name="slash">The slash character. Defaults to "/"</param>
        /// <returns>Cleaned up string</returns>
        public static string RemoveExtraSlashes(this string path, string slash = "/")
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            string doubleSlash = string.Format("{0}{0}", slash);
            while (path.IndexOf(doubleSlash) >= 0)
            {
                path = path.Replace(doubleSlash, slash);
            }

            // we may have ended up replacing "http://" as "http:/"
            if ((path.Contains(":/")) && (!path.Contains("://")))
            {
                path = path.Replace(":/", "://");
            }

            return path;
        }

        #endregion

        /// <summary>
        /// Returns the human-readable form of the Size (eg: "36 KB")
        /// </summary>
        /// <param name="sizeBytes">The size in bytes</param>
        /// <returns>String containing the human-readable size</returns>
        public static string SizeHuman(long sizeBytes)
        {
            const int ONE_KILOBYTE = 1024;
            const int ONE_MEGABYTE = 1024 * 1024;
            const int ONE_GIGABYTE = 1024 * 1024 * 1024;
            string _human = "0 bytes";

            if (sizeBytes < 1024)
            {
                _human = string.Format("{0} bytes", sizeBytes);
            }
            else if ((sizeBytes >= ONE_KILOBYTE) && (sizeBytes < ONE_MEGABYTE))
            {
                _human = string.Format("{0:F2} KB", (sizeBytes / ONE_KILOBYTE));
            }
            else if ((sizeBytes >= ONE_MEGABYTE) && (sizeBytes < ONE_GIGABYTE))
            {
                _human = string.Format("{0:F2} MB", (sizeBytes / ONE_MEGABYTE));
            }
            else
            {
                _human = string.Format("{0:F2} GB", (sizeBytes / ONE_GIGABYTE));
            }

            return _human;
        }

    }
}
