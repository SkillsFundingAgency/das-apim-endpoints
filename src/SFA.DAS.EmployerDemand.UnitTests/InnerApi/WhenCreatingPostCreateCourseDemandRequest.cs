using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingPostCreateCourseDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(CreateCourseDemandData data)
        {
            var actual = new PostCreateCourseDemandRequest(data);
            
            actual.Data.Should().BeEquivalentTo(data);
            actual.PostUrl.Should().Be($"api/demand/{data.Id}");
        }
    }
}