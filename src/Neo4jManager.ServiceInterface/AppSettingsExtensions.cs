using System.Collections.Generic;
using Neo4jManager.ServiceModel;
using ServiceStack.Configuration;

namespace Neo4jManager.ServiceInterface
{
    public static class AppSettingsExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static IEnumerable<Version> Neo4jVersions(this IAppSettings appSettings)
        {
            return appSettings.Get<IEnumerable<Version>>(AppSettingsKeys.Versions);
        }
    }
}
