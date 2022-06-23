using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostVacancyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(Guid id,string email, int ukprn, PostVacancyRequestData data)
        {
            var actual = new PostVacancyRequest(id, ukprn, email, data);

            actual.PostUrl.Should().Be($"api/Vacancies/{id}?ukprn={ukprn}&userEmail={email}");
        }

        [Test, AutoData]
        public void Then_The_Request_Data_Is_Sent(Guid id,string email, int ukprn, PostVacancyRequestData data)
        {
            var actual = new PostVacancyRequest(id,ukprn, email, data);

            var actualData = actual.Data as PostVacancyRequestData;
            actualData.Should().BeEquivalentTo(data);
        }
    }
}