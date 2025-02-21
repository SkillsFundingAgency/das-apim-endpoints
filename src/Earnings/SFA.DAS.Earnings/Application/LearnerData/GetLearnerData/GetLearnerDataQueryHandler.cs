using MediatR;
using SFA.DAS.Earnings.Api.Learnerdata;

namespace SFA.DAS.Earnings.Application.LearnerData.GetLearnerData;

public class GetLearnerDataQueryHandler(ILearnerDataStore datastore)
    : IRequestHandler<GetLearnerDataQuery, GetLearnerDataQueryResult>
{
    public Task<GetLearnerDataQueryResult> Handle(GetLearnerDataQuery request, CancellationToken cancellationToken)
    {
        var results = datastore.Search(request.Ukprn, request.AcademicYear, request.Page, request.PageSize);
        
        var totalRecords = datastore.Count(request.Ukprn, request.AcademicYear);
        
        return Task.FromResult(new GetLearnerDataQueryResult
        {
            Apprenticeships = results.ConvertAll(uln => new ApprenticeshipResult
            {
                Uln = uln
            }),
            TotalRecords = totalRecords
        });
    }
}