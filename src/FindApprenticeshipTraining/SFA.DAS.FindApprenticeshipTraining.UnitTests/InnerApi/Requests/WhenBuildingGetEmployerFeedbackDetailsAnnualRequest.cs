﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class WhenBuildingGetEmployerFeedbackDetailsAnnualRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
    {
        //Act
        var sut = new GetEmployerFeedbackDetailsAnnualRequest(ukprn);

        //Assert
        sut.GetUrl.Should().Be($"api/EmployerFeedbackResult/{ukprn}/annual");
    }
}