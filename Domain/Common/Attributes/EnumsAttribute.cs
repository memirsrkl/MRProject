namespace Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class EnumsAttributes : Attribute
{
    public string Name { get; }
    public string Color { get; }

    public EnumsAttributes(string name, string color)
    {
        Name = name;
        Color = color;
    }
}