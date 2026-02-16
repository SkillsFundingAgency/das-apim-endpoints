using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Rofjaa;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Rofjaa;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class GetProviderRelationshipServiceTests
{
    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Call_Api_WithcorrectValues(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
        [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
       [Greedy] GetProviderRelationshipService sut)
    {
        GetProviderAccountLegalEntitiesResponse providerDetails = GetProviderAccountDetails();

        SetupAgencyDetails(fjaaApiClient);

        foreach (var provider in providerDetails.AccountProviderLegalEntities)
        {
            accountsClient.Setup(x => x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(t => t.AccountId == provider.AccountId))).
                ReturnsAsync(new GetAccountByIdResponse()
                {
                    ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy,
                    AccountId = provider.AccountId,
                    LegalEntities =
                    [
                         new() {  Id = "1" },
                         new () { Id = "4" },
                         new () { Id = "3" }
                    ]
                });
        }

        // Act
        var details = await sut.GetEmployerDetails(providerDetails);

        //Assert
        details.Should().NotBeNull();
        details.Should().HaveCount(providerDetails.AccountProviderLegalEntities.Count);

        fjaaApiClient.Verify(x => x.Get<GetAgenciesResponse>(It.Is<GetAgenciesQuery>(t => t.GetUrl == "agencies")), Times.Once());

        foreach (var provider in providerDetails.AccountProviderLegalEntities)
        {
            accountsClient.Verify(x => x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(t => t.AccountId == provider.AccountId)), Times.Once());
            details.First(t => t.AgreementId == provider.AccountLegalEntityPublicHashedId).IsFlexiEmployer.Should().BeTrue();
        }
    }

    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Call_Api(
       [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
       [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
      [Greedy] GetProviderRelationshipService sut)
    {
        GetProviderAccountLegalEntitiesResponse providerDetails = GetProviderAccountDetails();

        SetupAgencyDetails(fjaaApiClient);

        accountsClient.Setup(x => x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(t => t.AccountId == providerDetails.AccountProviderLegalEntities.First().AccountId))).
                ReturnsAsync(new GetAccountByIdResponse()
                {
                    ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy,
                    AccountId = providerDetails.AccountProviderLegalEntities.First().AccountId,
                    LegalEntities =
                    [
                         new() {  Id = "1" },
                         new () { Id = "4" },
                         new () { Id = "3" }
                    ]
                });

        accountsClient.Setup(x => x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(t => t.AccountId == providerDetails.AccountProviderLegalEntities.Last().AccountId))).
              ReturnsAsync(new GetAccountByIdResponse()
              {
                  ApprenticeshipEmployerType = ApprenticeshipEmployerType.NonLevy,
                  AccountId = providerDetails.AccountProviderLegalEntities.Last().AccountId,
                  LegalEntities =
                  [
                       new() {  Id = "5" },
                       new () { Id = "6" },
                       new () { Id = "7" }
                  ]
              });

        // Act
        var details = await sut.GetEmployerDetails(providerDetails);

        //Assert
        details.Should().NotBeNull();
        details.Should().HaveCount(providerDetails.AccountProviderLegalEntities.Count);
        var provider = details.First(t => t.AgreementId == "AccHash2");
        provider.IsFlexiEmployer.Should().BeFalse();
        provider.IsLevy.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task GetCoursesForProviderByUkprn_Call_Api_WithcorrectValues(
        long ukprn,
        GetCoursesForProviderResponse response,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpClient,
        [Greedy] GetProviderRelationshipService sut)
    {
        var CoursesForProvider = new GetCoursesForProviderResponse
        {
            CourseTypes =
               [
                   new()
                    {
                         CourseType = "Apprenticeship",
                         Courses =
                         [
                              new() {  EffectiveFrom = new DateTime(2026, 1, 1) , EffectiveTo = null, Larscode = "805"   },
                              new() {  EffectiveFrom = new DateTime(2026, 1, 1) , EffectiveTo = null, Larscode = "806"   }
                         ]
                    },
                     new()
                    {
                         CourseType = "ShortCourse",
                         Courses =
                         [
                              new() {  EffectiveFrom = new DateTime(2026, 4, 1) , EffectiveTo = null, Larscode = "ZSC00001"   },
                              new() {  EffectiveFrom = new DateTime(2026, 4, 1) , EffectiveTo = null, Larscode = "ZSC00002"   }
                         ]
                    }
               ]
        };

        roatpClient.Setup(t => t.Get<GetCoursesForProviderResponse>(It.Is<GetCoursesForProviderRequest>(t => t.Ukprn == ukprn)))
            .ReturnsAsync(CoursesForProvider);

        var details = await sut.GetCoursesForProviderByUkprn(ukprn);

        details.Should().NotBeNull();
        details.CourseTypes.Should().NotBeNull();
        details.CourseTypes.Should().BeEquivalentTo(CoursesForProvider.CourseTypes);
        details.CourseTypes.Should().ContainSingle(t => t.CourseType == "Apprenticeship" && t.Courses.Any(k => k.EffectiveTo == null && k.Larscode == "806" && k.EffectiveFrom == new DateTime(2026, 1, 1)));
        details.CourseTypes.Should().ContainSingle(t => t.CourseType == "ShortCourse" && t.Courses.Any(k => k.EffectiveTo == null && k.Larscode == "ZSC00001" && k.EffectiveFrom == new DateTime(2026, 4, 1)));

        roatpClient.Verify(client => client.Get<GetCourseLevelsListResponse>(It.IsAny<GetCoursesForProviderRequest>()), Times.Never);
    }

    private static void SetupAgencyDetails(Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient)
    {
        fjaaApiClient.Setup(x => x.Get<GetAgenciesResponse>(It.Is<GetAgenciesQuery>(t => t.GetUrl == "agencies"))).
              ReturnsAsync(new GetAgenciesResponse()
              {
                  Agencies =
                  [
                       new GetAgencyResponse(){ LegalEntityId = 1, IsGrantFunded = true },
                       new GetAgencyResponse() { LegalEntityId = 2, IsGrantFunded = true },
                       new GetAgencyResponse() { LegalEntityId = 3, IsGrantFunded = false },
                  ]
              });
    }

    private static GetProviderAccountLegalEntitiesResponse GetProviderAccountDetails()
    {
        return new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                 new() { AccountId = 1, AccountLegalEntityPublicHashedId = "AccHash1"},
                 new() { AccountId = 2, AccountLegalEntityPublicHashedId = "AccHash2"},
            ]
        };
    }
}