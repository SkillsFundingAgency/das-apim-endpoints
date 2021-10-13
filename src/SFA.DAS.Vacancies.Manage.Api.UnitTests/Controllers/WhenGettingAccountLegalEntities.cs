﻿using System;
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
using SFA.DAS.Vacancies.Manage.Api.Controllers;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer;
using SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Controllers
{
    public class WhenGettingAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_If_Employer_Account_Identifier_Then_Get_Account_Legal_Entities_Called(
            string encodedAccountId,
            GetLegalEntitiesForEmployerResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountLegalEntitiesController controller)
        {
            var accountIdentifier = $"Employer|{encodedAccountId}";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLegalEntitiesForEmployerQuery>(c=>c.EncodedAccountId.Equals(encodedAccountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(accountIdentifier) as ObjectResult;
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountLegalEntitiesListResponse;
            model!.Should().BeEquivalentTo((GetAccountLegalEntitiesListResponse)mediatorResult);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Provider_Account_Identifier_Then_Get_Account_Legal_Entities_Called(
            int ukprn,
            GetProviderAccountLegalEntitiesQueryResponse mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountLegalEntitiesController controller)
        {
            var accountIdentifier = $"Provider|{ukprn}";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetProviderAccountLegalEntitiesQuery>(c=>c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList(accountIdentifier) as ObjectResult;
            
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountLegalEntitiesListResponse;
            model!.Should().BeEquivalentTo((GetAccountLegalEntitiesListResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Recognised_AccountIdentifier_Type_Then_Forbidden_Returned(
            string accountType,
            string identifier,
            [Greedy] AccountLegalEntitiesController controller)
        {
            var accountIdentifier = $"{accountType}|{identifier}";
            
            var controllerResult = await controller.GetList(accountIdentifier) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_If_Not_Recognised_AccountIdentifier_Format_Then_BadRequest_Returned(
            string identifier,
            [Greedy] AccountLegalEntitiesController controller)
        {
            var accountIdentifier = $"{identifier}";
            
            var controllerResult = await controller.GetList(accountIdentifier) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
        
        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_InternalServerError(
            string encodedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountLegalEntitiesController controller)
        {
            var accountIdentifier = $"Employer|{encodedAccountId}";
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetLegalEntitiesForEmployerQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<Exception>();

            var controllerResult = await controller.GetList(accountIdentifier) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}