﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SFA.DAS.SharedOuterApi.Helpers
{
    public static class QuerystringHelper
    {
        public static string GetFormattedQuerystring(string ListKey, IEnumerable<string> ListValues, IDictionary<string, string> KeyValues)
        {
            var queryParams = new NameValueCollection();
            if (KeyValues != null && KeyValues.Any())
            {
                foreach (var item in KeyValues)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                        queryParams.Add(item.Key, item.Value);
                }    
            }
            if (!string.IsNullOrEmpty(ListKey) && ListValues != null && ListValues.Any())
            {
                foreach (var value in ListValues)
                    queryParams.Add(ListKey, value);
            }
            return queryParams.ToQueryString();
        }

        public static string ToQueryString(this NameValueCollection nvc)
        {
            if (nvc == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;

                string[] values = nvc.GetValues(key);
                if (values == null) continue;

                foreach (string value in values)
                {
                    sb.Append(sb.Length == 0 ? "?" : "&");
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                }
            }
            return sb.ToString();
        }
    }
}
