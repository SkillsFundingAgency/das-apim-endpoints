using System.ComponentModel;

namespace SFA.DAS.LearnerDataJobs.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes != null && attributes.Length > 0)
        {
            return attributes[0].Description;
        }

        return value.ToString();
    }

    public static TEnum FromDescription<TEnum>(string description) where TEnum : struct, Enum
    {
        foreach (var field in typeof(TEnum).GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute?.Description == description)
                return (TEnum)field.GetValue(null)!;

            // Optional: also allow matching the enum name
            if (field.Name == description)
                return (TEnum)field.GetValue(null)!;
        }

        throw new ArgumentException($"'{description}' is not a valid description for {typeof(TEnum).Name}");
    }

    public static List<string> GetEnumDescriptions<TEnum>() where TEnum : Enum
    {
        return typeof(TEnum)
            .GetFields()
            .Where(f => f.IsLiteral)
            .Select(f =>
            {
                var attr = f.GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .Cast<DescriptionAttribute>()
                            .FirstOrDefault();

                return attr?.Description ?? f.Name;
            })
            .ToList();
    }
}