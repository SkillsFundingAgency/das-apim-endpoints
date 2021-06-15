using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetRegisterDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_And_Location_Data_For_RegisterDemand(
            GetRegisterDemandQuery query,
            GetStandardsListItem apiResponse,
            LocationItem location,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetRegisterDemandQueryHandler handler)
        {
            locationLookupService.Setup(x => x.GetLocationInformation(query.LocationName, 0, 0, true)).ReturnsAsync(location);
            mockApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.StandardId.Equals(query.CourseId))))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Course.Should().BeEquivalentTo(apiResponse);
            result.Location.Should().BeEquivalentTo(location);
        }
    }
}