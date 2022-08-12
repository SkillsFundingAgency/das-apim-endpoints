using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Queries
{
    [TestFixture]
    public class GetAllProviderLocationsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            List<ProviderLocationModel> locations,
            GetAllProviderLocationsQuery query,
            GetAllProviderLocationsQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<List<ProviderLocationModel>>(It.Is<GetAllProviderLocationsQuery>(q => q == query))).ReturnsAsync(locations);
            var result = await sut.Handle(query, new CancellationToken());
            result.ProviderLocations.Should().BeEquivalentTo(locations);
        }
    }
}
