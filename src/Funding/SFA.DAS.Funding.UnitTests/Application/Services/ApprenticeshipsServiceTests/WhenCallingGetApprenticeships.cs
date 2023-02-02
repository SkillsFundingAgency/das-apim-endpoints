using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Application.Services;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.InnerApi.Requests.Apprenticeships;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.UnitTests.Application.Services.FundingProviderEarningsServiceTests
{
    public class WhenCallingGetApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Apprenticeships(
            long ukprn,
            IEnumerable<ApprenticeshipDto> apiResponse,
            [Frozen] Mock<IApprenticeshipsApiClient<ApprenticeshipsConfiguration>> client,
            ApprenticeshipsService service
        )
        {
            client.Setup(x =>
                    x.GetAll<ApprenticeshipDto>(
                        It.Is<GetApprenticeshipsRequest>(c => c.GetAllUrl.Contains(ukprn.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetAll(ukprn);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}