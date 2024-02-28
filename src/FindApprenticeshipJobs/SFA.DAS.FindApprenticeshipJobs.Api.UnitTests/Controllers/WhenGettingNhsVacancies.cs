using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers;

public class WhenGettingNhsVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Live_Vacancies_Returned_From_Mediator(
        uint mockPageSize,
        uint mockPageNo,
        GetNhsJobsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] NhsVacanciesController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetNhsJobsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(It.IsAny<CancellationToken>()) as ObjectResult;
        var actualValue = actual!.Value as GetLiveVacanciesApiResponse;

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<GetLiveVacanciesApiResponse>();
            actualValue!.Vacancies.Should().BeEquivalentTo(mockQueryResult.NhsVacancies);
            actualValue.PageSize.Should().Be(mockQueryResult.NhsVacancies.Count);
            actualValue.PageNo.Should().Be(1);
            actualValue.TotalLiveVacanciesReturned.Should().Be(mockQueryResult.NhsVacancies.Count);
            actualValue.TotalLiveVacancies.Should().Be(mockQueryResult.NhsVacancies.Count);
            actualValue.TotalPages.Should().Be(1);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        uint mockPageSize,
        uint mockPageNo,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] NhsVacanciesController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetNhsJobsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(It.IsAny<CancellationToken>()) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}