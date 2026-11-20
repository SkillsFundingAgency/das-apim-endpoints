using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.MsaBrokerage.Api.AppStart;
using SFA.DAS.MsaBrokerage.Configuration;

namespace SFA.DAS.MsaBrokerage.Api.UnitTests.AppStart;

public class WhenAddingConfigurationOptions
{
    [Test]
    public void Then_The_MsaBrokerage_Configuration_Is_Bound()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "MsaBrokerageConfiguration:DefaultPageSize", "25" },
                { "MsaBrokerageConfiguration:MaxPageSize", "75" }
            })
            .Build();
        var services = new ServiceCollection();

        services.AddConfigurationOptions(configuration);
        var provider = services.BuildServiceProvider();

        var actual = provider.GetRequiredService<MsaBrokerageConfiguration>();
        actual.DefaultPageSize.Should().Be(25);
        actual.MaxPageSize.Should().Be(75);
    }
}
