using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.InternalApi
{
    public class TestInternalApiConfiguration : IInternalApiConfiguration
    {
        public virtual string Url { get; set; }
        public virtual string Identifier { get; set; }
    }

    public class TestTokenPassThroughApiConfiguration : ITokenPassThroughApiConfiguration
    {
        public virtual string Url { get; set; }
        public virtual string Identifier { get; set; }
        public virtual string BearerTokenSigningKey { get; set; }
    }
}