﻿using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory
{
    public class GetJobsQuery : IRequest<GetJobsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
