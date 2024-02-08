using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.Apprenticeships.Api.UnitTests
{
    public static class AssertExtensions
    {
        //extension that asserts that the object is of the expected type and returns it
        public static T ShouldBeOfType<T>(this object? actual)
        {
            if (actual == null)
                throw new AssertionException($"Expected object of type {typeof(T).Name} but was null");

            actual.Should().BeOfType<T>();
            return (T)actual;
        }

    }
}
