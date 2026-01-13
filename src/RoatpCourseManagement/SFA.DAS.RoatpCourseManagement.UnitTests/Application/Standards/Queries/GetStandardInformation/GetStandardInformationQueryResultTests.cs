using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetStandardInformation;

[TestFixture]
public class GetStandardInformationQueryResultTests
{
    [Test, AutoData]
    public void Operator_TransformsFromApiModel(GetStandardForLarsCodeResponse source)
    {
        GetStandardInformationQueryResult sut = source;

        sut.Should().BeEquivalentTo(source, option =>
        {
            option.Excluding(s => s.Version);
            return option;
        });

        sut.LarsCode.Should().Be(source.LarsCode.ToString());
    }
}