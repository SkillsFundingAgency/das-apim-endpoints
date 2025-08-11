using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
[TestFixture]
public class WhenHandlingGetCivilServiceJobsQuery
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsEmptyList_WhenApiResponseIsNotOk(
        [Frozen] Mock<ICivilServiceJobsApiClient> apiClientMock,
        [Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        apiClientMock.Setup(x => x.GetWithResponseCode(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, ""));

        var result = await handler.Handle(new GetCivilServiceJobsQuery(), CancellationToken.None);

        result.CivilServiceVacancies.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_ReturnsEmptyList_WhenApiResponseBodyIsNull(
        [Frozen] Mock<ICivilServiceJobsApiClient> apiClientMock,
        [Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        apiClientMock.Setup(x => x.GetWithResponseCode(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null, HttpStatusCode.OK, ""));

        var result = await handler.Handle(new GetCivilServiceJobsQuery(), CancellationToken.None);

        result.CivilServiceVacancies.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_ReturnsEmptyList_WhenJobsListIsEmpty(
        [Frozen] Mock<ICivilServiceJobsApiClient> apiClientMock,
        [Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var apiResponse = new GetCivilServiceJobsApiResponse { Jobs = []};
        apiClientMock.Setup(x => x.GetWithResponseCode(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(JsonConvert.SerializeObject(apiResponse), HttpStatusCode.OK, ""));

        var result = await handler.Handle(new GetCivilServiceJobsQuery(), CancellationToken.None);

        result.CivilServiceVacancies.Should().BeEmpty();
    }
}
