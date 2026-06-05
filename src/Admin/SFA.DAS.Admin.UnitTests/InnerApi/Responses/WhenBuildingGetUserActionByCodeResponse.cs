using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.InnerApi.Responses;

namespace SFA.DAS.Admin.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class WhenBuildingGetUserActionByCodeResponse
    {
        [Test]
        public void Then_Defaults_are_initialized()
        {
            var resp = new GetUserActionByCodeResponse();

            resp.AdminActions.Should().NotBeNull();
            resp.AdminActions.Should().BeEmpty();
            resp.ActionType.Should().NotBeNull();
            resp.ActionStatus.Should().NotBeNull();
            resp.FamilyName.Should().NotBeNull();
            resp.GivenNames.Should().NotBeNull();
            resp.CertificateType.Should().NotBeNull();
            resp.CourseName.Should().NotBeNull();
        }

        [Test]
        public void Then_Properties_can_be_set_and_read()
        {
            var action = new AdminAction { Username = "tester", ActionTime = new DateTime(2020, 1, 1), Action = "Created" };

            var resp = new GetUserActionByCodeResponse
            {
                Id = 42,
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ActionType = "Type",
                ActionTime = new DateTime(2021, 1, 1),
                ActionStatus = "Status",
                Uln = 1234567890,
                FamilyName = "Smith",
                GivenNames = "John",
                CertificateId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CertificateType = "TypeA",
                CourseName = "Course",
                AdminActions = new System.Collections.Generic.List<AdminAction> { action }
            };

            resp.Id.Should().Be(42);
            resp.UserId.Should().Be(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            resp.AdminActions.Should().ContainSingle().Which.Username.Should().Be("tester");
        }
    }
}
