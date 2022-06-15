using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetCandidateSkills;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetCandidateSkillsListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetCandidateSkillsQueryResponse source)
        {
            var actual = (GetCandidateSkillsListResponse) source;

            actual.CandidateSkills.Should().BeEquivalentTo(source.CandidateSkills);
        }

        [Test, AutoData]
        public void Then_If_Null_Empty_List_Returned(GetCandidateSkillsQueryResponse source)
        {
            source.CandidateSkills = null;
            
            var actual = (GetCandidateSkillsListResponse) source;

            actual.CandidateSkills.Should().BeEmpty();
        }
    }
}