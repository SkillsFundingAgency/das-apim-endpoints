using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostVacancyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(Guid id)
        {
            var actual = new PostVacancyRequest(id, null);

            actual.PostUrl.Should().Be($"api/Vacancies/{id}");
        }

        [Test, AutoData]
        public void Then_The_Request_Data_Is_Sent(Guid id, PostVacancyRequestData data)
        {
            var actual = new PostVacancyRequest(id, data);

            var actualData = actual.Data as PostVacancyRequestData;
            actualData.Should().BeEquivalentTo(data);
        }
    }
}