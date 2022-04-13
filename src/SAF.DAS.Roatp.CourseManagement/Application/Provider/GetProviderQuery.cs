using MediatR;

namespace SFA.DAS.Roatp.CourseManagement.Application.Provider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public GetProviderQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
        public int Ukprn { get; }
    }
}