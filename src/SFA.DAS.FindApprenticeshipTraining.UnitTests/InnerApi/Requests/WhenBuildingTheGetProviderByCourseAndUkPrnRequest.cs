using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProviderByCourseAndUkPrnRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int providerId, int courseId, double lat, double lon, string sectorSubjectArea, Guid shortlistUserId)
        {
            var actual = new GetProviderByCourseAndUkPrnRequest(providerId, courseId, sectorSubjectArea, lat,lon, shortlistUserId);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?lat={lat}&lon={lon}&sectorSubjectArea={sectorSubjectArea}&shortlistUserId={shortlistUserId}");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_No_Location_Or_ShortListUserId(int providerId, int courseId, string sectorSubjectArea)
        {
            var actual = new GetProviderByCourseAndUkPrnRequest(providerId, courseId, sectorSubjectArea);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?lat=&lon=&sectorSubjectArea={sectorSubjectArea}&shortlistUserId=");
        }
    }
}