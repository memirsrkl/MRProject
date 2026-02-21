using System.Reflection;
using Domain.Common.Attributes;

namespace Domain.Common.Extensions;

public static class EnumExtensions
{
    public static EnumsAttributes? GetAttributes(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        return field?.GetCustomAttribute<EnumsAttributes>();
    }

    public static string? GetName(this Enum value)
        => value.GetAttributes()?.Name;

    public static string? GetColor(this Enum value)
        => value.GetAttributes()?.Color;
}