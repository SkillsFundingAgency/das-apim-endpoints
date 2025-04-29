using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingDeleteShortlistItemForUserRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid id, Guid userId)
        {
            //Act
            var actual = new DeleteShortlistItemForUserRequest(id, userId);
            
            //Assert
            actual.DeleteUrl.Should().Be($"api/shortlist/users/{userId}/items/{id}");
        }
    }
}