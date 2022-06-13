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
    public class WhenGettingTraineeshipVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Mediator_As_Employer_With_Filter(
            SearchTraineeshipVacancyRequest request,
            GetTraineeshipVacanciesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            request.FilterBySubscription = true;
            request.Sort = VacancySort.ExpectedStartDateAsc;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTraineeshipVacanciesQuery>(
                        c=>c.Ukprn.Equals(request.Ukprn) 
                         && c.AccountIdentifier.AccountType == AccountType.Employer
                         && c.AccountIdentifier.AccountHashedId == accountId
                         && c.AccountIdentifier.Ukprn == null
                         && c.PageNumber.Equals(request.PageNumber)
                         && c.AccountPublicHashedId.Equals(accountId)
                         && c.AccountLegalEntityPublicHashedId.Equals(request.AccountLegalEntityPublicHashedId)
                         && c.PageSize.Equals(request.PageSize)
                         && c.Lat.Equals(request.Lat)
                         && c.Lon.Equals(request.Lon)
                         && c.Routes.Equals(request.Routes)
                         && c.Sort.Equals(request.Sort.ToString())
                         && c.DistanceInMiles.Equals(request.DistanceInMiles)
                         && c.NationWideOnly.Equals(request.NationWideOnly)
                         && c.RouteId.Equals(request.RouteId)
                         && c.PostedInLastNumberOfDays.Equals(request.PostedInLastNumberOfDays)
                         ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetTraineeshipVacancies(accountIdentifier, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTraineeshipVacanciesListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetTraineeshipVacanciesListResponse)mediatorResult);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Mediator_As_Employer_With_No_AccountId_Filter_When_FilterBySubscription_Is_False(
            SearchTraineeshipVacancyRequest request,
            GetTraineeshipVacanciesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountId = "ABC123";
            var accountIdentifier = $"Employer-{accountId}-product";
            request.FilterBySubscription = false;
            request.Sort = VacancySort.ExpectedStartDateAsc;
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTraineeshipVacanciesQuery>(
                        c=>c.Ukprn.Equals(request.Ukprn) 
                         && c.AccountIdentifier.AccountType == AccountType.Employer
                         && c.AccountIdentifier.AccountHashedId == accountId
                         && c.AccountIdentifier.Ukprn == null
                         && c.PageNumber.Equals(request.PageNumber)
                         && c.AccountPublicHashedId == null
                         && c.AccountLegalEntityPublicHashedId.Equals(request.AccountLegalEntityPublicHashedId)
                         && c.PageSize.Equals(request.PageSize)
                         && c.Lat.Equals(request.Lat)
                         && c.Lon.Equals(request.Lon)
                         && c.Routes.Equals(request.Routes)
                         && c.Sort.Equals(request.Sort.ToString())
                         && c.DistanceInMiles.Equals(request.DistanceInMiles)
                         && c.NationWideOnly.Equals(request.NationWideOnly)
                         && c.RouteId.Equals(request.RouteId)
                         && c.PostedInLastNumberOfDays.Equals(request.PostedInLastNumberOfDays)
                         ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetTraineeshipVacancies(accountIdentifier, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTraineeshipVacanciesListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetTraineeshipVacanciesListResponse)mediatorResult);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Mediator_As_Provider_Then_Ukprn_Set_From_Header(
            SearchTraineeshipVacancyRequest request,
            int ukprn,
            GetTraineeshipVacanciesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            request.FilterBySubscription = true;
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTraineeshipVacanciesQuery>(
                        c=>c.Ukprn.Equals(ukprn)
                           && c.AccountIdentifier.AccountType == AccountType.Provider
                           && c.AccountIdentifier.AccountHashedId == null
                           && c.AccountIdentifier.Ukprn == ukprn
                           && c.PageNumber.Equals(request.PageNumber)
                           && c.AccountPublicHashedId == null
                           && c.AccountLegalEntityPublicHashedId.Equals(request.AccountLegalEntityPublicHashedId)
                           && c.PageSize.Equals(request.PageSize)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetTraineeshipVacancies(accountIdentifier, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTraineeshipVacanciesListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetTraineeshipVacanciesListResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Vacancies_From_Mediator_As_Provider_And_Uses_Request_Ukprn_If_FilterBySubscription_Is_false(
            SearchTraineeshipVacancyRequest request,
            int ukprn,
            GetTraineeshipVacanciesQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            request.FilterBySubscription = false;
            var accountIdentifier = $"Provider-{ukprn}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetTraineeshipVacanciesQuery>(
                        c=>c.Ukprn.Equals(request.Ukprn)
                           && c.AccountIdentifier.AccountType == AccountType.Provider
                           && c.AccountIdentifier.AccountHashedId == null
                           && c.AccountIdentifier.Ukprn == ukprn
                           && c.PageNumber.Equals(request.PageNumber)
                           && c.AccountPublicHashedId == null
                           && c.AccountLegalEntityPublicHashedId.Equals(request.AccountLegalEntityPublicHashedId)
                           && c.PageSize.Equals(request.PageSize)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetTraineeshipVacancies(accountIdentifier, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetTraineeshipVacanciesListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetTraineeshipVacanciesListResponse)mediatorResult);
        }
        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string accountId,
            SearchTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTraineeshipVacanciesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.GetTraineeshipVacancies(accountIdentifier,request) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Test, MoqAutoData]
        public async Task And_SecurityException_Then_Returns_Forbidden(
            string accountId,
            SearchTraineeshipVacancyRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] VacancyController controller)
        {
            var accountIdentifier = $"Employer-{accountId}-product";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTraineeshipVacanciesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new SecurityException());

            var controllerResult = await controller.GetTraineeshipVacancies(accountIdentifier,request) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }
    }
}