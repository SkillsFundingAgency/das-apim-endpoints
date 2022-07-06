using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Api.Controllers;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.UnitTests.Controllers
{
    public class WhenGettingVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Handler_Is_Called_And_Data_Returned(
            string vacancyReference,
            GetVacancyQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            mockMediator.Setup(x => x.Send(It.Is<GetVacancyQuery>(c => 
                    c.VacancyReference.Equals(vacancyReference)),
                CancellationToken.None)).ReturnsAsync(queryResult);
            
            var controllerResult = await controller.GetVacancy(vacancyReference) as ObjectResult;
            
            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetVacancyResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetVacancyResponse)queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Result_Then_Not_Found_Result_Returned(
            string vacancyReference,
            GetVacancyQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            queryResult.Vacancy = null;
            mockMediator.Setup(x => x.Send(It.Is<GetVacancyQuery>(c => 
                    c.VacancyReference.Equals(vacancyReference)),
                CancellationToken.None)).ReturnsAsync(queryResult);
            
            var controllerResult = await controller.GetVacancy(vacancyReference) as NotFoundResult;
            
            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Error_Then_Internal_Server_Error_Returned(
            string vacancyReference,
            GetVacancyQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetVacancyQuery>(),
                CancellationToken.None)).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.GetVacancy(vacancyReference) as StatusCodeResult;
            
            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}