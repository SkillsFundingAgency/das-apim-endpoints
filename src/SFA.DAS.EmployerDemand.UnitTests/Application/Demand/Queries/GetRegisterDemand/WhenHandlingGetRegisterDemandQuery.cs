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
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries.GetRegisterDemand
{
    public class WhenHandlingGetRegisterDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Courses_Api(
            GetRegisterDemandQuery query,
            GetStandardsListItem apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetRegisterDemandQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.IsAny<GetStandardRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Course.Should().BeEquivalentTo(apiResponse);
        }
    }
}