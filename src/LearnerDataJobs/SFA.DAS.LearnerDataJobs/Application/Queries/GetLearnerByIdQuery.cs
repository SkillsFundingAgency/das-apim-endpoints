using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LearnerDataJobs.Application.Queries
{
    public record GetLearnerByIdQuery(long ukprn, long Id) : IRequest<GetLearnerByIdResult>;
}
