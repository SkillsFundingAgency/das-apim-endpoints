﻿using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetQualificationDetailsQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetQualificationDetailsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetQualificationDetailsQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_QualificationDetailsData_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationDetailsQuery>();
            var response = _fixture.Create<GetQualificationDetailsQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailsApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(response.Id));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationDetailsQuery>();

            _apiClientMock.Setup(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailsApiRequest>()))
                          .Returns(Task.FromResult<GetQualificationDetailsQueryResponse>(null));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo($"No details found for qualification reference: {query.QualificationReference}"));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationDetailsQuery>();
            var exceptionMessage = "An error occurred";
            _apiClientMock.Setup(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailsApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
