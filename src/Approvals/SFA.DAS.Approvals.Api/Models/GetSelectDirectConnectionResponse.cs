using System.Collections.Generic;
using System;
using System.Linq;
using SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;

namespace SFA.DAS.Approvals.Api.Models;
public class GetSelectDirectConnectionResponse
{
    public bool IsLevyAccount { get; set; }
    public IEnumerable<TransferDirectConnection> TransferConnections { get; set; }

    public static implicit operator GetSelectDirectConnectionResponse(GetSelectDirectTransferConnectionQueryResult source)
    {
        return new GetSelectDirectConnectionResponse
        {
            IsLevyAccount = source.IsLevyAccount,
            TransferConnections = source.TransferConnections.Select(x => new TransferDirectConnection
            {
                FundingEmployerAccountId = x.FundingEmployerAccountId,
                FundingEmployerHashedAccountId = x.FundingEmployerHashedAccountId,
                FundingEmployerPublicHashedAccountId = x.FundingEmployerPublicHashedAccountId,
                FundingEmployerAccountName = x.FundingEmployerAccountName,
                ApprovedOn = x.StatusAssignedOn
            })
        };
    }

    public class TransferDirectConnection
    {
        public long FundingEmployerAccountId { get; set; }
        public string FundingEmployerHashedAccountId { get; set; }
        public string FundingEmployerPublicHashedAccountId { get; set; }
        public string FundingEmployerAccountName { get; set; }
        public DateTime? ApprovedOn { get; set; }
    }
}