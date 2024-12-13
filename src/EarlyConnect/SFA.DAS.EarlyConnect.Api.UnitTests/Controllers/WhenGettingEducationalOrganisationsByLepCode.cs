using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Api.Requests.GetRequests;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode;
using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingEducationalOrganisationsByLepCode
    {
        private Mock<IMediator> _mediatorMock;
        private EducationalOrganisationDataController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new EducationalOrganisationDataController(_mediatorMock.Object);
        }

        [Test]
        public async Task GetEducationalOrganisationsData_ValidRequest_ReturnsOk()
        {
            var educationalOrganisationsGetRequest = new EducationalOrganisationsGetRequest
            {
                LepCode = "ValidLepCode",
                SearchTerm = "School"
            };

            var mediatorResult = new GetEducationalOrganisationsByLepCodeResult
            {
                EducationalOrganisations = new List<EducationalOrganisationData>
                {
                    new EducationalOrganisationData
                    {
                        Name = "Test School",
                        AddressLine1 = "123 Test St",
                        Town = "Test Town",
                        County = "Test County",
                        PostCode = "12345"
                    }
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetEducationalOrganisationsByLepCodeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await _controller.GetEducationalOrganisationsData(educationalOrganisationsGetRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var returnedData = okResult.Value as GetEducationalOrganisationsResponse;
            Assert.That(returnedData, Is.Not.Null);
            Assert.That(returnedData.EducationalOrganisations.Count, Is.EqualTo(mediatorResult.EducationalOrganisations.Count));
            Assert.That(returnedData.EducationalOrganisations.First().Name, Is.EqualTo("Test School"));
        }

        [Test]
        public async Task GetEducationalOrganisationsData_NoResults_ReturnsEmptyList()
        {
            var educationalOrganisationsGetRequest = new EducationalOrganisationsGetRequest
            {
                LepCode = "ValidLepCode",
                SearchTerm = "NonExistent"
            };

            var mediatorResult = new GetEducationalOrganisationsByLepCodeResult
            {
                EducationalOrganisations = new List<EducationalOrganisationData>()
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetEducationalOrganisationsByLepCodeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await _controller.GetEducationalOrganisationsData(educationalOrganisationsGetRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var returnedData = okResult.Value as GetEducationalOrganisationsResponse;
            Assert.That(returnedData, Is.Not.Null);
            Assert.That(returnedData.EducationalOrganisations.Count, Is.EqualTo(0));
        }
    }
}
