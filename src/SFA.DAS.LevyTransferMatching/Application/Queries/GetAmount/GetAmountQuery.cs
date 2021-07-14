﻿using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAmount
{
    public class GetAmountQuery : IRequest<GetAmountQueryResult>
    {
        public string EncodedAccountId { get; set; }
    }
}
