using System.Collections.Generic;
using System.Net;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Ai.Clients;

namespace SFA.DAS.RecruitJobs.UnitTests.Ai;

public class WhenGettingAiResultScore
{
    [Test, MoqAutoData]
    public void Then_An_Empty_Result_Score_Is_Six()
    {
        // arrange
        var sut = new AiReviewResult();

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(6);
    }
    
    [Test, MoqAutoData]
    public void Then_Successfully_Called_But_No_Results_Scores_Three()
    {
        // arrange
        var sut = new AiReviewResult
        {
            SpellcheckResult = new AzureAiResponse<Dictionary<string, int>> { StatusCode = HttpStatusCode.OK },
            DiscriminationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK },
            ContentEvaluationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK },
        };

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(3);
    }
    
    [Test, MoqAutoData]
    public void Then_No_Errors_Scores_Zero()
    {
        // arrange
        var sut = new AiReviewResult
        {
            SpellcheckResult = new AzureAiResponse<Dictionary<string, int>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, int>() },
            DiscriminationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
            ContentEvaluationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
        };

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(0);
    }
    
    [Test, MoqAutoData]
    public void Then_Discrimination_Errors_Score_One()
    {
        // arrange
        var sut = new AiReviewResult
        {
            SpellcheckResult = new AzureAiResponse<Dictionary<string, int>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, int>() },
            DiscriminationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string> { ["fieldName"] = "problem" } },
            ContentEvaluationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
        };

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(1);
    }
    
    [Test, MoqAutoData]
    public void Then_Content_Errors_Score_One()
    {
        // arrange
        var sut = new AiReviewResult
        {
            SpellcheckResult = new AzureAiResponse<Dictionary<string, int>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, int>() },
            DiscriminationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
            ContentEvaluationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string> { ["fieldName"] = "problem" } },
        };

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(1);
    }
    
    [Test, MoqAutoData]
    public void Then_Spelling_Errors_Score_Point_Five()
    {
        // arrange
        var sut = new AiReviewResult
        {
            SpellcheckResult = new AzureAiResponse<Dictionary<string, int>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, int> { ["fieldName"] = 1 } },
            DiscriminationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
            ContentEvaluationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
        };

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(0.5);
    }
    
    [Test, MoqAutoData]
    public void Then_Multiple_Spelling_Errors_Tally_Up()
    {
        // arrange
        var sut = new AiReviewResult
        {
            SpellcheckResult = new AzureAiResponse<Dictionary<string, int>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, int> { ["fieldName"] = 3 } },
            DiscriminationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
            ContentEvaluationResult = new AzureAiResponse<Dictionary<string, string>> { StatusCode = HttpStatusCode.OK, Result = new Dictionary<string, string>() },
        };

        // act
        var actual = sut.GetScore();

        // assert
        actual.Should().Be(1.5);
    }
}