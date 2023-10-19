using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AparRegister.Api.ApiResponses;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;

namespace SFA.DAS.AparRegister.Api.UnitTests.ApiResponses
{
    public class WhenCastingFromMediatorResponseToProvidersApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetProvidersQueryResult source)
        {
            var actual = (ProvidersApiResponse)source;

            actual.Providers.Should().BeEquivalentTo(source.RegisteredProviders);
        }
    }
}