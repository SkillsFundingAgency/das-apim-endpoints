using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetQualificationsListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetQualificationsQueryResponse source)
        {
            var actual = (GetQualificationsResponse) source;

            actual.Qualifications.Should().BeEquivalentTo(source.Qualifications);
        }

        [Test, AutoData]
        public void Then_If_Null_Empty_List_Returned(GetQualificationsQueryResponse source)
        {
            source.Qualifications = null;
            
            var actual = (GetQualificationsResponse) source;

            actual.Qualifications.Should().BeEmpty();
        }
    }
}