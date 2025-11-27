using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Services.SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.LearnerData.UnitTests.Application.UpdateLearner
{
    internal class WhenHandlingUpdateLearnerCommandWithBreaksInLearning
    {
        private readonly Fixture _fixture = new();

#pragma warning disable CS8618 // Non-nullable field, instantiated in SetUp method
        private Mock<ILearningApiClient<LearningApiConfiguration>> _learningApiClient;
        private Mock<IEarningsApiClient<EarningsApiConfiguration>> _earningsApiClient;
        private Mock<IUpdateLearningPutRequestBuilder> _updateLearningPutRequestBuilder;
        private Mock<ILogger<UpdateLearnerCommandHandler>> _logger;
        private UpdateLearnerCommandHandler _sut;


#pragma warning restore CS8618 // Non-nullable field, instantiated in SetUp method

        [SetUp]
        public void Setup()
        {
            _learningApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
            _earningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
            _updateLearningPutRequestBuilder = new Mock<IUpdateLearningPutRequestBuilder>();
            _logger = new Mock<ILogger<UpdateLearnerCommandHandler>>();
            _sut = new UpdateLearnerCommandHandler(
                _logger.Object,
                _learningApiClient.Object,
                _earningsApiClient.Object,
                _updateLearningPutRequestBuilder.Object,
                Mock.Of<ICoursesApiClient<CoursesApiConfiguration>>());
        }


        



        
        
       
    }
}
