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
public class WhenGettingLiveVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Live_Vacancies_Returned_From_Mediator(
        uint mockPageSize,
        uint mockPageNo,
        GetLiveVacanciesQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LiveVacanciesController sut)
    {
        mockQueryResult.Vacancies.ToList().ForEach(x => x.IsRecruitVacancy = true);
        mockMediator.Setup(x => x.Send(It.IsAny<GetLiveVacanciesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(mockPageSize, mockPageNo, It.IsAny<CancellationToken>()) as ObjectResult;
        var actualValue = actual!.Value as GetLiveVacanciesApiResponse;

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<GetLiveVacanciesApiResponse>();
            actualValue!.Vacancies.Should().BeEquivalentTo(mockQueryResult.Vacancies);
            actualValue.PageSize.Should().Be(mockQueryResult.PageSize);
            actualValue.PageNo.Should().Be(mockQueryResult.PageNo);
            actualValue.TotalLiveVacanciesReturned.Should().Be(mockQueryResult.TotalLiveVacanciesReturned);
            actualValue.TotalLiveVacancies.Should().Be(mockQueryResult.TotalLiveVacancies);
            actualValue.TotalPages.Should().Be(mockQueryResult.TotalPages);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        uint mockPageSize,
        uint mockPageNo,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LiveVacanciesController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetLiveVacanciesQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(mockPageSize, mockPageNo, It.IsAny<CancellationToken>()) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
