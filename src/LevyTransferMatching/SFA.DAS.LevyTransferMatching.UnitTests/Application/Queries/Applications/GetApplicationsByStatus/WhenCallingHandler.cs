using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationsByStatus;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications.GetApplicationsByStatus
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_GetApplications_by_status(
            GetApplicationsByStatusQuery query,
            GetApplicationsResponse apiResponse,
            [Frozen] Mock<ILevyTransferMatchingService> _ltmService,
            GetApplicationsByStatusQueryHandler handler)
        {
            _ltmService
                .Setup(client => client.GetApplications(It.IsAny<GetApplicationsRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Applications.Should().NotBeEmpty();
            result.Applications.Count().Should().Be(apiResponse.Applications.Count());

            result.Applications.First().Id.Should().Be(apiResponse.Applications.First().Id);
            result.Applications.First().PledgeId.Should().Be(apiResponse.Applications.First().PledgeId);
            result.Applications.First().Amount.Should().Be(apiResponse.Applications.First().Amount);
            result.Applications.First().JobRole.Should().Be(apiResponse.Applications.First().StandardTitle);
        }
    }
}

