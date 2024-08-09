using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Api.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.UnitTests.Controllers
{
    public class WhenGettingProviderPhoneNumbers
    {
        [Test, MoqAutoData]
        public async Task Then_The_providerPhoneNumbers_Are_Returned_From_Mediator(
            GetProviderPhoneNumbersResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller,
            long ukprn)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetProviderPhoneNumbers(ukprn) as ObjectResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var returnedAddresses = actual.Value as ProviderPhoneNumbers;
            returnedAddresses.PhoneNumbers.Should().BeEquivalentTo(queryResult.PhoneNumbers);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller,
            long ukprn)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetProviderPhoneNumbers(ukprn) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
