using MediatR;
using SFA.DAS.Payments.Models.Responses;

namespace SFA.DAS.Payments.Application.Learners
{
    public class GetLearnersQuery : IRequest<IEnumerable<LearnerResponse>>
    {
        public int Ukprn { get; set; }
        public int AcademicYear { get; set; }
        public GetLearnersQuery(int ukprn, int academicYear)
        {
            Ukprn = ukprn;
            AcademicYear = academicYear;
        }
    }

    public class GetLearnersQueryHandler : IRequestHandler<GetLearnersQuery, IEnumerable<LearnerResponse>>
    {
        public Task<IEnumerable<LearnerResponse>> Handle(GetLearnersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<LearnerResponse> foo = new List<LearnerResponse> { new LearnerResponse { Uln = 1133, LearnRefNumber = "abc123" } };
            return Task.FromResult(foo);
        }
    }
}
