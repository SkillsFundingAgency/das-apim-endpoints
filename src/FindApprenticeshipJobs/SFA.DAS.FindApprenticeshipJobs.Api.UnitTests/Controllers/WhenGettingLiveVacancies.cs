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
        [Frozen] Mock<IMediator> mockMediator,
        uint mockPageSize,
        uint mockPageNo,
        GetLiveVacanciesQueryResult mockQueryResult)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetLiveVacanciesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);
        var sut = new LiveVacanciesController(mockMediator.Object);

        var actual = await sut.Get(mockPageSize, mockPageNo, It.IsAny<CancellationToken>());
        var actualValue = (actual as ObjectResult)!.Value;

        using (new AssertionScope())
        {
            actual.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            actualValue.Should().BeOfType<GetLiveVacanciesApiResponse>();
            actualValue.As<GetLiveVacanciesApiResponse>().Vacancies.Should().BeEquivalentTo(mockQueryResult.Vacancies);
            actualValue.As<GetLiveVacanciesApiResponse>().PageSize.Should().Be(mockQueryResult.PageSize);
            actualValue.As<GetLiveVacanciesApiResponse>().PageNo.Should().Be(mockQueryResult.PageNo);
            actualValue.As<GetLiveVacanciesApiResponse>().TotalLiveVacanciesReturned.Should().Be(mockQueryResult.TotalLiveVacanciesReturned);
            actualValue.As<GetLiveVacanciesApiResponse>().TotalLiveVacancies.Should().Be(mockQueryResult.TotalLiveVacancies);
            actualValue.As<GetLiveVacanciesApiResponse>().TotalPages.Should().Be(mockQueryResult.TotalPages);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        [Frozen] Mock<IMediator> mockMediator,
        uint mockPageSize,
        uint mockPageNo)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetLiveVacanciesQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());
        var sut = new LiveVacanciesController(mockMediator.Object);

        var actual = await sut.Get(mockPageSize, mockPageNo, It.IsAny<CancellationToken>()) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
