using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPutAccountUserRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Built_And_Data_Passed(string userRef, string email, string firstName, string lastName, Guid correlationId)
        {
            var actual = new PutAccountUserRequest(userRef, email, firstName, lastName, correlationId);

            actual.PutUrl.Should().Be("api/user/upsert");
            actual.Data.GetType().GetProperty("UserRef")!.GetValue(actual.Data, null).Should().Be(userRef);
            actual.Data.GetType().GetProperty("EmailAddress")!.GetValue(actual.Data, null).Should().Be(email);
            actual.Data.GetType().GetProperty("FirstName")!.GetValue(actual.Data, null).Should().Be(firstName);
            actual.Data.GetType().GetProperty("LastName")!.GetValue(actual.Data, null).Should().Be(lastName);
            actual.Data.GetType().GetProperty("CorrelationId")!.GetValue(actual.Data, null).Should().Be(correlationId.ToString());
        }
    }
}