using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser
{
    public class GetEmployerProfileUserQuery : IRequest<GetEmployerProfileUserResult>
    {
        public Guid UserId { get; set; }
    }
}