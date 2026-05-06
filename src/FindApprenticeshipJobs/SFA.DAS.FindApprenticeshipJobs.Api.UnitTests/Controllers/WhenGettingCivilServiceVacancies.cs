using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers;
[TestFixture]
internal class WhenGettingCivilServiceVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Live_Vacancies_Returned_From_Mediator(
        GetCivilServiceJobsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CivilServiceVacanciesController controller)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetCivilServiceJobsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await controller.Get(It.IsAny<CancellationToken>()) as ObjectResult;

        var actualValue = actual!.Value as GetLiveVacanciesApiResponse;
        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<GetLiveVacanciesApiResponse>();
            actualValue!.Vacancies.Should().BeEquivalentTo(mockQueryResult.CivilServiceVacancies);
            actualValue.PageSize.Should().Be(mockQueryResult.CivilServiceVacancies.Count);
            actualValue.PageNo.Should().Be(1);
            actualValue.TotalLiveVacanciesReturned.Should().Be(mockQueryResult.CivilServiceVacancies.Count);
            actualValue.TotalLiveVacancies.Should().Be(mockQueryResult.CivilServiceVacancies.Count);
            actualValue.TotalPages.Should().Be(1);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CivilServiceVacanciesController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetCivilServiceJobsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(It.IsAny<CancellationToken>()) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
