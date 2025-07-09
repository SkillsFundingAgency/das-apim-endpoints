using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Queries.GetRplRequirements;

[TestFixture]
public class GetRplRequirementsQueryHandlerTests
{
    private GetRplRequirementsQueryHandler _handler;
    private Mock<ICourseTypeRulesService> _courseTypeRulesService;
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

        _courseTypeRulesService = new Mock<ICourseTypeRulesService>();
        _courseTypeRulesService.Setup(x => x.GetRplRulesAsync(_query.CourseId))
            .ReturnsAsync(new RplRulesResult
            {
                Standard = _standardResponse,
                RplRules = _rplResponse
            });

        _logger = new Mock<ILogger<GetRplRequirementsQueryHandler>>();

        _handler = new GetRplRequirementsQueryHandler(_courseTypeRulesService.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_Returns_RplRequirements_When_Standard_And_Rpl_Data_Exist()
    {
        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ApprenticeshipType.Should().Be(_standardResponse.ApprenticeshipType);
        result.IsRequired.Should().Be(_rplResponse.IsRequired);
        result.OffTheJobTrainingMinimumHours.Should().Be(_rplResponse.OffTheJobTrainingMinimumHours);
    }

    [Test]
    public async Task Handle_Throws_Exception_When_Standard_Not_Found()
    {
        // Arrange
        _courseTypeRulesService.Setup(x => x.GetRplRulesAsync(_query.CourseId))
            .ThrowsAsync(new Exception($"Standard not found for course ID {_query.CourseId}"));

        // Act
        var act = () => _handler.Handle(_query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Standard not found for course ID {_query.CourseId}");
    }

    [Test]
    public async Task Handle_Throws_Exception_When_Rpl_Data_Not_Found()
    {
        // Arrange
        _courseTypeRulesService.Setup(x => x.GetRplRulesAsync(_query.CourseId))
            .ThrowsAsync(new Exception($"RPL rules not found for apprenticeship type {_standardResponse.ApprenticeshipType}"));

        // Act
        var act = () => _handler.Handle(_query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"RPL rules not found for apprenticeship type {_standardResponse.ApprenticeshipType}");
    }
}