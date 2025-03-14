using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class WhenBuildingDeleteShortlistItemRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(Guid id)
    {
        //Act
        var sut = new DeleteShortlistItemRequest(id);

        //Assert
        sut.DeleteUrl.Should().Be($"api/shortlists/{id}");
    }
}