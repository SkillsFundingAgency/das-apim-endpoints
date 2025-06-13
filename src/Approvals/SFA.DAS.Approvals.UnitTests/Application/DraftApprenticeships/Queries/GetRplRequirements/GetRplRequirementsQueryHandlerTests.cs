using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.UnitTests.Extensions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Queries.GetRplRequirements;

[TestFixture]
public class GetRplRequirementsQueryHandlerTests
{
    private GetRplRequirementsQueryHandler _handler;
    private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
    private Mock<ICourseTypesApiClient> _courseTypesApiClient;
    private Mock<ILogger<GetRplRequirementsQueryHandler>> _logger;
    private GetRplRequirementsQuery _query;
    private GetStandardsListItem _standardResponse;
    private GetRecognitionOfPriorLearningResponse _rplResponse;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _query = fixture.Create<GetRplRequirementsQuery>();
        _standardResponse = fixture.Create<GetStandardsListItem>();
        _rplResponse = fixture.Create<GetRecognitionOfPriorLearningResponse>();

        _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        _coursesApiClient.Setup(x =>
                x.Get<GetStandardsListItem>(
                    It.Is<GetStandardDetailsByIdRequest>(r => r.Id == _query.CourseId)))
            .ReturnsAsync(_standardResponse);

        _courseTypesApiClient = new Mock<ICourseTypesApiClient>();
        _courseTypesApiClient.Setup(x =>
                x.Get<GetRecognitionOfPriorLearningResponse>(
                    It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl == $"api/coursetypes/{_standardResponse.ApprenticeshipType}/features/rpl")))
            .ReturnsAsync(_rplResponse);

        _logger = new Mock<ILogger<GetRplRequirementsQueryHandler>>();

        _handler = new GetRplRequirementsQueryHandler(_coursesApiClient.Object, _courseTypesApiClient.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_Returns_RplRequirements_When_Standard_And_Rpl_Data_Exist()
    {
        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.IsRequired.Should().Be(_rplResponse.IsRequired);
        result.OffTheJobTrainingMinimumHours.Should().Be(_rplResponse.OffTheJobTrainingMinimumHours);
    }

    [Test]
    public async Task Handle_Returns_Null_When_Standard_Not_Found()
    {
        // Arrange
        _coursesApiClient.Setup(x =>
                x.Get<GetStandardsListItem>(
                    It.Is<GetStandardDetailsByIdRequest>(r => r.Id == _query.CourseId)))
            .ReturnsAsync((GetStandardsListItem)null);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _logger.VerifyLogged(
            $"Standard not found for course ID {_query.CourseId}",
            LogLevel.Error,
            Times.Once());
    }

    [Test]
    public async Task Handle_Returns_Null_When_Rpl_Data_Not_Found()
    {
        // Arrange
        _courseTypesApiClient.Setup(x =>
                x.Get<GetRecognitionOfPriorLearningResponse>(
                    It.Is<GetRecognitionOfPriorLearningRequest>(r => r.GetUrl == $"api/coursetypes/{_standardResponse.ApprenticeshipType}/features/rpl")))
            .ReturnsAsync((GetRecognitionOfPriorLearningResponse)null);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _logger.VerifyLogged(
            $"RPL requirements not found for apprenticeship type {_standardResponse.ApprenticeshipType}",
            LogLevel.Error,
            Times.Once());
    }
}