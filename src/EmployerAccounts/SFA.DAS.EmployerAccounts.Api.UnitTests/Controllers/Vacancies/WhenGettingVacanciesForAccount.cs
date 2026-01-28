using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Vacancies;

public class WhenGettingVacanciesForAccount
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancies_Are_Returned_From_The_Handler(
        long accountId,
        GetEmployerVacanciesQueryResponse queryResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]VacanciesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetEmployerVacanciesQuery>(c => c.AccountId == accountId),
            It.IsAny<CancellationToken>())).ReturnsAsync(queryResponse);

        var actual = await controller.GetVacancyData(accountId) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual.Value as GetEmployerVacanciesApiResponse;
        model.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Vacancies_Are_Returned_From_The_Handler_Then_Empty_List_Is_Returned(
        long accountId,
        GetEmployerVacanciesQueryResponse queryResponse,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]VacanciesController controller)
    {
        queryResponse.Vacancies = [];
        mediator.Setup(x => x.Send(It.Is<GetEmployerVacanciesQuery>(c => c.AccountId == accountId),
            It.IsAny<CancellationToken>())).ReturnsAsync(queryResponse);
        
        var actual = await controller.GetVacancyData(accountId) as OkObjectResult;

        actual.Should().NotBeNull();
        var model = actual.Value as GetEmployerVacanciesApiResponse;
        model.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Error_Internal_Server_Error_Is_Returned(
        long accountId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy]VacanciesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetEmployerVacanciesQuery>(c => c.AccountId == accountId),
            It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var actual = await controller.GetVacancyData(accountId) as StatusCodeResult;
        
        actual.Should().NotBeNull();
        actual.StatusCode.Should().Be((int)System.Net.HttpStatusCode.InternalServerError);
    }
}