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
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Api.Controllers;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.UnitTests.Controllers
{
    public class WhenGettingVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Mediator(
            string accountLegalEntityPublicHashedId,
            int? ukprn,
            int pageNumber, 
            int pageSize,
            GetVacanciesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacanciesQuery>(
                        c=>c.Ukprn.Equals(ukprn) 
                         && c.AccountIdentifier.AccountType == AccountType.Employer
                         && c.AccountIdentifier.AccountPublicHashedId == accountId
                         && c.AccountIdentifier.Ukprn == null
                         && c.PageNumber.Equals(pageNumber)
                         && c.AccountPublicHashedId.Equals(accountId)
                         && c.AccountLegalEntityPublicHashedId.Equals(accountLegalEntityPublicHashedId)
                         && c.PageSize.Equals(pageSize)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetVacancies(accountIdentifier, pageNumber, pageSize, accountLegalEntityPublicHashedId, ukprn) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetVacanciesListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetVacanciesListResponse)mediatorResult);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Mediator_As_Provider_Then_Ukprn_Set_From_Header(
            int ukprn,
            string accountLegalEntityPublicHashedId,
            int pageNumber, 
            int pageSize,
            GetVacanciesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetVacanciesQuery>(
                        c=>c.Ukprn.Equals(ukprn)
                           && c.AccountIdentifier.AccountType == AccountType.Provider
                           && c.AccountIdentifier.AccountPublicHashedId == null
                           && c.AccountIdentifier.Ukprn == ukprn
                           && c.PageNumber.Equals(pageNumber)
                           && c.AccountPublicHashedId == null
                           && c.AccountLegalEntityPublicHashedId.Equals(accountLegalEntityPublicHashedId)
                           && c.PageSize.Equals(pageSize)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetVacancies(accountIdentifier, pageNumber, pageSize, accountLegalEntityPublicHashedId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetVacanciesListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetVacanciesListResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetVacanciesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.GetVacancies(accountIdentifier,1,10) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Test, MoqAutoData]
        public async Task And_SecurityException_Then_Returns_Forbidden(
            string accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetVacanciesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SecurityException());

            var controllerResult = await controller.GetVacancies(accountIdentifier,1,10) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }
    }
}