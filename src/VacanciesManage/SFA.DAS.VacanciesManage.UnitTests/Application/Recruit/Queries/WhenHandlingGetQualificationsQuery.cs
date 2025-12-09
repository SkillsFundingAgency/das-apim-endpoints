using System.Collections.Generic;
using System.Threading;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.Recruit.Queries
{
    public class WhenHandlingQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_If_Cached_Then_Cached_Response_Returned_And_Api_Not_Called(
            List<string> cacheQueryResponse,
            GetQualificationsQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetQualificationsQueryHandler handler)
        {
            cacheStorageService.Setup(x => x.RetrieveFromCache<List<string>>("GetQualifications"))
                .ReturnsAsync(cacheQueryResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Qualifications.Should().BeEquivalentTo(cacheQueryResponse);
            apiClient
                .Verify(x => x.Get<List<string>>(
                    It.Is<GetCandidateSkillsRequest>(c => c.GetUrl.Contains($"referencedata/candidate-qualifications"))), Times.Never);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called_And_Cache_Updated(
            List<string> apiQueryResponse,
            GetQualificationsQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetQualificationsQueryHandler handler)
        {
            cacheStorageService.Setup(x => x.RetrieveFromCache<List<string>>("GetQualifications"))
                .ReturnsAsync(() => null);
            apiClient.Setup(x =>
                    x.Get<List<string>>(
                        It.Is<GetCandidateQualificationsRequest>(c => c.GetUrl.Contains($"referencedata/candidate-qualifications"))))
                .ReturnsAsync(apiQueryResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Qualifications.Should().BeEquivalentTo(apiQueryResponse);
            cacheStorageService.Verify(x=>x.SaveToCache("GetQualifications",apiQueryResponse, 3, null));
        }
    }
}