using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingAccountLegalEntities
    {
        private InnerApiResponse _innerApiResponse;
        private string _JsonString = "{\"Test\" : \"XXXX\"}";

        [SetUp]
        public void Arrange()
        {
            _innerApiResponse = new InnerApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Json = JsonDocument.Parse(_JsonString)
            };
        }

        [Test, MoqAutoData]
        public async Task When_Getting_LegalEntities_For_Account_Then_Request_Is_Passed_To_Inner_Api(
            long accountId,
            [Frozen] Mock<IEmployerIncentivesQueryPassThroughService> passThroughMock,
           [Greedy] AccountQueryController sut)
        {
            passThroughMock
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_innerApiResponse);

            await sut.GetLegalEntities(accountId);

            passThroughMock.Verify(x => x.GetAsync($"/accounts/{accountId}/legalentities", null, It.IsAny<CancellationToken>()));
        }

        [Test, MoqAutoData]
        public async Task When_Getting_LegalEntities_For_Account_Then_Response_From_Inner_Api_Is_Returned(
            long accountId,
            [Frozen] Mock<IEmployerIncentivesQueryPassThroughService> passThroughMock,
            [Greedy] AccountQueryController sut)
        {
            passThroughMock
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_innerApiResponse);

            var result = await sut.GetLegalEntities(accountId);

            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be((int)_innerApiResponse.StatusCode);
            objectResult.Value.ToString().Should().Be(_JsonString);
        }
    }
}
