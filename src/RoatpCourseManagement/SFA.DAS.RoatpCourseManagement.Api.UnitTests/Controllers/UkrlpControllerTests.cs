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

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class UkrlpDataControllerTests
    {
        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_ReturnsContent(
            [Frozen] Mock<IUkrlpService> serviceMock,
            [Greedy] UkrlpDataController sut)
        {
            var command = new UkrlpDataCommand {ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long>()};
            var providerAddresses = new List<ProviderAddress>
            {
                new()
                {
                    Address1 = "1 Green Road"
                }
            };

            serviceMock.Setup(s => s.GetAddresses(command)).ReturnsAsync(providerAddresses);
            var response = await sut.GetProvidersData(command);

            var okResult = response as OkObjectResult;
            var actualResponse = okResult.Value;
            Assert.AreSame(actualResponse, providerAddresses);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode.GetValueOrDefault());
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_TwoParameters_ReturnsBadRequest(
            [Frozen] Mock<IUkrlpService> serviceMock,
            [Greedy] UkrlpDataController sut)
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long> {12345678} };
            var response = await sut.GetProvidersData(command);

            var badRequestResult = response as BadRequestResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_NoParameters_ReturnsBadRequest(
            [Frozen] Mock<IUkrlpService> serviceMock,
            [Greedy] UkrlpDataController sut)
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = null, Ukprns = new List<long> () };
            var response = await sut.GetProvidersData(command);

            var badRequestResult = response as BadRequestResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        [MoqAutoData]
        public async Task GetProviderAddresses_NoContentFromService_ReturnsNotFound(
            [Frozen] Mock<IUkrlpService> serviceMock,
            [Greedy] UkrlpDataController sut)
        {
            var command = new UkrlpDataCommand { ProvidersUpdatedSince = DateTime.Today, Ukprns = new List<long>() };
            serviceMock.Setup(s => s.GetAddresses(command)).ReturnsAsync((List<ProviderAddress>)null);
            var response = await sut.GetProvidersData(command);

            var notFoundResult = response as NotFoundResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }
    }
}
