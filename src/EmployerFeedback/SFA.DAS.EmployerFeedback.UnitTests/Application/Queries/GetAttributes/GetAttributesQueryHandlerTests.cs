using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetAttributes
{
    public class GetAttributesQueryHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _apiClientMock;
        private GetAttributesQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _handler = new GetAttributesQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsAttributes_WhenApiResponseIsSuccess()
        {
            var attributes = new List<GetAttributesResponse>
            {
                new GetAttributesResponse { AttributeId = 1, AttributeName = "Test" }
            };
            var apiResponse = new ApiResponse<List<GetAttributesResponse>>(attributes, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetAttributesRequest();

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<List<GetAttributesResponse>>(It.Is<GetAttributesRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetAttributesQuery(), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Attributes, Is.EqualTo(attributes));
        }

        [Test]
        public void Handle_ThrowsException_WhenApiResponseIsNotSuccess()
        {
            var apiResponse = new ApiResponse<List<GetAttributesResponse>>(null, HttpStatusCode.InternalServerError, "Error");
            var expectedRequest = new GetAttributesRequest();

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<List<GetAttributesResponse>>(It.Is<GetAttributesRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var handler = new GetAttributesQueryHandler(_apiClientMock.Object);

            Assert.ThrowsAsync<ApiResponseException>(async () =>
                await handler.Handle(new GetAttributesQuery(), CancellationToken.None));
        }

        [Test]
        public async Task Handle_ReturnsNullAttributes_WhenApiResponseBodyIsNullButStatusIsSuccess()
        {
            var apiResponse = new ApiResponse<List<GetAttributesResponse>>(null, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetAttributesRequest();

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<List<GetAttributesResponse>>(It.Is<GetAttributesRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetAttributesQuery(), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Attributes, Is.Null);
        }
    }
}
