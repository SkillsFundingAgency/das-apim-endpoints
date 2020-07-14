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

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.AccountControllerTests
{
    [TestFixture]
    public class WhenRemovingAccountLegalEntity
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
        public async Task Then_Request_Is_Passed_To_Inner_Api(
            long accountId,
            long accountLegalEntityId,
            [Frozen] Mock<IPassThroughApiClient> passThroughMock,
            [Greedy] AccountController sut)
        {
            passThroughMock
                .Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_innerApiResponse);

            await sut.RemoveLegalEntity(accountId, accountLegalEntityId);

            passThroughMock.Verify(x => x.Delete($"/accounts/{accountId}/legalentities/{accountLegalEntityId}", It.IsAny<CancellationToken>()));
        }

        [Test, MoqAutoData]
        public async Task Then_Response_Is_Returned_From_Inner_Api(
            long accountId,
            long accountLegalEntityId,
            [Frozen] Mock<IPassThroughApiClient> passThroughMock,
            [Greedy] AccountController sut)
        {
            passThroughMock
                .Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_innerApiResponse);

            var result  = await sut.RemoveLegalEntity(accountId, accountLegalEntityId);

            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();
            var objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be((int)_innerApiResponse.StatusCode);
            objectResult.Value.ToString().Should().Be(_JsonString);
        }
    }
}