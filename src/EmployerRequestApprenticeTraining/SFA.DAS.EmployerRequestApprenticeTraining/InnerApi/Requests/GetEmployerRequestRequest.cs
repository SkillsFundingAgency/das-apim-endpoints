﻿using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    [ExcludeFromCodeCoverage]
    public class GetEmployerRequestRequest : IGetApiRequest
    {
        public Guid EmployerRequestId { get; set; }

        public GetEmployerRequestRequest(Guid employerRequestId)
        {
            EmployerRequestId = employerRequestId;
        }

        public string GetUrl => $"api/employerrequest/{EmployerRequestId}";
    }
}
