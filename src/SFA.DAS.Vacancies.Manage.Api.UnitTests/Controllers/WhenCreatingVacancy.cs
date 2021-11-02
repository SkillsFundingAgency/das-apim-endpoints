using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Api.Controllers;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Controllers
{
    public class WhenCreatingVacancy
    {
        [Test]
        [MoqInlineAutoData("00000000-0000-0000-0000-000000000000", true, HttpStatusCode.AlreadyReported)]
        [MoqInlineAutoData("00000000-0000-0000-0000-000000000000", false, HttpStatusCode.Created)]
        [MoqInlineAutoData("11111111-1111-1111-1111-111111111111", true, HttpStatusCode.TooManyRequests)]
        [MoqInlineAutoData("11111111-1111-1111-1111-111111111111", false, HttpStatusCode.Created)]
        public async Task Sandbox_Special_Case_Guids(
            string guid,
            bool addSandbox,
            HttpStatusCode expectedStatusCode,
            CreateVacancyCommandResponse mediatorResponse,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var id = Guid.Parse(guid);
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}";
            if (addSandbox) accountIdentifier += "-sandbox";
            
            mockMediator.Setup(x => 
                    x.Send(It.Is<CreateVacancyCommand>(c => 
                        c.Id.Equals(id)
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.EmployerAccountId.Equals(accountId.ToUpper())
                        && c.IsSandbox.Equals(addSandbox)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as IStatusCodeActionResult;

            controllerResult.StatusCode.Should().Be((int) expectedStatusCode);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Response_Returned_And_Type_Set_For_Employer(
            Guid id,
            bool addSandbox,
            CreateVacancyCommandResponse mediatorResponse,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}";
            if (addSandbox) accountIdentifier += "-sandbox";
            mockMediator.Setup(x => 
                    x.Send(It.Is<CreateVacancyCommand>(c => 
                        c.Id.Equals(id)
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.EmployerAccountId.Equals(accountId.ToUpper())
                        && c.IsSandbox.Equals(addSandbox)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as CreatedResult;

            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.Created);
            controllerResult.Value.Should().BeEquivalentTo(new { mediatorResponse.VacancyReference });
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Response_Returned_And_Type_Set_For_Provider(
            int ukprn,
            Guid id,
            CreateVacancyCommandResponse mediatorResponse,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            request.User = null;
            var accountIdentifier = $"Provider-{ukprn}";
            mockMediator.Setup(x => 
                    x.Send(It.Is<CreateVacancyCommand>(c => 
                        c.Id.Equals(id)
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.User.Ukprn.Equals(ukprn)
                        && c.PostVacancyRequestData.User.Email == null
                        && c.IsSandbox.Equals(false)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as CreatedResult;

            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.Created);
            controllerResult.Value.Should().BeEquivalentTo(new { mediatorResponse.VacancyReference });
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Format_Is_Not_Correct_For_The_AccountIdentifier_Then_Forbidden_Returned(
            long accountIdentifier,
            Guid id,
            CreateVacancyRequest request,
            [Greedy] VacancyController controller)
        {
            var controllerResult = await controller.CreateVacancy(accountIdentifier.ToString(), id, request) as StatusCodeResult;
            
            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.Forbidden);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Ukprn_Is_Not_Correct_For_The_AccountIdentifier_Then_Forbidden_Returned(
            Guid id,
            CreateVacancyRequest request,
            [Greedy] VacancyController controller)
        {
            var ukprn = "ABC123";
            var accountIdentifier = $"Provider-{ukprn}";
            
            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as BadRequestObjectResult;
            
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            controllerResult.Value.Should().BeEquivalentTo("Account Identifier is not in the correct format.");
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_ContentException_Bad_Request_Is_Returned(
            Guid id,
            string errorContent,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Exception_Internal_Server_Error_Is_Returned(
            Guid id,
            string errorContent,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as StatusCodeResult;
            
            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}