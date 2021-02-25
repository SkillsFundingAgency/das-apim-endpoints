using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingThePostShortlistForUserRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_With_Data(PostShortlistData source)
        {
            var actual = new PostShortlistForUserRequest
            {
                Data = source
            };
            
            actual.Data.Should().BeEquivalentTo(source);
            actual.PostUrl.Should().Be("api/shortlist");
        }
    }
}