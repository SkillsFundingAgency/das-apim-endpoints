using System.Linq;
using System.Net;
using System.Threading;
using SFA.DAS.Recruit.Application.Queries.GetBulkPostcodeData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetBulkPostcodeData;

public class WhenHandlingGetBulkPostcodeDataQuery
{
    [Test, MoqAutoData]
    public async Task Then_PostcodeData_Are_Fetched(
        GetBulkPostcodeDataResponse getBulkPostcodeDataResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> locationApiClient,
        [Greedy] GetBulkPostcodeDataQueryHandler sut)
    {
        // arrange
        var getBulkPostcodeDataQuery = new GetBulkPostcodeDataQuery(getBulkPostcodeDataResponse.Results.Select(x => x.Postcode).ToList());
        var apiResponse = new ApiResponse<GetBulkPostcodeDataResponse>(getBulkPostcodeDataResponse, HttpStatusCode.OK, null);
        GetLocationsByPostBulkPostcodeRequest? request = null;
        locationApiClient
            .Setup(x => x.PostWithResponseCode<GetBulkPostcodeDataResponse>(It.IsAny<GetLocationsByPostBulkPostcodeRequest>(), true))
            .Callback<IPostApiRequest, bool>((x, _) => request = x as GetLocationsByPostBulkPostcodeRequest)
            .ReturnsAsync(apiResponse);
        
        // act
        var result = await sut.Handle(getBulkPostcodeDataQuery, CancellationToken.None);

        // assert
        request.Should().NotBeNull();
        request!.PostUrl.Should().Be("api/Postcodes/bulk");
        request.Data.Should().BeEquivalentTo(getBulkPostcodeDataQuery.Postcodes);
        
        result.Should().NotBeNull();
        result.Results.Select(x => x.Result).Should().BeEquivalentTo(getBulkPostcodeDataResponse.Results, options => options.ExcludingMissingMembers());
    }
    
    [Test, MoqAutoData]
    public async Task Then_Unsuccessful_Api_Call_Returns_Null(
        GetBulkPostcodeDataResponse getBulkPostcodeDataResponse,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> locationApiClient,
        [Greedy] GetBulkPostcodeDataQueryHandler sut)
    {
        // arrange
        var getBulkPostcodeDataQuery = new GetBulkPostcodeDataQuery(getBulkPostcodeDataResponse.Results.Select(x => x.Postcode).ToList());
        var apiResponse = new ApiResponse<GetBulkPostcodeDataResponse>(getBulkPostcodeDataResponse, HttpStatusCode.InternalServerError, null);
        GetLocationsByPostBulkPostcodeRequest? request = null;
        locationApiClient
            .Setup(x => x.PostWithResponseCode<GetBulkPostcodeDataResponse>(It.IsAny<GetLocationsByPostBulkPostcodeRequest>(), true))
            .Callback<IPostApiRequest, bool>((x, _) => request = x as GetLocationsByPostBulkPostcodeRequest)
            .ReturnsAsync(apiResponse);
        
        // act
        var result = await sut.Handle(getBulkPostcodeDataQuery, CancellationToken.None);

        // assert
        request.Should().NotBeNull();
        request!.PostUrl.Should().Be("api/Postcodes/bulk");
        request.Data.Should().BeEquivalentTo(getBulkPostcodeDataQuery.Postcodes);
        
        result.Should().BeNull();
    }
}