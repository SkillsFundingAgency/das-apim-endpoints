using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Locations.Queries;
public class WhenConvertingFromGetStandardsListItem
{
    [Test, AutoData]
    public void Then_Converts_StandardsListItem_To_Standard(GetStandardsListItem source)
    {
        var result = (Standard)source;
        result.LarsCode.Should().Be(source.LarsCode.ToString());
        result.Title.Should().Be(source.Title);
    }
}
