using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.Campaign.Application.Queries.Standard;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.TrainingCourse
{
    public class WhenGettingATrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Sector_Looked_Up_And_Returns_Standard(
            int _routeId,
            GetStandardQuery query,
            GetStandardListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetStandardQueryHandler handler
        )
        {
            courseService.Setup(x => x.GetRoutes())
                .ReturnsAsync(new GetRoutesListResponse
                {
                    Routes = new List<GetRoutesListItem>
                    {
                        new GetRoutesListItem
                        {
                            Id = _routeId,
                            Name = query.StandardUId
                        }
                    }
                });
            apiClient.Setup(x => x.Get<GetStandardListResponse>(It.Is<Campaign.InnerApi.Requests.GetStandardRequest>(c => c.StandardId == query.StandardUId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Standard.Title.Should().BeEquivalentTo(apiResponse.Title);
        }
    }
}
