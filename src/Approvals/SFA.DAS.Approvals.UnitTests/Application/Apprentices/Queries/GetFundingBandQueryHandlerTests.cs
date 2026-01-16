using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Courses.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

[TestFixture]
public class GetFundingBandQueryHandlerTests
{
    private GetFundingBandQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

    private GetTrainingProgrammeResponse _trainingProgrammeResponse;
    private GetFundingBandQuery _query;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _query = fixture.Create<GetFundingBandQuery>();
        _trainingProgrammeResponse = new GetTrainingProgrammeResponse();
        _trainingProgrammeResponse.TrainingProgramme = new TrainingProgramme
        {
            CourseCode = _query.CourseCode,
            StandardUId = "XXXX",
            StandardPageUrl = "https://test123",
            FundingPeriods = new List<TrainingProgrammeFundingPeriod>
            {
                new() {EffectiveFrom = DateTime.MinValue, EffectiveTo = DateTime.MaxValue, FundingCap = 1234}
            }
        };

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.Get<GetTrainingProgrammeResponse>(It.Is<GetCalculatedVersionOfTrainingProgrammeRequest>(r =>
                    r.CourseCode == _query.CourseCode && r.StartDate == _query.StartDate.Value)))
            .ReturnsAsync(_trainingProgrammeResponse);

        _handler = new GetFundingBandQueryHandler(_apiClient.Object);
    }

    [Test]
    public async Task Handle_Returns_FundingInfo_From_Commitments_Api()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.StandardPageUrl.Should().Be(_trainingProgrammeResponse.TrainingProgramme.StandardPageUrl);
        result.Version.Should().Be(_trainingProgrammeResponse.TrainingProgramme.Version);
    }

    [Test]
    public async Task Handle_Returns_null_if_FundingInfo_not_found()
    {
        _apiClient.Setup(x =>
                x.Get<GetTrainingProgrammeResponse>(It.Is<GetCalculatedVersionOfTrainingProgrammeRequest>(r =>
                    r.CourseCode == _query.CourseCode && r.StartDate == _query.StartDate.Value)))
            .ReturnsAsync((GetTrainingProgrammeResponse) null);

        var result = await _handler.Handle(_query, CancellationToken.None);
        Assert.That(result, Is.Null);
    }
}

