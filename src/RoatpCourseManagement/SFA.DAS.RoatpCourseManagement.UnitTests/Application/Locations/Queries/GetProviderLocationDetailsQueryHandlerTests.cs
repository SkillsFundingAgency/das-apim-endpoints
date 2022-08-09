using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Queries
{
    [TestFixture]
    public class GetProviderLocationDetailsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            ProviderLocationModel location,
            GetProviderLocationDetailsQuery query,
            GetProviderLocationDetailsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<ProviderLocationModel>(It.Is<GetProviderLocationDetailsQuery>(q => q == query))).ReturnsAsync(location);
            var result = await sut.Handle(query, new CancellationToken());
            result.ProviderLocation.Should().BeEquivalentTo(location);
        }
    }
}
