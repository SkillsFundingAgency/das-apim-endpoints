using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Campaign.Api.UnitTests
{
    public static class AssertExtensions
    {
        public static void AssertThatTheObjectResultIsValid(this ObjectResult result)
        {
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        public static void AssertThatTheObjectValueIsValid<T>(this ObjectResult result)
        {
            var value = (T) result.Value;
            value.Should().NotBeNull();
        }

        public static void AssertThatTheNotFoundObjectResultIsValid(this NotFoundObjectResult result)
        {
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        public static void AssertThatTheNotFoundObjectResultValueIsValid<T>(this NotFoundObjectResult result)
        {
            var value = (T)result.Value;
            value.Should().NotBeNull();
        }
    }
}
