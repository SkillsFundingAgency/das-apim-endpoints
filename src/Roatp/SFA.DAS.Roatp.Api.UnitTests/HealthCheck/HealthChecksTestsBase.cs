﻿using System;
using System.Net;
using System.Net.Http;

namespace SFA.DAS.Roatp.Api.UnitTests.HealthCheck;

public class HealthChecksTestsBase
{
    protected static HttpResponseMessage ResponseMessageOk => BuildResponseMessage(HttpStatusCode.OK);
    protected static HttpResponseMessage ResponseMessageBadRequest => BuildResponseMessage(HttpStatusCode.BadRequest);

    private static HttpResponseMessage BuildResponseMessage(HttpStatusCode statusCode)
    {
        return new HttpResponseMessage
        {
            StatusCode = statusCode,
            Version = new Version()
        };
    }
}