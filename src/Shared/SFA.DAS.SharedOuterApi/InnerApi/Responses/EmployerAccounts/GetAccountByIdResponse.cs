﻿using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts
{
    public class GetAccountByIdResponse
    {
        public long AccountId { get; set; }
        public string HashedAccountId { get; set; }
        public string PublicHashedAccountId { get; set; }
        public string DasAccountName { get; set; }
        public DateTime DateRegistered { get; set; }
        public string OwnerEmail { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApprenticeshipEmployerType
    {
        NonLevy = 0,
        Levy = 1,
        Unknown = 2,
    }
}
