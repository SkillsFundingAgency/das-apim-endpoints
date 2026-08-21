using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class RedirectModel
    {
        public const string ExactMatchType = "Exact";
        public const string PrefixMatchType = "Prefix";

        public string FromPath { get; set; }
        public string ToPath { get; set; }
        public string MatchType { get; set; }
        public bool Permanent { get; set; }

        /// <summary>
        /// Maps published redirect entries, dropping any that are missing a path. Editors work in a free text
        /// field, so anything the consuming site can't act on is discarded here rather than being passed on and
        /// risking the whole list failing to deserialise over one bad entry.
        /// </summary>
        public static List<RedirectModel> BuildFrom(CmsContent content)
        {
            if (content.ContentItemsAreNullOrEmpty())
            {
                return new List<RedirectModel>();
            }

            return content.Items
                .Where(item => !string.IsNullOrWhiteSpace(item?.Fields?.FromPath)
                               && !string.IsNullOrWhiteSpace(item.Fields.ToPath))
                .Select(item => new RedirectModel
                {
                    FromPath = item.Fields.FromPath.Trim(),
                    ToPath = item.Fields.ToPath.Trim(),
                    MatchType = ParseMatchType(item.Fields.MatchType),
                    Permanent = item.Fields.Permanent ?? true
                })
                .ToList();
        }

        /// <summary>
        /// Anything that isn't recognisably a prefix redirect is treated as an exact one, which is both the safer
        /// default and what an editor leaving the field alone will have meant.
        /// </summary>
        private static string ParseMatchType(string matchType)
        {
            return string.Equals(matchType?.Trim(), PrefixMatchType, StringComparison.OrdinalIgnoreCase)
                ? PrefixMatchType
                : ExactMatchType;
        }
    }
}
