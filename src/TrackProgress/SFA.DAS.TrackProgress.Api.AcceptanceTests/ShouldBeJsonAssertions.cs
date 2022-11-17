using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Text.Json;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests;

public static class ShouldBeJsonAssertions
{
    [CustomAssertion]
    public static AndWhichConstraint<StringAssertions, T> BeValidJson<T>(
                this StringAssertions stringAssertions,
                string because = "",
                params object[] becauseArgs)
    {
        try
        {
            var obj = JsonSerializer.Deserialize<T>(stringAssertions.Subject);
            return new AndWhichConstraint<StringAssertions, T>(stringAssertions, obj!);
        }
        catch (Exception ex)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} to be deserialise to {0}{reason}, but parsing failed with {1}.\nJSON: {2}",
                typeof(T).Name, ex.Message, stringAssertions.Subject);
            return new AndWhichConstraint<StringAssertions, T>(stringAssertions, Array.Empty<T>());
        }
    }
}