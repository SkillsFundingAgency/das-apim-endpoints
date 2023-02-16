using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutUpsertEmployerUserAccountRequest
    {
        [Test, AutoData]
        public void Then_The_Url_And_Data_Is_Constructed_Correctly(Guid userId, string id, string email, string firstName, string lastName)
        {
            var actual = new PutUpsertEmployerUserAccountRequest(userId,id, email, firstName, lastName);

            actual.PutUrl.Should().Be($"api/users/{userId}");
            actual.Data.Should().BeEquivalentTo(new
            {
                GovUkIdentifier = id,
                FirstName = firstName, 
                LastName = lastName,
                Email = email
            });
        }
    }
}