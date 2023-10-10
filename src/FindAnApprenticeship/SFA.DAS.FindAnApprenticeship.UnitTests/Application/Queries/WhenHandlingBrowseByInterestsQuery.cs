using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingBrowseByInterestsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Course_Service_Called_And_Routes_Returned(
            BrowseByInterestsQuery query,
            GetRoutesListResponse response,
            [Frozen] Mock<ICourseService> courseService,
            BrowseByInterestsQueryHandler handler)
        {
            courseService
                .Setup(x => x.GetRoutes())
                .ReturnsAsync(response);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Routes.Should().BeEquivalentTo(response.Routes);
        }
    }
}