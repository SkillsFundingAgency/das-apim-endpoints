using System;
using Microsoft.AspNetCore.WebUtilities;
using WireMock.RequestBuilders;

namespace SFA.DAS.EmployerFeedback.FakeApis.Extensions
{
    public static class RequestBuilderUrlExtensions
    {
        public static IRequestBuilder WithPathAndParams(this IRequestBuilder builder, string uriPathAndParams)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            if (uriPathAndParams is null) throw new ArgumentNullException(nameof(uriPathAndParams));

            if (!uriPathAndParams.StartsWith('/'))
                uriPathAndParams = $"/{uriPathAndParams}";

            var uri = new Uri($"http://localhost:8080{uriPathAndParams}");
            var path = uri.AbsolutePath;
            builder = builder.WithPath(path);

            var query = uri.Query;
            if (!string.IsNullOrEmpty(query))
            {
                var parsed = QueryHelpers.ParseQuery(query);
                foreach (var (key, values) in parsed)
                    foreach (var v in values)
                        builder = builder.WithParam(key, v);
            }

            return builder;
        }
    }
}
