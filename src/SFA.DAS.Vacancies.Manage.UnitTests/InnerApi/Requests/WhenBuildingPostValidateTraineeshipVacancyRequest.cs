using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostValidateTraineeshipVacancyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(Guid id, PostTraineeshipVacancyRequestData data)
        {
            var actual = new PostValidateTraineeshipVacancyRequest(id, data);

            actual.PostUrl.Should().Be($"api/vacancies/{id}/ValidateTraineeship?ukprn={data.User.Ukprn}&userEmail={data.User.Email}");
        }

        [Test, AutoData]
        public void Then_The_Request_Data_Is_Sent(Guid id, PostTraineeshipVacancyRequestData data)
        {
            var actual = new PostValidateTraineeshipVacancyRequest(id, data);

            var actualData = actual.Data as PostTraineeshipVacancyRequestData;
            actualData.Should().BeEquivalentTo(data);
        }
    }
}