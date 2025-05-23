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
using SFA.DAS.VacanciesManage.Api.Controllers;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Controllers
{
    public class WhenCreatingVacancy
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
            CreateVacancyCommandResponse mediatorResponse,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var id = Guid.Parse(guid);
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-Product";
            
            mockMediator.Setup(x => 
                    x.Send(It.Is<CreateVacancyCommand>(c => 
                        c.Id.Equals(id)
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.EmployerAccountId.Equals(accountId.ToUpper())
                        && c.IsSandbox.Equals(isSandbox)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request, isSandbox) as IStatusCodeActionResult;

            controllerResult.StatusCode.Should().Be((int) expectedStatusCode);
        }

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
                        && c.AccountIdentifier.AccountHashedId == accountId
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.EmployerAccountId.Equals(accountId.ToUpper())
                        && c.PostVacancyRequestData.OwnerType.Equals(OwnerType.Employer)
                        && c.PostVacancyRequestData.EmployerContact.Name.Equals(request.SubmitterContactDetails.Name)
                        && c.PostVacancyRequestData.EmployerContact.Phone.Equals(request.SubmitterContactDetails.Phone)
                        && c.PostVacancyRequestData.EmployerContact.Email.Equals(request.SubmitterContactDetails.Email)
                        && c.IsSandbox.Equals(false)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as CreatedResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.Created);
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
                        && c.AccountIdentifier.AccountHashedId == null
                        && c.PostVacancyRequestData.Title.Equals(request.Title)
                        && c.PostVacancyRequestData.User.Ukprn.Equals(ukprn)
                        && c.PostVacancyRequestData.OwnerType.Equals(OwnerType.Provider)
                        && c.PostVacancyRequestData.ProviderContact.Name.Equals(request.SubmitterContactDetails.Name)
                        && c.PostVacancyRequestData.ProviderContact.Phone.Equals(request.SubmitterContactDetails.Phone)
                        && c.PostVacancyRequestData.ProviderContact.Email.Equals(request.SubmitterContactDetails.Email)
                        && c.IsSandbox.Equals(false)
                    ), CancellationToken.None))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as CreatedResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.Created);
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
            
            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.Forbidden);
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
        
        [Test]
        [MoqInlineAutoData("ProgrammeId", "standardLarsCode")]
        [MoqInlineAutoData("employerName", "alternativeEmployerName")]
        [MoqInlineAutoData("employerNameOption", "employerNameOption")]
        [MoqInlineAutoData("Address", "address")]
        [MoqInlineAutoData("EmployerLocationInformation", "recruitingNationallyDetails")]
        [MoqInlineAutoData("Addresses[0].Country", "address.postcode")]
        public async Task Then_If_ContentException_Bad_Request_Is_Returned_And_Error_Keys_Mapped_For_One_Locations(
            string fieldName,
            string expectedFieldName,
            Guid id,
            string errorContent,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            // arrange
            request.MultipleAddresses = null;
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,$"{{\"errors\":[{{\"field\":\"{fieldName}\",\"message\":\"An error message\"}}]}}"));
            
            // act
            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as ObjectResult;

            // assert
            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be($"{{\"errors\":[{{\"field\":\"{expectedFieldName}\",\"message\":\"An error message\"}}]}}");
        }
        
        [Test]
        [MoqInlineAutoData("ProgrammeId", "standardLarsCode")]
        [MoqInlineAutoData("employerName", "alternativeEmployerName")]
        [MoqInlineAutoData("employerNameOption", "employerNameOption")]
        [MoqInlineAutoData("Addresses", "multipleAddresses")]
        [MoqInlineAutoData("EmployerLocationInformation", "recruitingNationallyDetails")]
        [MoqInlineAutoData("Addresses[1].Country", "multipleAddresses[1].postcode")]
        public async Task Then_If_ContentException_Bad_Request_Is_Returned_And_Error_Keys_Mapped_For_Many_Locations(
            string fieldName,
            string expectedFieldName,
            Guid id,
            string errorContent,
            CreateVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            // arrange
            request.Address = null;
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<CreateVacancyCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,$"{{\"errors\":[{{\"field\":\"{fieldName}\",\"message\":\"An error message\"}}]}}"));
            
            // act
            var controllerResult = await controller.CreateVacancy(accountIdentifier, id, request) as ObjectResult;

            // assert
            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be($"{{\"errors\":[{{\"field\":\"{expectedFieldName}\",\"message\":\"An error message\"}}]}}");
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