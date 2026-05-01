using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries;

public record GetRegisteredProvidersQuery : IRequest<ApiResponse<RegisteredProviderResponse>>;
