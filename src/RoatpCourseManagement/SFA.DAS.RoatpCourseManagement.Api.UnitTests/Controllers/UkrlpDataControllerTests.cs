using AutoFixture.NUnit3;using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Services;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp;
using Azure;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using System.Threading;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class UkrlpDataControllerTests
    {
        private Mock<IMediator> _mediator;
        private UkrlpDataController _sut;
        [SetUp]
        public void Before_Each_Test()
        {
            _mediator = new Mock<IMediator>();
            _sut = new UkrlpDataController(_mediator.Object, Mock.Of<ILogger<UkrlpDataController>>());
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddressesFromProvidersUpdatedSince_ReturnsContent()
        {
            var command = new UkrlpDataCommand {ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long>()};
            var providerAddresses = new List<ProviderAddress>
            {
                new()
                {
                    Address1 = "1 Green Road"
                }
            };
            var lookupResponse = new UkprnLookupResponse
            {
                Results = providerAddresses,
                Success = true
            };

            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(lookupResponse);

            var response = await _sut.GetProvidersData(command);

            var okResult = response as OkObjectResult;
            var actualResponse = okResult.Value;
            Assert.AreSame(actualResponse, providerAddresses);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode.GetValueOrDefault());
            _mediator.Verify(x => x.Send(It.IsAny<UkrlpDataCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddressesFromUkprns_ReturnsContent()
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = null, Ukprns = new List<long> { 12345678 } };
            var providerAddresses = new List<ProviderAddress>
            {
                new()
                {
                    Address1 = "1 Green Road"
                }
            };
            var lookupResponse = new UkprnLookupResponse
            {
                Results = providerAddresses,
                Success = true
            };
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(lookupResponse);

            var response = await _sut.GetProvidersData(command);
        
            var okResult = response as OkObjectResult;
            var actualResponse = okResult.Value;
            Assert.AreSame(actualResponse, providerAddresses);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode.GetValueOrDefault());
            _mediator.Verify(x => x.Send(It.IsAny<UkrlpDataCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_TwoParameters_ReturnsBadRequest()
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long> {12345678} };
            var response = await _sut.GetProvidersData(command);
        
            var badRequestResult = response as BadRequestResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }
        
        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_NoParameters_ReturnsBadRequest()
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = null, Ukprns = new List<long> () };
            var response = await _sut.GetProvidersData(command);
        
            var badRequestResult = response as BadRequestResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }
        
        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_NoContentFromService_ReturnsNotFound()
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long>() };
            var lookupResponse = new UkprnLookupResponse
            {
                Results = null,
                Success = true
            };
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(lookupResponse);

            var response = await _sut.GetProvidersData(command);
            
            var notFoundResult = response as NotFoundResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_UnsuccessfulFromService_ReturnsNotFound()
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long>() };
            var lookupResponse = new UkprnLookupResponse
            {
                Results = new List<ProviderAddress>(),
                Success = false
            };
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(lookupResponse);

            var response = await _sut.GetProvidersData(command);

            var notFoundResult = response as NotFoundResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }
    }
}
