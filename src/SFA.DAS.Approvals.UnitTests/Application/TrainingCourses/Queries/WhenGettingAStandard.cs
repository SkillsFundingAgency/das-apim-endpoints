using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingAStandard
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_Standard_Is_Returned(
        GetStandardQuery query,
        GetStandardsListItem apiResponse,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        GetStandardQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(x => x.Id == query.CourseCode))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}
