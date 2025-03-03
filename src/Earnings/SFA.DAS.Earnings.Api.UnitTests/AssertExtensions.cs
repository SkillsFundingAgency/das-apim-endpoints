using FluentAssertions;

namespace SFA.DAS.Earnings.Api.UnitTests;

public static class AssertExtensions
{
    /// <summary>
    /// Test helper extension that asserts that the object is of the expected type and returns it
    /// </summary>
    /// <typeparam name="T">The type to assert against</typeparam>
    /// <param name="actual">The actual object to assert</param>
    /// <returns>The object cast to the given type</returns>
    /// <exception cref="AssertionException">Fails if the object is not of the given type</exception>
    public static T ShouldBeOfType<T>(this object? actual)
    {
        if (actual == null)
            throw new AssertionException($"Expected object of type {typeof(T).Name} but was null");

        actual.Should().BeOfType<T>();
        return (T)actual;
    }
}