using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutCreateUserRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Are_Data_Are_Correctly_Constructed(Guid id, UserRequestData data)
        {
            var actual = new PutUpdateUserRequest(id, data);

            actual.PutUrl.Should().Be($"api/users/{id}");
            ((UserRequestData)actual.Data).Should().BeEquivalentTo(data);
        }
    }
}