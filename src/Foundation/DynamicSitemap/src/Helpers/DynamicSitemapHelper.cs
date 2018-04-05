using Sitecore.Data.Items;
using Sitecore.SharedSource.DynamicSitemap.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.SharedSource.DynamicSitemap.Helpers
{
    /// <summary>
    /// Dynamic Sitemap Helper
    /// </summary>
    public static class DynamicSitemapHelper
    {
        /// <summary>
        /// Replaces host in url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="serverHost">Host</param>
        /// <returns>Prepared url</returns>
        public static String ReplaceHost(string url, string serverHost)
        {
            Uri uri = null;

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return string.Empty;
            }

            UriBuilder builder = new UriBuilder(uri);

            // Clean up Server Host (ex. https://www.sitecore.com to www.sitecore.com)
            serverHost = serverHost.Replace("http://", String.Empty)
                                   .Replace("https://", String.Empty).Trim(new char[] { '/' });

            // Set host information to the defined host name
            builder.Host = serverHost;

            Uri result = builder.Uri;

            url = result.ToString();

            return url;
        }

        /// <summary>
        /// Checks if Item is wildcard
        /// </summary>
        /// <param name="item">Sitecore item</param>
        /// <returns>true if item is wildcard</returns>
        public static bool IsWildcard(Item item)
        {
            return item.Name == "*";
        }

        /// <summary>
        /// Ensures that url has http/https prefix
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="useHttps">Use Https</param>
        /// <returns></returns>
        public static String EnsureHttpPrefix(String url, bool useHttps = false)
        {
            Uri uri = null;
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return url;
            }

            if (!url.StartsWith("http") && !url.StartsWith("https"))
            {
                url = (useHttps ? "https" : "http") + url;
            }
            
            else if (url.StartsWith("http") && !url.StartsWith("https") && useHttps)
            {
                url = url.ReplaceFirst("http", "https");
            }

            return url;
        }

        /// <summary>
        /// Identify the items with a presentation detail
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns></returns>
        public static bool IsPage(Item item)
        {
            var result = false;
            var layoutField = new Data.Fields.LayoutField(item.Fields[FieldIDs.LayoutField]);
            if (!layoutField.InnerField.HasValue || string.IsNullOrEmpty(layoutField.Value)) return false;
            var layout = Layouts.LayoutDefinition.Parse(layoutField.Value);
            foreach (var deviceObj in layout.Devices)
            {
                var device = deviceObj as Layouts.DeviceDefinition;
                if (device == null) return false;
                if (device.Renderings.Count > 0)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
