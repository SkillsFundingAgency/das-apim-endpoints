using System;
using System.Net;
using System.Security;
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
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Api.Controllers;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Controllers
{
    public class WhenCreatingTraineeshipVacancy
    {
        [Test]
        [MoqInlineAutoData("00000000-0000-0000-0000-000000000000", true, HttpStatusCode.BadRequest)]
        [MoqInlineAutoData("00000000-0000-0000-0000-000000000000", false, HttpStatusCode.Created)]
        [MoqInlineAutoData("11111111-1111-1111-1111-111111111111", true, HttpStatusCode.TooManyRequests)]
        [MoqInlineAutoData("11111111-1111-1111-1111-111111111111", false, HttpStatusCode.Created)]
        [MoqInlineAutoData("d849c8fd-e393-4ab4-beac-09f4504ddd77", true, HttpStatusCode.Created)]
        public async Task Sandbox_Special_Case_Guids(
            string guid,
            bool isSandbox,
            HttpStatusCode expectedStatusCode,
            CreateTraineeshipVacancyCommandResponse mediatorResponse,
            CreateTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller,
            int ukprn)
        {
            var id = Guid.Parse(guid);
            var accountIdentifier = $"Provider-{ukprn}-product";

            mockMediator.Setup(x =>
                    x.Send(It.Is<CreateTraineeshipVacancyCommand>(c =>
                        c.Id.Equals(id)
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                         && c.AccountIdentifier.Ukprn == ukprn
                        && c.IsSandbox.Equals(isSandbox)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request, isSandbox) as IStatusCodeActionResult;

            controllerResult.StatusCode.Should().Be((int)expectedStatusCode);
        }
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Response_Returned_And_Type_Set_For_Provider(
            int ukprn,
            Guid id,
            CreateTraineeshipVacancyCommandResponse mediatorResponse,
            CreateTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator.Setup(x =>
                    x.Send(It.Is<CreateTraineeshipVacancyCommand>(c =>
                        c.Id.Equals(id)
                        && c.AccountIdentifier.AccountType == AccountType.Provider
                        && c.AccountIdentifier.Ukprn == ukprn
                        && c.AccountIdentifier.AccountHashedId == null
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.User.Ukprn.Equals(ukprn)
                        && c.IsSandbox.Equals(false)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request) as CreatedResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Value.Should().BeEquivalentTo(new { mediatorResponse.VacancyReference });
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Account_Is_Employer_For_The_AccountIdentifier_Then_Forbidden_Returned(
            Guid id,
            CreateTraineeshipVacancyRequest request,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-Product";

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request) as StatusCodeResult; 

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Format_Is_Not_Correct_For_The_AccountIdentifier_Then_Forbidden_Returned(
            long accountIdentifier,
            Guid id,
            CreateTraineeshipVacancyRequest request,
            [Greedy] VacancyController controller)
        {

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier.ToString(), id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Ukprn_Is_Not_Correct_For_The_AccountIdentifier_Then_Forbidden_Returned(
            Guid id,
            CreateTraineeshipVacancyRequest request,
            [Greedy] VacancyController controller)
        {
            var ukprn = "ABC123";
            var accountIdentifier = $"Provider-{ukprn}-product";

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request) as BadRequestObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            controllerResult.Value.Should().BeEquivalentTo("Account Identifier is not in the correct format.");
        }

        [Test, MoqAutoData]
        public async Task Then_If_ContentException_Bad_Request_Is_Returned_And_Error_Keys_Mapped(
            int ukprn,
            Guid id,
            string errorContent,
            CreateTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateTraineeshipVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest, @"{""errors"":[{""field"":""RouteId"",""message"":""Training programme a does not exist.""},{""field"":""employerName"",""message"":""Employer name is not in the correct format.""},{""field"":""employerNameOption"",""message"":""Invalid employer name option.""}]}"));

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(@"{""errors"":[{""field"":""RouteId"",""message"":""Training programme a does not exist.""},{""field"":""alternativeEmployerName"",""message"":""Employer name is not in the correct format.""},{""field"":""employerNameOption"",""message"":""Invalid employer name option.""}]}");
        }

        [Test, MoqAutoData]
        public async Task Then_If_SecurityException_Then_Forbidden_Returned(
             int ukprn,
            Guid id,
            string errorContent,
            CreateTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateTraineeshipVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SecurityException("Error"));

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Exception_Internal_Server_Error_Is_Returned(
             int ukprn,
            Guid id,
            string errorContent,
            CreateTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateTraineeshipVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var controllerResult = await controller.CreateTraineeshipVacancy(accountIdentifier, id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}