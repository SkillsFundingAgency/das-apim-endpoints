using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.ProviderInterest.Commands
{
    public class WhenCastingCommandToApiRequestData
    {
        [Test, AutoData]
        public void Then_Maps_Fields(CreateProviderInterestsCommand source)
        {
            var result = (CreateProviderInterestsData) source;

            result.Should().BeEquivalentTo(source, options => options
                .ExcludingMissingMembers());
        }
    }
}