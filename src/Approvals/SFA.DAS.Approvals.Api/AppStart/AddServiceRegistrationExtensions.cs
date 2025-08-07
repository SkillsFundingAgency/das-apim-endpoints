﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Approvals.Api.Clients;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Approvals.Api.AppStart;

public static class AddServiceRegistrationExtensions
{
    private static void AddCommitmentApiInternalClient(IServiceCollection services, IConfiguration configuration)
    {
        bool useLocalDevClient = configuration.IsLocalOrDev() && configuration["UseLocalDevCommitmentApiClient"] == "true";

        if (useLocalDevClient)
        {
            services.AddTransient<IInternalApiClient<CommitmentsV2ApiConfiguration>, LocalCommitmentsApiInternalApiClient>();
        }
        else
        {
            services.AddTransient<IInternalApiClient<CommitmentsV2ApiConfiguration>, CommitmentsApiInternalApiClient>();
        }
    }

    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
            
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient(typeof(ITokenPassThroughInternalApiClient<>), typeof(TokenPassThroughInternalApiClient<>));
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<IFjaaApiClient<FjaaApiConfiguration>, FjaaApiClient>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IAssessorsApiClient<AssessorsApiConfiguration>, AssessorsApiClient>();
        services.AddTransient<IRoatpServiceApiClient<RoatpConfiguration>, RoatpServiceApiClient>();
        services.AddTransient<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>, ApprenticeCommitmentsApiClient>();
        services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
        services.AddTransient<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>, LevyTransferMatchingApiClient>();
        services.AddTransient<IProviderAccountApiClient<ProviderAccountApiConfiguration>, ProviderAccountApiClient>();
        services.AddTransient<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>, ProviderCoursesApiClient>();
        services.AddTransient<IProviderPaymentEventsApiClient<ProviderEventsConfiguration>, ProviderPaymentEventsApiClient>();
        services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
        services.AddTransient<ILearningApiClient<LearningApiConfiguration>, LearningApiClient>();
        services.AddTransient<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>, CollectionCalendarApiClient>();
        services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        services.AddTransient<ICourseTypesApiClient, CourseTypesApiClient>();
        AddCommitmentApiInternalClient(services, configuration);
        services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
        services.AddTransient<IReservationApiClient<ReservationApiConfiguration>, ReservationApiClient>();
        services.AddTransient<IFinanceApiClient<FinanceApiConfiguration>, FinanceApiClient>();
        services.AddTransient<IDeliveryModelService, DeliveryModelService>();
        services.AddTransient<IFjaaService, FjaaService>();
        services.AddTransient<ITrainingProviderService, TrainingProviderService>();
        services.AddTransient<IProviderStandardsService, ProviderStandardsService>();
        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
        services.AddTransient<ICacheStorageService, CacheStorageService>();
        services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
        services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();
        services.AddTransient<IAutoReservationsService, AutoReservationsService>();
        services.AddTransient<ICourseTypeRulesService, CourseTypeRulesService>();
        services.AddTransient<IBulkCourseMetadataService, BulkCourseMetadataService>();
        services.AddSingleton<IMapLearnerRecords, MapLearnerRecords>();
        services.AddTransient<IAddCourseTypeDataToCsvService, AddCourseTypeDataToCsvService>();
        services.AddServiceParameters();
    }
}