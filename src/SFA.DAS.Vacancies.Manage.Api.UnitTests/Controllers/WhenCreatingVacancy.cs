using System;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class WhenCreatingVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Response_Returned_And_Type_Set_For_Employer(
            
            Guid id,
            CreateVacancyCommandResponse mediatorResponse,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator.Setup(x => 
                    x.Send(It.Is<CreateVacancyCommand>(c => 
                        c.Id.Equals(id)
                        && c.AccountIdentifier.AccountType == AccountType.Employer
                        && c.AccountIdentifier.Ukprn == null
                        && c.AccountIdentifier.AccountPublicHashedId == accountId
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.EmployerAccountId.Equals(accountId.ToUpper())
                        && c.PostVacancyRequestData.OwnerType.Equals(OwnerType.Employer)
                        && c.PostVacancyRequestData.EmployerContact.Name.Equals(request.SubmitterContactDetails.Name)
                        && c.PostVacancyRequestData.EmployerContact.Phone.Equals(request.SubmitterContactDetails.Phone)
                        && c.PostVacancyRequestData.EmployerContact.Email.Equals(request.SubmitterContactDetails.Email)
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
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator.Setup(x => 
                    x.Send(It.Is<CreateVacancyCommand>(c => 
                        c.Id.Equals(id)
                        && c.AccountIdentifier.AccountType == AccountType.Provider
                        && c.AccountIdentifier.Ukprn == ukprn
                        && c.AccountIdentifier.AccountPublicHashedId == null
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.User.Ukprn.Equals(ukprn)
                        && c.PostVacancyRequestData.OwnerType.Equals(OwnerType.Provider)
                        && c.PostVacancyRequestData.ProviderContact.Name.Equals(request.SubmitterContactDetails.Name)
                        && c.PostVacancyRequestData.ProviderContact.Phone.Equals(request.SubmitterContactDetails.Phone)
                        && c.PostVacancyRequestData.ProviderContact.Email.Equals(request.SubmitterContactDetails.Email)
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
            var accountIdentifier = $"Provider-{ukprn}-product";
            
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
            var accountIdentifier = $"Employer-{accountId}-product";
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
        public async Task Then_If_SecurityException_Bad_Request_Is_Returned(
            Guid id,
            string errorContent,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SecurityException("Error"));
            
            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.Forbidden);
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
            var accountIdentifier = $"Employer-{accountId}-product";
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