﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance
{
    public class GetTransferConnectionsResponse
    {
        public IEnumerable<TransferConnection> TransferConnections { get; set; }
        public class TransferConnection
        {
            public long FundingEmployerAccountId { get; set; }
            public string FundingEmployerHashedAccountId { get; set; }
            public string FundingEmployerPublicHashedAccountId { get; set; }
            public string FundingEmployerAccountName { get; set; }
            public short? Status { get; set; }
            public DateTime? StatusAssignedOn { get; set; }
        }
    }
}