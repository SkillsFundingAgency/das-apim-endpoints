﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Exceptions;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.Exceptions
{
    public class WhenConstructingApiResponseException
    {
        [Test]
        public void And_Using_Message_Ctor_Then_Sets_Message_From_Param()
        {
            var exception = new ApiResponseException(System.Net.HttpStatusCode.BadRequest, "something went wrong");

            exception.Message.Should().Be($"HTTP status code did not indicate success: 400 BadRequest");
        }

        [Test, AutoData]
        public void And_Using_Message_And_InnerException_Ctor_Then_Sets_Message_From_Param(
            Exception innerException)
        {
            var exception = new ApiResponseException(System.Net.HttpStatusCode.MethodNotAllowed, "something went wrong", innerException);

            exception.Message.Should().Be($"HTTP status code did not indicate success: 405 MethodNotAllowed");
            exception.InnerException.Should().Be(innerException);
        }
    }
}