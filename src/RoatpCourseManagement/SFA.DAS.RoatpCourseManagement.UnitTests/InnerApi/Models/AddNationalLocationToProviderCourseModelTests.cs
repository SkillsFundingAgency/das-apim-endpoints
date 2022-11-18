using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Models
{
    [TestFixture]
    public class AddNationalLocationToProviderCourseModelTests
    {
        [Test, RecursiveMoqAutoData]
        public void Constructor_SetsProperties(string userId, string userDisplayName)
        {
            AddNationalLocationToProviderCourseModel addNationalLocationToProviderCourseModel = new(userId, userDisplayName);

            addNationalLocationToProviderCourseModel.Should().NotBeNull();
            addNationalLocationToProviderCourseModel.UserId.Should().Be(userId);
            addNationalLocationToProviderCourseModel.UserDisplayName.Should().Be(userDisplayName);
        }
    }
}