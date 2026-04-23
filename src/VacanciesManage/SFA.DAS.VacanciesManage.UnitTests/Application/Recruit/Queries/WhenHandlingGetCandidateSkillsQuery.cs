using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetCandidateSkills;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.Recruit.Queries;

public class WhenHandlingGetCandidateSkillsQuery
{
    [Test, MoqAutoData]
    public async Task Then_If_Cached_Then_Cached_Response_Returned_And_Api_Not_Called(
        List<string> cacheQueryResponse,
        GetCandidateSkillsQuery query,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        GetCandidateSkillsQueryHandler handler)
    {
        cacheStorageService.Setup(x => x.RetrieveFromCache<List<string>>("GetCandidateSkills"))
            .ReturnsAsync(cacheQueryResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.CandidateSkills.Should().BeEquivalentTo(cacheQueryResponse);
        apiClient
            .Verify(x => x.Get<List<string>>(
                It.Is<GetCandidateSkillsRequest>(c => c.GetUrl.Contains($"referencedata/candidate-skills"))), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Api_Called_And_Added_To_Cache(
        List<string> apiQueryResponse,
        GetCandidateSkillsQuery query,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> apiClient,
        GetCandidateSkillsQueryHandler handler)
    {
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<List<string>>("GetCandidateSkills"))
            .ReturnsAsync((List<string>)null!);
        apiClient
            .Setup(x => x.Get<List<string>>(It.IsAny<GetReferencedataCandidateSkillsApiRequest>()))
            .ReturnsAsync(apiQueryResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.CandidateSkills.Should().BeEquivalentTo(apiQueryResponse);
        cacheStorageService.Verify(x => x.SaveToCache("GetCandidateSkills", apiQueryResponse, 3, null));
    }
}