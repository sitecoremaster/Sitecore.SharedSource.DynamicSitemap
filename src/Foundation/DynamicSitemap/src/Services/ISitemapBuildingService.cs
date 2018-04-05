using System.Collections.Generic;
using Sitecore.SharedSource.DynamicSitemap.Model;

namespace Sitecore.SharedSource.DynamicSitemap.Services
{
    public interface ISitemapBuildingService
    {
        string BuildSitemap(SitemapSiteConfiguration sitemapSiteConfiguration, IEnumerable<UrlElement> elements);
    }
}