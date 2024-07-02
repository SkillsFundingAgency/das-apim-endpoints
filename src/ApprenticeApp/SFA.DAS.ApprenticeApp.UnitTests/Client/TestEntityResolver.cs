using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Configuration;

namespace SFA.DAS.ApprenticeApp.UnitTests.Client
{
    [ExcludeFromCodeCoverage]
    public class TestEntityResolver : IContentTypeResolver
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public Type Resolve(string contentTypeId)
        {
            return _types.TryGetValue(contentTypeId, out var type) ? type : null;
        }
    }
}