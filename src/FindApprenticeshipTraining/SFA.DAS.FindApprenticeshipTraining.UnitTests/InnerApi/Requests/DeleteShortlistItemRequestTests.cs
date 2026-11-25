using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class DeleteShortlistItemRequestTests
{
    [Test, AutoData]
    public void WhenBuildingDeleteShortlistItemRequest_ReturnsExpectedUrl(Guid id)
    {
        //Act
        var sut = new DeleteShortlistItemRequest(id);

        //Assert
        sut.DeleteUrl.Should().Be($"shortlists/{id}");
    }
}