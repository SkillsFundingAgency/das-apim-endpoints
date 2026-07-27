using FluentAssertions;
using Moq;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;
using AccountResource = SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts.Resource;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Rofjaa;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class GetProviderRelationshipServiceTests
{
    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Call_Api_WithcorrectValues(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
        [Frozen] Mock<IFjaaAgenciesService> fjaaAgenciesService,
       [Greedy] GetProviderRelationshipService sut)
    {
        // Arrange
        GetProviderAccountLegalEntitiesResponse providerDetails = GetProviderAccountDetails();

        SetupAgencyDetails(fjaaAgenciesService);
        SetupAccountsQuery(accountsClient, providerDetails);

        // Act
        var details = await sut.GetEmployerDetails(providerDetails);

        // Assert
        details.Should().NotBeNull();
        details.Should().HaveCount(providerDetails.AccountProviderLegalEntities.Count);

        fjaaAgenciesService.Verify(x => x.GetAgencies(It.IsAny<CancellationToken>()), Times.Once());
        accountsClient.Verify(x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(It.Is<PostAccountsQueryRequest>(r =>
            r.Data.Filter.AccountIds.OrderBy(id => id).SequenceEqual(new long[] { 1, 2 })), It.IsAny<bool>()), Times.Once());

        foreach (var provider in providerDetails.AccountProviderLegalEntities)
        {
            details.First(t => t.AgreementId == provider.AccountLegalEntityPublicHashedId).IsFlexiEmployer.Should().BeTrue();
        }
    }

    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Call_Api(
       [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
       [Frozen] Mock<IFjaaAgenciesService> fjaaAgenciesService,
      [Greedy] GetProviderRelationshipService sut)
    {
        // Arrange
        GetProviderAccountLegalEntitiesResponse providerDetails = GetProviderAccountDetails();

        SetupAgencyDetails(fjaaAgenciesService);

        accountsClient.Setup(x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(It.IsAny<PostAccountsQueryRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<PostAccountsQueryResponse>(new PostAccountsQueryResponse
            {
                Accounts =
                [
                    new AccountQueryResultItem
                    {
                        AccountId = 1,
                        ApprenticeshipEmployerType = nameof(ApprenticeshipEmployerType.Levy),
                        LegalEntities =
                        [
                            new AccountResource { Id = "1" },
                            new AccountResource { Id = "4" },
                            new AccountResource { Id = "3" }
                        ]
                    },
                    new AccountQueryResultItem
                    {
                        AccountId = 2,
                        ApprenticeshipEmployerType = nameof(ApprenticeshipEmployerType.NonLevy),
                        LegalEntities =
                        [
                            new AccountResource { Id = "5" },
                            new AccountResource { Id = "6" },
                            new AccountResource { Id = "7" }
                        ]
                    }
                ]
            }, System.Net.HttpStatusCode.OK, string.Empty));

        // Act
        var details = await sut.GetEmployerDetails(providerDetails);

        // Assert
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
        // Arrange
        var CoursesForProvider = new GetCoursesForProviderResponse
        {
            CourseTypes =
               [
                   new()
                    {
                         CourseType = "Apprenticeship",
                         Courses =
                         [
                              new() {  EffectiveFrom = new DateTime(2026, 1, 1) , EffectiveTo = null, LarsCode = "805"   },
                              new() {  EffectiveFrom = new DateTime(2026, 1, 1) , EffectiveTo = null, LarsCode = "806"   }
                         ]
                    },
                     new()
                    {
                         CourseType = "ShortCourse",
                         Courses =
                         [
                              new() {  EffectiveFrom = new DateTime(2026, 4, 1) , EffectiveTo = null, LarsCode = "ZSC00001"   },
                              new() {  EffectiveFrom = new DateTime(2026, 4, 1) , EffectiveTo = null, LarsCode = "ZSC00002"   }
                         ]
                    }
               ]
        };

        roatpClient.Setup(t => t.GetWithResponseCode<GetCoursesForProviderResponse>(It.Is<GetCoursesForProviderRequest>(t => t.Ukprn == ukprn)))
            .ReturnsAsync(new ApiResponse<GetCoursesForProviderResponse>(CoursesForProvider, System.Net.HttpStatusCode.OK,""));

        // Act
        var details = await sut.GetCoursesForProviderByUkprn(ukprn);

        // Assert
        details.Should().NotBeNull();
        details.CourseTypes.Should().NotBeNull();
        details.CourseTypes.Should().BeEquivalentTo(CoursesForProvider.CourseTypes);
        details.CourseTypes.Should().ContainSingle(t => t.CourseType == "Apprenticeship" && t.Courses.Any(k => k.EffectiveTo == null && k.LarsCode == "806" && k.EffectiveFrom == new DateTime(2026, 1, 1)));
        details.CourseTypes.Should().ContainSingle(t => t.CourseType == "ShortCourse" && t.Courses.Any(k => k.EffectiveTo == null && k.LarsCode == "ZSC00001" && k.EffectiveFrom == new DateTime(2026, 4, 1)));

        roatpClient.Verify(client => client.Get<GetCourseLevelsListResponse>(It.IsAny<GetCoursesForProviderRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Batches_Accounts_Query_When_More_Than_Max_Per_Request(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
        [Frozen] Mock<IFjaaAgenciesService> fjaaAgenciesService,
        [Greedy] GetProviderRelationshipService sut)
    {
        // Arrange
        const int accountCount = AccountQueryFieldNames.MaxAccountIdsPerRequest + 1;
        var providerDetails = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = Enumerable.Range(1, accountCount)
                .Select(i => new GetProviderAccountLegalEntityItem
                {
                    AccountId = i,
                    AccountLegalEntityPublicHashedId = $"Hash{i}"
                })
                .ToList()
        };

        SetupAgencyDetails(fjaaAgenciesService);

        accountsClient
            .Setup(x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(It.IsAny<PostAccountsQueryRequest>(), It.IsAny<bool>()))
            .ReturnsAsync((PostAccountsQueryRequest request, bool _) => new ApiResponse<PostAccountsQueryResponse>(
                new PostAccountsQueryResponse
                {
                    Accounts = request.Data.Filter.AccountIds.Select(accountId => new AccountQueryResultItem
                    {
                        AccountId = accountId,
                        ApprenticeshipEmployerType = nameof(ApprenticeshipEmployerType.NonLevy),
                        LegalEntities = []
                    }).ToList()
                },
                System.Net.HttpStatusCode.OK,
                string.Empty));

        // Act
        var details = await sut.GetEmployerDetails(providerDetails);

        // Assert
        details.Should().HaveCount(accountCount);
        accountsClient.Verify(
            x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(It.IsAny<PostAccountsQueryRequest>(), It.IsAny<bool>()),
            Times.Exactly(2));
        accountsClient.Verify(
            x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(
                It.Is<PostAccountsQueryRequest>(r => r.Data.Filter.AccountIds.Count == AccountQueryFieldNames.MaxAccountIdsPerRequest),
                It.IsAny<bool>()),
            Times.Once());
        accountsClient.Verify(
            x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(
                It.Is<PostAccountsQueryRequest>(r => r.Data.Filter.AccountIds.Count == 1),
                It.IsAny<bool>()),
            Times.Once());
    }

    private static void SetupAccountsQuery(
        Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
        GetProviderAccountLegalEntitiesResponse providerDetails)
    {
        accountsClient.Setup(x => x.PostWithResponseCode<AccountsQueryRequestBody, PostAccountsQueryResponse>(It.IsAny<PostAccountsQueryRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<PostAccountsQueryResponse>(new PostAccountsQueryResponse
            {
                Accounts = providerDetails.AccountProviderLegalEntities
                    .Select(x => x.AccountId)
                    .Distinct()
                    .Select(accountId => new AccountQueryResultItem
                    {
                        AccountId = accountId,
                        ApprenticeshipEmployerType = nameof(ApprenticeshipEmployerType.Levy),
                        LegalEntities =
                        [
                            new AccountResource { Id = "1" },
                            new AccountResource { Id = "4" },
                            new AccountResource { Id = "3" }
                        ]
                    }).ToList()
            }, System.Net.HttpStatusCode.OK, string.Empty));
    }

    private static void SetupAgencyDetails(Mock<IFjaaAgenciesService> fjaaAgenciesService)
    {
        fjaaAgenciesService.Setup(x => x.GetAgencies(It.IsAny<CancellationToken>()))
              .ReturnsAsync(new GetAgenciesResponse()
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
