using System.Threading;
using SFA.DAS.Recruit.Application.Queries.GetPostcodeData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetPostcodeData;

public class WhenHandlingGetPostcodeDataQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Postcode_Info_Is_Returned(
        LocationItem locationItem,
        GetPostcodeDataQuery getPostcodeDataQuery,
        [Frozen] Mock<ILocationLookupService> locationLookupService,
        [Greedy] GetPostcodeDataQueryHandler sut)
    {
        // arrange
        string? expectedPostcode = null;
        locationLookupService
            .Setup(x => x.GetLocationInformation(It.IsAny<string>(), 0, 0, false))
            .Callback<string, double, double, bool>((x, _, _, _) => expectedPostcode = x)
            .ReturnsAsync(locationItem); 
        
        // act
        var result = await sut.Handle(getPostcodeDataQuery, CancellationToken.None);

        // assert
        expectedPostcode.Should().Be(getPostcodeDataQuery.Postcode);
        result.Should().BeEquivalentTo(locationItem, options => options.ExcludingMissingMembers());
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
            .Setup(x => x.GetLocationInformation(It.IsAny<string>(), 0, 0, false))
            .ReturnsAsync((LocationItem)null!); 
        
        // act
        var result = await sut.Handle(getPostcodeDataQuery, CancellationToken.None);

        // assert
        result.Should().Be(GetPostcodeDataResult.None);
    }
}