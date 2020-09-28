using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetLocationByLocationRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string locationName, string authorityName)
        {
            var actual = new GetLocationByLocationName(locationName);

            actual.GetUrl.Should()
                .Be($"api/locations?locationName={locationName}");
        }

        [Test, InlineAutoData("test/*6]'est&#", "test%'*'/@")]
        public void Then_The_Request_Is_Correctly_Encoded_For_Special_Characters(string locationName,
            string authorityName)
        {
            var actual = new GetLocationByLocationName($"{locationName}&{locationName}");

            actual.GetUrl.Should()
                .Be($"api/locations?locationName={HttpUtility.UrlEncode($"{locationName}&{locationName}")}");
        }
    }
}
