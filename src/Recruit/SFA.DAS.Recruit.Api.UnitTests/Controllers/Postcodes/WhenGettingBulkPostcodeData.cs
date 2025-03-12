using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetBulkPostcodeData;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Postcodes;

public class WhenGettingBulkPostcodeData
{
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        List<string> postcodes,
        GetBulkPostcodeDataResult getBulkPostcodeDataResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] PostcodesController sut)
    {
        // arrange
        GetBulkPostcodeDataQuery? expectedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetBulkPostcodeDataQuery>(), CancellationToken.None))
            .Callback<IRequest<GetBulkPostcodeDataResult>, CancellationToken>((x, _) => expectedQuery = x as GetBulkPostcodeDataQuery)
            .ReturnsAsync(getBulkPostcodeDataResult);

        // act
        var result = await sut.GetBulkPostcodeData(postcodes, CancellationToken.None) as OkObjectResult;
        var payload = result?.Value as List<GetBulkPostcodeDataItemResult>;

        // assert
        expectedQuery.Should().NotBeNull();
        expectedQuery!.Postcodes.Should().BeEquivalentTo(postcodes);
        
        payload.Should().NotBeNull();
        payload!.Should().BeEquivalentTo(getBulkPostcodeDataResult.Results);
    }
}