using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingBrowseByInterestsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Count_Returned(
             GetRoutesListResponse response,
             [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            BrowseByInterestsQueryHandler handler)
        {
            mockCoursesApiClient
                .Setup(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(response);

            var actual = await handler.Handle(new BrowseByInterestsQuery(), CancellationToken.None);

            actual.Routes.Should().BeEquivalentTo(response.Routes);
        }
    }
}