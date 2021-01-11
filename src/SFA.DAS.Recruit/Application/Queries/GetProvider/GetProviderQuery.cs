﻿using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderQueryResult>
    {
        public int Id { get; set; }
    
    }
}