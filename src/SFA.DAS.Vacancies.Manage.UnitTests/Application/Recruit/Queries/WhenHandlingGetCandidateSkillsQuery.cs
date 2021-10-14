using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetCandidateSkills;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Recruit.Queries
{
    public class WhenHandlingGetCandidateSkillsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called(
            List<string> apiQueryResponse,
            GetCandidateSkillsQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
            GetCandidateSkillsQueryHandler handler)
        {
            apiClient
                .Setup(x => x.Get<List<string>>(
                    It.Is<GetCandidateSkillsRequest>(c => c.GetUrl.Contains($"referencedata/candidate-skills"))))
                .ReturnsAsync(apiQueryResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.CandidateSkills.Should().BeEquivalentTo(apiQueryResponse);
        }
    }
}