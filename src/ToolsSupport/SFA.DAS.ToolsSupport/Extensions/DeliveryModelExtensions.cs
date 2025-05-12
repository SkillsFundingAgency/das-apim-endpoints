using static SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.ToolsSupport.Extensions;

public static class DeliveryModelExtensions
{
    public static string? ToDescription(this DeliveryModel? deliveryModel) => deliveryModel?.ToDescription(); 

    public static string ToDescription(this DeliveryModel deliveryModel) =>
        deliveryModel switch
        {
            DeliveryModel.PortableFlexiJob => "Portable flexi-job",
            DeliveryModel.FlexiJobAgency => "Flexi-job agency",
            _ => "Regular"
        };
}