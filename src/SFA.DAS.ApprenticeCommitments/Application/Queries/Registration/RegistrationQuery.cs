using System;
using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Registration
{
    public class RegistrationQuery : IRequest<RegistrationResponse>
    {
        public Guid ApprenticeshipId { get; set; }
    }
}