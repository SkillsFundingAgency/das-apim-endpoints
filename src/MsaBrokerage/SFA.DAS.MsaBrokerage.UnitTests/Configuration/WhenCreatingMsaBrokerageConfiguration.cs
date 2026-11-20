using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.MsaBrokerage.Configuration;

namespace SFA.DAS.MsaBrokerage.UnitTests.Configuration;

public class WhenCreatingMsaBrokerageConfiguration
{
    [Test]
    public void Then_The_Paging_Defaults_Are_Set()
    {
        var configuration = new MsaBrokerageConfiguration();

        configuration.DefaultPageSize.Should().Be(MsaBrokerageConfiguration.DefaultPageSizeValue);
        configuration.MaxPageSize.Should().Be(MsaBrokerageConfiguration.MaxPageSizeValue);
    }
}
