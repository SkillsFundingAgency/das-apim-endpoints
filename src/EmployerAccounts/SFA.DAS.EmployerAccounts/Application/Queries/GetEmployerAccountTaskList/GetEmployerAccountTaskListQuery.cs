using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList
{
    public class GetEmployerAccountTaskListQuery : IRequest<GetEmployerAccountTaskListQueryResult>
    {
        public string HashedAccountId { get; set; }
        public long AccountId { get; set; }
        public List<Operation> Operations { get; set; } = new List<Operation> { Operation.Recruitment, Operation.RecruitmentRequiresReview };
    }
}