﻿using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Hub
{
    public class GetHubQuery : IRequest<GetHubQueryResult>
    {
        public string Hub { get; set; }
    }
}
