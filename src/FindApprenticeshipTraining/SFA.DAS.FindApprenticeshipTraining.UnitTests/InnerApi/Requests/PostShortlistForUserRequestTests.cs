using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class PostShortlistForUserRequestTests
{
    [Test, AutoData]
    public void WhenBuildingPostShortlistForUserRequest_ReturnsExpectedUrlWithData(PostShortlistData source)
    {
        var actual = new PostShortlistForUserRequest
        {
            Data = source
        };

        actual.Data.Should().BeEquivalentTo(source);
        actual.PostUrl.Should().Be("shortlists");
    }
}
