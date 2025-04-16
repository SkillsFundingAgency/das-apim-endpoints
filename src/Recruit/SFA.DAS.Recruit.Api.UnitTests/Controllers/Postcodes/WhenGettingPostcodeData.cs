using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Postcodes;

public class WhenGettingPostcodeData
{
    private const string Postcode = "AB11 1AA";
    
    [Test, MoqAutoData]
    public async Task Then_No_Result_Returns_Not_Found(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] PostcodesController sut
        )
    {
        // arrange
        mediator
            .Setup(x => x.Send(It.IsAny<GetPostcodeDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetPostcodeDataResult.None);
        
        // act
        var result = await sut.GetPostcodeData(Postcode, CancellationToken.None);
        
        // assert
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Test]
    [MoqInlineAutoData(null)]
    [MoqInlineAutoData("")]
    public async Task Then_Invalid_Value_Returns_Bad_Request(
        string postcode,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] PostcodesController sut
    )
    {
        // act
        var result = await sut.GetPostcodeData(postcode, CancellationToken.None);
        
        // assert
        result.Should().BeOfType<BadRequestResult>();
    }
    
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetPostcodeDataResult getpostcodeDataResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] PostcodesController sut
    )
    {
        // arrange
        GetPostcodeDataQuery? expectedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetPostcodeDataQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetPostcodeDataResult>, CancellationToken>((q, _) => expectedQuery = q as GetPostcodeDataQuery)
            .ReturnsAsync(getpostcodeDataResult);
        
        // act
        var result = await sut.GetPostcodeData(Postcode, CancellationToken.None) as OkObjectResult;
        var payload = result?.Value as GetPostcodeDataResponse;
        
        // assert
        expectedQuery.Should().NotBeNull();
        expectedQuery!.Postcode.Should().Be(Postcode);
  
        payload.Should().NotBeNull();
        
        payload!.Query.Should().Be(Postcode);
        payload.Result.Should().BeEquivalentTo(getpostcodeDataResult, options => options.WithMapping("Name", "Postcode"));
    }
}