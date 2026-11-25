using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidatePostcodeAddressQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Locations_From_Location_Api(
        GetCandidatePostcodeAddressQuery query,
        PostcodeInfo response,
        [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
        [Greedy] GetCandidatePostcodeAddressQueryHandler handler)
    {
        // arrange
        mockLocationLookupService
            .Setup(x => x.GetPostcodeInfoAsync(query.Postcode))
            .ReturnsAsync(response);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // act
        result.PostcodeExists.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_No_Address_Returned_So_PostcodeExists_Is_False(
        GetCandidatePostcodeAddressQuery query,
        [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
        [Greedy] GetCandidatePostcodeAddressQueryHandler handler)
    {
        // arrange
        mockLocationLookupService
            .Setup(x => x.GetPostcodeInfoAsync(query.Postcode))
            .ReturnsAsync(() => null!);

        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // act
        result.PostcodeExists.Should().BeFalse();
    }
}