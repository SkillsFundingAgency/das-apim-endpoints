using MediatR;
using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserType : IRequest<List<ProfileModel>>
    {
        public GetProfilesByUserType(string userType) => UserType = userType;
        public string UserType { get; }
    }
}
