using MediatR;

namespace SFA.DAS.Earnings.Application.ApprovedApprenticeships.GetApprovedApprenticeships;

public class GetApprovedApprenticeshipsQueryHandler(IApprovedApprenticeshipsStore datastore)
    : IRequestHandler<GetApprovedApprenticeshipsQuery, GetApprovedApprenticeshipsQueryResult>
{
    public Task<GetApprovedApprenticeshipsQueryResult> Handle(GetApprovedApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        var results = datastore.Search(request.Ukprn, request.AcademicYear, request.Page, request.PageSize);
        
        var totalRecords = datastore.Count(request.Ukprn, request.AcademicYear);
        
        return Task.FromResult(new GetApprovedApprenticeshipsQueryResult
        {
            Apprenticeships = results.ConvertAll(uln => new ApprenticeshipResult
            {
                Uln = uln
            }),
            TotalRecords = totalRecords
        });
    }
}