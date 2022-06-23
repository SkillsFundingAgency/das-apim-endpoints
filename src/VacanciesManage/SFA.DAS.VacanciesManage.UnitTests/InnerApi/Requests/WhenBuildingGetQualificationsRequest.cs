using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetQualificationsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build()
        {
            var actual = new GetQualificationsRequest();

            actual.GetUrl.Should().Be("api/referencedata/candidate-qualifications");
        }
    }
}