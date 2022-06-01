using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostTraineeshipVacancyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(Guid id, string email, int ukprn, PostTraineeshipVacancyRequestData data)
        {
            var actual = new PostTraineeshipVacancyRequest(id, ukprn, email, data);

            actual.PostUrl.Should().Be($"api/Vacancies/CreateTraineeship/{id}?ukprn={ukprn}&userEmail={email}");
        }

        [Test, AutoData]
        public void Then_The_Request_Data_Is_Sent(Guid id, string email, int ukprn, PostTraineeshipVacancyRequestData data)
        {
            var actual = new PostTraineeshipVacancyRequest(id, ukprn, email, data);

            var actualData = actual.Data as PostTraineeshipVacancyRequestData;
            actualData.Should().BeEquivalentTo(data);
        }
    }
}