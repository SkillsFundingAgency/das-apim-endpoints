
//using AutoFixture.NUnit3;
//using Moq;
//using SFA.DAS.ApprenticeAan.Application.Infrastructure;

//namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MyApprenticeshipControllerTests;

//public class UpdateMemberProfileAndPreferencesTests
//{

//    [Test, AutoData]
//    public void UpdateMemberProfileAndPreferences_MemberInfoIncluded_InvokePatchMember(
//        Guid memberId, 
//        Guid requestedByMemberId)
//    {
//        Mock<IAanHubRestApiClient> restApiClientMock = new();
//        MembersController sut = new(restApiClientMock.Object); 

//        sut.UpdateMemberProfileAndPreferences(memberId,requestedByMemberId);

//        restApiClientMock.Verify(c => c.PatchMember(memberId, ));
//    }

//}

//public class MembersController
//{
//    private readonly IAanHubRestApiClient _apiClient;

//    public MembersController(IAanHubRestApiClient apiClient)
//    {
//        _apiClient = apiClient;
//    }

//    [Put("{memberId}")]
//    public void UpdateMemberProfileAndPreferences([FromRoute] Guid memberId) 
//    {

//    }

//}
