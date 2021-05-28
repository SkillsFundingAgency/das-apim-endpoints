using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.ApiRequests;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.ApiRequests
{
    public class WhenCastingFromCreateProviderInterestsRequestToMediatorCommand
    {
        [Test, AutoData]
        public void Then_Maps_Values(CreateProviderInterestsRequest source)
        {
            CreateProviderInterestsCommand result = source;

            result.Should().BeEquivalentTo(source);
        }
    }
}