using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.ProhibitedContent;

public class WhenGettingProfanities
{
    [Test, MoqAutoData]
    public async Task Then_The_Profanities_List_Is_Returned(
        List<string> profanities,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitInnerClient,
        [Greedy] ProhibitedContentController sut)
    {
        // arrange
        var response = new ApiResponse<List<string>>(profanities, HttpStatusCode.OK, null);
        GetProfanitiesRequest? capturedRequest = null;
        recruitInnerClient
            .Setup(x => x.GetWithResponseCode<List<string>>(It.IsAny<GetProfanitiesRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetProfanitiesRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.GetProfanities(recruitInnerClient.Object) as OkObjectResult;

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be("api/prohibitedcontent/Profanity");
        
        result.Should().NotBeNull();
        var items = result.Value as List<string>;
        items.Should().BeEquivalentTo(profanities);
    }
    
    [Test, MoqAutoData]
    public async Task Then_No_Results_Are_Returned_When_There_Is_An_Issue(
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitInnerClient,
        [Greedy] ProhibitedContentController sut)
    {
        // arrange
        var response = new ApiResponse<List<string>>(null!, HttpStatusCode.InternalServerError, "some issue");
        recruitInnerClient
            .Setup(x => x.GetWithResponseCode<List<string>>(It.IsAny<GetProfanitiesRequest>()))
            .ReturnsAsync(response);

        // act
        var result = await sut.GetProfanities(recruitInnerClient.Object) as OkObjectResult;

        // assert
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(new List<string>());
    }
}