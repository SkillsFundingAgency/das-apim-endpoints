using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Configuration;
using SFA.DAS.ApprenticeApp.Models.Contentful;

namespace SFA.DAS.ApprenticeApp.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public class ContentfulEntityResolver : IContentTypeResolver
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>()
        {
            { "page", typeof(Page) },
            { "apprenticeAppCategory", typeof(Page) },
            { "apprenticeAppArticle", typeof(Page) }
        };

        public Type Resolve(string contentTypeId)
        {
            return _types.TryGetValue(contentTypeId, out var type) ? type : null;
        }
    }
}