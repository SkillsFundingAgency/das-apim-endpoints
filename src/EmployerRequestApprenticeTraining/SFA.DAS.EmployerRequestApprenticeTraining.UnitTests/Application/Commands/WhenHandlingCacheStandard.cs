﻿using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CacheStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.CacheStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingCacheStandard
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private Mock<ICoursesApiClient<CoursesApiConfiguration>> _mockCoursesApiClient;
        private CacheStandardCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _mockCoursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
            _handler = new CacheStandardCommandHandler(_mockApiClient.Object, _mockCoursesApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(string larsCode, string title, string reference, string sector, int level, string errorContent)
        {
            // Arrange
            var coursesResponse = new ApiResponse<StandardDetailResponse>(
                new StandardDetailResponse { Title = title, Route = sector, IfateReferenceNumber = reference, Level = level}, HttpStatusCode.OK, errorContent);

            _mockCoursesApiClient.Setup(c => c.GetWithResponseCode<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(coursesResponse);

            var standardResponse = new ApiResponse<StandardResponse>(
                new StandardResponse { StandardTitle= title, StandardLevel = level, StandardReference = reference, StandardSector = sector,}, HttpStatusCode.OK, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<CacheStandardRequestData, StandardResponse>(It.IsAny<CacheStandardRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(standardResponse);

            var command = new CacheStandardCommand { StandardLarsCode = larsCode};

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PostWithResponseCode<CacheStandardRequestData, StandardResponse>(It.Is<CacheStandardRequest>(r => 
                r.Data.StandardReference == coursesResponse.Body.IfateReferenceNumber &&
                r.Data.StandardSector == coursesResponse.Body.Route &&
                r.Data.StandardTitle == coursesResponse.Body.Title &&
                r.Data.StandardLevel == coursesResponse.Body.Level), It.IsAny<bool>()), Times.Once);
        }


        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_CoursesApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CacheStandardCommand command,
            string errorContent)
        {
            var response = new ApiResponse<StandardDetailResponse>(null, statusCode, errorContent);

            _mockCoursesApiClient
                .Setup(client => client.GetWithResponseCode<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
                .ReturnsAsync(response);

            var standardResponse = new ApiResponse<StandardResponse>(new StandardResponse(), HttpStatusCode.OK, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<CacheStandardRequestData, StandardResponse>(It.IsAny<CacheStandardRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(standardResponse);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_RATApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CacheStandardCommand command,
            string errorContent)
        {
            var coursesResponse = new ApiResponse<StandardDetailResponse>(new StandardDetailResponse(), HttpStatusCode.OK, errorContent);

            _mockCoursesApiClient.Setup(c => c.GetWithResponseCode<StandardDetailResponse>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == command.StandardLarsCode)))
                .ReturnsAsync(coursesResponse);

            var response = new ApiResponse<StandardResponse>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<CacheStandardRequestData, StandardResponse>(It.IsAny<CacheStandardRequest>(), true))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
