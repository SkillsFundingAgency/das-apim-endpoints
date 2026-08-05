using System.Collections.Specialized;
using System.Text;

namespace SFA.DAS.Aodp.Application.Extensions
{
    public static class UriExtension
    {
        public static Uri AttachParameters(this Uri uri, NameValueCollection parameters)
        {
            var sb = new StringBuilder();
            string str = "?";
            for (int i = 0; i < parameters.Count; i++)
            {
                sb.Append(str + parameters.AllKeys[i] + "=" + parameters[i]);
                str = "&";
            }

            return new Uri(uri + sb.ToString());
        }

        public static string AttachParameters(this string uri, NameValueCollection parameters)
        {
            var sb = new StringBuilder();
            string str = "?";
            for (int i = 0; i < parameters.Count; i++)
            {
                sb.Append(str + parameters.AllKeys[i] + "=" + parameters[i]);
                str = "&";
            }

            return uri + sb.ToString();
        }

        public static string AttachMultiValueParameters(this string uri, NameValueCollection parameters)
        {
            var sb = new StringBuilder();
            var first = true;

            foreach (string key in parameters.AllKeys)
            {
                var values = parameters.GetValues(key);
                if (values == null) continue;

                foreach (var value in values)
                {
                    sb.Append(first ? "?" : "&");
                    first = false;

                    sb.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
                }
            }

            return uri + sb.ToString();
        }
    }
}
