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

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    public class WhenGettingLiveVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_Live_Vacancies_Returned_From_Mediator(
            long vacancyReference,
            GetLiveVacancyQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LiveVacanciesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetLiveVacancyQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockQueryResult);

            var actual = await sut.GetLiveVacancy(vacancyReference, It.IsAny<CancellationToken>()) as ObjectResult;

            using (new AssertionScope())
            {
                actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
                actual.Value.Should().BeOfType<GetLiveVacanciesApiResponse.LiveVacancy>();
                actual.Value.Should().BeEquivalentTo((GetLiveVacanciesApiResponse.LiveVacancy)mockQueryResult.LiveVacancy);
            }
        }
    }
}
