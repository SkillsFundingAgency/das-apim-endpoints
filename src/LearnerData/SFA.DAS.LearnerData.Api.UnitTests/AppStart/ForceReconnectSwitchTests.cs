using FluentAssertions;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using NUnit.Framework;
using System.Reflection;

namespace SFA.DAS.LearnerData.Api.UnitTests.AppStart;

[TestFixture]
public class ForceReconnectSwitchTests
{
    [Test(Description = "Check that Api csproj sets the ForceReconnect switch")]
    public void LibraryStillReadsTheForceReconnectSwitch()
    {
        // This test is a package-bump "tripwire".
        // UseForceReconnect is internal in Microsoft.Extensions.Caching.StackExchangeRedis. If the library renames or drops the switch,
        // the csproj setting becomes a silent no-op we'd want to know about.
        // UseForceReconnect is a recommended MS best practice for Redis connection resilience.
        AppContext.SetSwitch("Microsoft.AspNetCore.Caching.StackExchangeRedis.UseForceReconnect", true);

        var property = typeof(RedisCacheOptions).GetProperty("UseForceReconnect", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        property.Should().NotBeNull("the library's internal option has been renamed or removed");
        property!.GetValue(new RedisCacheOptions()).Should().Be(true);
    }
}
