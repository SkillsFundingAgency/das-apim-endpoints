using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetVacancyPreview;

public class WhenHandlingGetVacancyPreviewQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Course_Data_Returned(
        GetVacancyPreviewQuery query,
        GetStandardsListItem apiResponse,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        GetVacancyPreviewQueryHandler handler)
    {
        coursesApiClient
            .Setup(x => x.GetWithResponseCode<GetStandardsListItem>(
                It.Is<GetStandardRequest>(c => c.StandardId == query.StandardId)))
            .ReturnsAsync(new ApiResponse<GetStandardsListItem>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Course.Should().BeEquivalentTo(apiResponse);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned_If_Course_NotFound(
        GetVacancyPreviewQuery query,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        GetVacancyPreviewQueryHandler handler)
    {
        coursesApiClient
            .Setup(x => x.GetWithResponseCode<GetStandardsListItem>(
                It.Is<GetStandardRequest>(c => c.StandardId == query.StandardId)))
            .ReturnsAsync(new ApiResponse<GetStandardsListItem>(null!, HttpStatusCode.NotFound, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Course.Should().BeNull();
    }
}