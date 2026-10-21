using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Services;

[TestFixture]
public class EnglishAndMathsValueServiceTests
{
    private const string LearnAimRef = "60342287";
    private static readonly DateTime StartDate = new(2026, 9, 21);
    private const int MaximumAttempts = 4;

    private Mock<ISldDataApiClient<SLDDataApiConfiguration>> _sldDataApiClient;
    private EnglishAndMathsValueService _sut;

    [SetUp]
    public void SetUp()
    {
        _sldDataApiClient = new Mock<ISldDataApiClient<SLDDataApiConfiguration>>();
        _sut = new EnglishAndMathsValueService(
            _sldDataApiClient.Object,
            Mock.Of<ILogger<EnglishAndMathsValueService>>(),
            retryDelays: [TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero]);
    }

    [Test]
    public async Task GetValue_Returns_The_Value_From_Sld_For_The_Course_And_Start_Date()
    {
        IGetApiRequest sentRequest = null;
        _sldDataApiClient
            .Setup(x => x.GetWithResponseCode<decimal?>(It.IsAny<IGetApiRequest>()))
            .Callback<IGetApiRequest>(request => sentRequest = request)
            .ReturnsAsync(new ApiResponse<decimal?>(1234.56m, HttpStatusCode.OK, string.Empty));

        var result = await _sut.GetValue(LearnAimRef, StartDate);

        result.Should().Be(1234.56m);
        sentRequest.GetUrl.Should().Be("api/v1.0/lars/get-em-value/60342287/2026-09-21");
    }

    [Test]
    public async Task GetValue_When_Sld_Returns_NotFound_Throws_NotFound_Without_Retrying()
    {
        SetupSldResponses(new ApiResponse<decimal?>(null, HttpStatusCode.NotFound, "Not found"));

        var act = () => _sut.GetValue(LearnAimRef, StartDate);

        var exception = await act.Should().ThrowAsync<EnglishAndMathsValueNotFoundException>();
        exception.Which.LearnAimRef.Should().Be(LearnAimRef);
        exception.Which.StartDate.Should().Be(StartDate);
        VerifySldCalled(Times.Once());
    }

    [Test]
    public async Task GetValue_When_Sld_Returns_BadRequest_Throws_Unavailable_Without_Retrying()
    {
        SetupSldResponses(new ApiResponse<decimal?>(null, HttpStatusCode.BadRequest, "Bad Request"));

        var act = () => _sut.GetValue(LearnAimRef, StartDate);

        var exception = await act.Should().ThrowAsync<EnglishAndMathsValueUnavailableException>();
        exception.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        VerifySldCalled(Times.Once());
    }

    [Test]
    public async Task GetValue_When_Sld_Returns_ServerError_On_Every_Attempt_Throws_Unavailable_After_Retrying()
    {
        SetupSldResponses(new ApiResponse<decimal?>(null, HttpStatusCode.InternalServerError, "Internal Server Error"));

        var act = () => _sut.GetValue(LearnAimRef, StartDate);

        var exception = await act.Should().ThrowAsync<EnglishAndMathsValueUnavailableException>();
        exception.Which.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        VerifySldCalled(Times.Exactly(MaximumAttempts));
    }

    [Test]
    public async Task GetValue_When_Sld_Returns_ServerError_Then_Succeeds_Returns_The_Value()
    {
        SetupSldResponses(
            new ApiResponse<decimal?>(null, HttpStatusCode.InternalServerError, "Internal Server Error"),
            new ApiResponse<decimal?>(1000m, HttpStatusCode.OK, string.Empty));

        var result = await _sut.GetValue(LearnAimRef, StartDate);

        result.Should().Be(1000m);
        VerifySldCalled(Times.Exactly(2));
    }

    [Test]
    public async Task GetValue_When_The_Network_Fails_On_Every_Attempt_Throws_Unavailable_After_Retrying()
    {
        var networkFailure = new HttpRequestException("Connection refused");
        _sldDataApiClient
            .Setup(x => x.GetWithResponseCode<decimal?>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(networkFailure);

        var act = () => _sut.GetValue(LearnAimRef, StartDate);

        var exception = await act.Should().ThrowAsync<EnglishAndMathsValueUnavailableException>();
        exception.Which.InnerException.Should().BeSameAs(networkFailure);
        VerifySldCalled(Times.Exactly(MaximumAttempts));
    }

    [Test]
    public async Task GetValue_When_The_Request_Times_Out_On_Every_Attempt_Throws_Unavailable_After_Retrying()
    {
        _sldDataApiClient
            .Setup(x => x.GetWithResponseCode<decimal?>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(new TaskCanceledException("Timed out"));

        var act = () => _sut.GetValue(LearnAimRef, StartDate);

        await act.Should().ThrowAsync<EnglishAndMathsValueUnavailableException>();
        VerifySldCalled(Times.Exactly(MaximumAttempts));
    }

    [Test]
    public async Task GetValue_When_Sld_Returns_Ok_With_No_Value_Throws_Unavailable_Without_Retrying()
    {
        SetupSldResponses(new ApiResponse<decimal?>(null, HttpStatusCode.OK, string.Empty));

        var act = () => _sut.GetValue(LearnAimRef, StartDate);

        var exception = await act.Should().ThrowAsync<EnglishAndMathsValueUnavailableException>();
        exception.Which.StatusCode.Should().Be(HttpStatusCode.OK);
        VerifySldCalled(Times.Once());
    }

    private void SetupSldResponses(params ApiResponse<decimal?>[] responses)
    {
        var sequence = _sldDataApiClient.SetupSequence(x => x.GetWithResponseCode<decimal?>(It.IsAny<IGetApiRequest>()));
        foreach (var response in responses)
        {
            sequence = sequence.ReturnsAsync(response);
        }

        // Keep returning the last response so "every attempt" scenarios don't depend on the retry count
        for (var i = responses.Length; i < MaximumAttempts + 1; i++)
        {
            sequence = sequence.ReturnsAsync(responses[^1]);
        }
    }

    private void VerifySldCalled(Times times)
    {
        _sldDataApiClient.Verify(x => x.GetWithResponseCode<decimal?>(It.IsAny<IGetApiRequest>()), times);
    }
}
