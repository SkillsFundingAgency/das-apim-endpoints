using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public string GetUrl => $"providers/{Ukprn}";
        public int Ukprn { get; }
      
        public GetProviderQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
