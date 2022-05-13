using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Service_Called(
            GetCoursesQuery query,
            GetStandardsListResponse standardsListResponse,
            [Frozen] Mock<ICourseService> courseService,
            GetCoursesQueryHandler handler)
        {
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(standardsListResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Standards.Should().BeEquivalentTo(standardsListResponse.Standards);
        }
    }
}