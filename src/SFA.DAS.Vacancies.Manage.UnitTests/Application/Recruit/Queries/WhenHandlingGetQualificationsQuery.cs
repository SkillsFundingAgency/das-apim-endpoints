using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetQualifications;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Recruit.Queries
{
    public class WhenHandlingQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_If_Cached_Then_Cached_Response_Returned_And_Api_Not_Called(
            List<string> cacheQueryResponse,
            GetQualificationsQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
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
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetQualificationsQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.Get<List<string>>(
                        It.Is<GetQualificationsRequest>(c => c.GetUrl.Contains($"referencedata/candidate-qualifications"))))
                .ReturnsAsync(apiQueryResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Qualifications.Should().BeEquivalentTo(apiQueryResponse);
            cacheStorageService.Verify(x=>x.SaveToCache("GetQualifications",apiQueryResponse, 3));
        }
    }
}