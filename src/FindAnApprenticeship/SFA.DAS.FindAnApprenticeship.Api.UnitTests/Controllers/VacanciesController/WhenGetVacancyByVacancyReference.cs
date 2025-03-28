﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VacanciesController
{
    public class WhenGetVacancyByVacancyReference
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
        string vacancyReference,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IMetrics> metrics,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VacanciesController controller)
        {
            result.ApprenticeshipVacancy.VacancySource = VacancyDataSource.Raa;
            mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.SearchByVacancyReference(vacancyReference) as OkObjectResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as GetApprenticeshipVacancyApiResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Should().BeEquivalentTo((GetApprenticeshipVacancyApiResponse)result);

            mediator.Verify(m => m.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));

            metrics.Verify(x => x.IncreaseVacancyViews(vacancyReference, 1), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            string vacancyReference,
            [Frozen] Mock<IMetrics> metrics,
            [Frozen] Mock<ILogger<Api.Controllers.VacanciesController>> logger,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
            .ThrowsAsync(new Exception());

            var actual = await controller.SearchByVacancyReference(vacancyReference) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));

            metrics.Verify(x => x.IncreaseVacancyViews(vacancyReference, 1), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Null_Is_Returned_Then_Not_Found_Response_Returned(
            string vacancyReference,
            [Frozen] Mock<IMetrics> metrics,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ReturnsAsync((GetApprenticeshipVacancyQueryResult)null);

            var actual = await controller.SearchByVacancyReference(vacancyReference) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            mediator.Verify(m => m.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));

            metrics.Verify(x => x.IncreaseVacancyViews(vacancyReference, 1), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_Is_Non_Raa_Query_Response_Is_Returned(
            string vacancyReference,
            GetApprenticeshipVacancyQueryResult result,
            [Frozen] Mock<IMetrics> metrics,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            result.ApprenticeshipVacancy.VacancySource = VacancyDataSource.Nhs;
            mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.SearchByVacancyReference(vacancyReference) as OkObjectResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as GetApprenticeshipVacancyApiResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Should().BeEquivalentTo((GetApprenticeshipVacancyApiResponse)result);

            mediator.Verify(m => m.Send(It.Is<GetApprenticeshipVacancyQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));

            metrics.Verify(x => x.IncreaseVacancyViews(vacancyReference, 1), Times.Never);
        }
    }
}
