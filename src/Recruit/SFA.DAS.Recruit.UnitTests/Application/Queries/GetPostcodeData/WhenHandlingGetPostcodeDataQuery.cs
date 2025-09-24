using System.Threading;
using SFA.DAS.Recruit.Application.Queries.GetPostcodeData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetPostcodeData;

public class WhenHandlingGetPostcodeDataQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Postcode_Info_Is_Returned(
        PostcodeInfo postcodeInfo,
        GetPostcodeDataQuery getPostcodeDataQuery,
        [Frozen] Mock<ILocationLookupService> locationLookupService,
        [Greedy] GetPostcodeDataQueryHandler sut)
    {
        // arrange
        locationLookupService
            .Setup(x => x.GetPostcodeInfoAsync(getPostcodeDataQuery.Postcode))
            .ReturnsAsync(postcodeInfo); 
        
        // act
        var result = await sut.Handle(getPostcodeDataQuery, CancellationToken.None);

        // assert
        result.Should().BeEquivalentTo(postcodeInfo, options => options.ExcludingMissingMembers());
    }
    
    [Test, MoqAutoData]
    public async Task Then_No_Postcode_Found_Is_Handled(
        LocationItem locationItem,
        GetPostcodeDataQuery getPostcodeDataQuery,
        [Frozen] Mock<ILocationLookupService> locationLookupService,
        [Greedy] GetPostcodeDataQueryHandler sut)
    {
        // arrange
        locationLookupService
            .Setup(x => x.GetPostcodeInfoAsync(getPostcodeDataQuery.Postcode))
            .ReturnsAsync((PostcodeInfo)null!);
        
        // act
        var result = await sut.Handle(getPostcodeDataQuery, CancellationToken.None);

        // assert
        result.Should().Be(GetPostcodeDataResult.None);
    }
}